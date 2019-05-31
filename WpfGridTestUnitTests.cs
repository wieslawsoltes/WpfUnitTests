using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xunit;

namespace WpfGridTest
{
    public class WpfGridTestUnitTests
    {
        public WpfGridTestUnitTests()
        {
            TestRoot = new ContentControl();
            Size_Group_Changes_Are_Tracked();
            TestRoot = new ContentControl();
            Size_Priorities_Are_Maintained();
            TestRoot = new ContentControl();
            Collection_Changes_Are_Tracked();
        }

        [Fact]
        public void Size_Group_Changes_Are_Tracked()
        {
            var grids = new[] {
                CreateGrid((null, new GridLength(0, GridUnitType.Auto)), (null, new GridLength())),
                CreateGrid(("A", new GridLength(30)), (null, new GridLength())) };
            var scope = new Grid();
            foreach (var xgrids in grids)
                scope.Children.Add(xgrids);

            var root = TestRoot;

            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Content = scope;

            root.Measure(new Size(50, 50));
            root.Arrange(new Rect(new Point(), new Point(50, 50)));
            Assert.Equal(0, grids[0].ColumnDefinitions[0].ActualWidth);

            grids[0].ColumnDefinitions[0].SharedSizeGroup = "A";

            root.Measure(new Size(51, 51));
            root.Arrange(new Rect(new Point(), new Point(51, 51)));
            AssertEqual(30, grids[0].ColumnDefinitions[0].ActualWidth);

            grids[0].ColumnDefinitions[0].SharedSizeGroup = null;

            root.Measure(new Size(52, 52));
            root.Arrange(new Rect(new Point(), new Point(52, 52)));
            Assert.Equal(0, grids[0].ColumnDefinitions[0].ActualWidth);
        }

        [Fact]
        public void Collection_Changes_Are_Tracked()
        {
            var grid = CreateGrid(
                ("A", new GridLength(20)),
                ("A", new GridLength(30)),
                ("A", new GridLength(40)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = TestRoot;
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Content = scope;

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(40, cd.ActualWidth));

            grid.ColumnDefinitions.RemoveAt(2);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions.Insert(1, new ColumnDefinition { Width = new GridLength(35), SharedSizeGroup = "A" });

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(35, cd.ActualWidth));

            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(10), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth), true);

            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(50), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth), true);
        }

        [Fact]
        public void Size_Priorities_Are_Maintained()
        {
            var sizers = new List<Control>();
            var grid = CreateGrid(
                ("A", new GridLength(20)),
                ("A", new GridLength(20, GridUnitType.Auto)),
                ("A", new GridLength(1, GridUnitType.Star)),
                ("A", new GridLength(1, GridUnitType.Star)),
                (null, new GridLength()));
            for (int i = 0; i < 3; i++)
                sizers.Add(AddSizer(grid, i, 6 + i * 6));
            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(100, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            // all in group are equal to the first fixed column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(20, cd.ActualWidth));

            grid.ColumnDefinitions[0].SharedSizeGroup = null;

            grid.Measure(new Size(100, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            // all in group are equal to width (MinWidth) of the sizer in the second column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(6 + 1 * 6, cd.ActualWidth), true);

            grid.ColumnDefinitions[1].SharedSizeGroup = null;

            grid.Measure(new Size(double.PositiveInfinity, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            // with no constraint star columns default to the MinWidth of the sizer in the column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(6 + 2 * 6, cd.ActualWidth), true);
        }

        // grid creators
        // private Grid CreateGrid(params string[] columnGroups)
        // {
        //     return CreateGrid(columnGroups.Select(s => (s, (double)ColumnDefinition.WidthProperty.DefaultMetadata.DefaultValue)).ToArray());
        // }  

        private Grid CreateGrid(params (string name, GridLength width)[] columns)
        {
            return CreateGrid(columns.Select(c =>
                (c.name, c.width, (double)ColumnDefinition.MinWidthProperty.DefaultMetadata.DefaultValue)).ToArray());
        }

        private Grid CreateGrid(params (string name, GridLength width, double minWidth)[] columns)
        {
            return CreateGrid(columns.Select(c =>
                (c.name, c.width, c.minWidth, (double)ColumnDefinition.MaxWidthProperty.DefaultMetadata.DefaultValue)).ToArray());
        }

        private Grid CreateGrid(params (string name, GridLength width, double minWidth, double maxWidth)[] columns)
        {

            var grid = new Grid();
            foreach (var k in columns.Select(c => new ColumnDefinition
            {
                SharedSizeGroup = c.name,
                Width = c.width,
                MinWidth = c.minWidth,
                MaxWidth = c.maxWidth
            }))
                grid.ColumnDefinitions.Add(k);

            return grid;
        }

        private Control AddSizer(Grid grid, int column, double size = 30)
        {
            var ctrl = new Control { MinWidth = size, MinHeight = size };
            ctrl.SetValue(Grid.ColumnProperty, column);
            grid.Children.Add(ctrl);
            return ctrl;
        }
    }
}
