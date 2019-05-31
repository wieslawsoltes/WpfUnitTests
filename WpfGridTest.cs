using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Xunit.Abstractions;

namespace WpfGridTest
{
    public class WpfGridTest
    { 
        private readonly ITestOutputHelper output;

        public WpfGridTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        private void PrintColumnDefinitions(Grid grid)
        {
            output.WriteLine($"[Grid] ActualWidth: {grid.ActualWidth} ActualHeight: {grid.ActualHeight}");
            output.WriteLine($"[ColumnDefinitions]");
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var cd = grid.ColumnDefinitions[i];
                output.WriteLine($"[{i}] ActualWidth: {cd.ActualWidth} SharedSizeGroup: {cd.SharedSizeGroup}");
            }
        }

        // [WpfFact]
        // public void Size_Propagation_Is_Constrained_To_Innermost_Scope()
        // {
        //     var grids = new[] { CreateGrid("A", null), CreateGrid(("A", new GridLength(30)), (null, new GridLength())) };
        //     var innerScope = new Grid();
        //     foreach(var xgrids in grids)
        //     innerScope.Children.Add(xgrids);
        //     innerScope.SetValue(Grid.IsSharedSizeScopeProperty, true);

        //     var outerGrid = CreateGrid(("A", new GridLength(0)));
        //     var outerScope = new Grid();
        //     outerScope.Children.Add(outerGrid);
        //     outerScope.Children.Add(innerScope);

        //     var root = new Grid();
        //     root.SetValue(Grid.IsSharedSizeScopeProperty, true);
        //     root.Children.Add(outerScope);

        //     root.Measure(new Size(50, 50));
        //     root.Arrange(new Rect(new Point(), new Point(50, 50)));
        //     Assert.Equal(1, outerGrid.ColumnDefinitions[0].ActualWidth);
        // }

        [WpfFact]
        public void Size_Group_Changes_Are_Tracked()
        {
            var grids = new[] {
                CreateGrid((null, new GridLength(0, GridUnitType.Auto)), (null, new GridLength())),
                CreateGrid(("A", new GridLength(30)), (null, new GridLength())) };
            var scope = new Grid();
            foreach(var xgrids in grids)
            scope.Children.Add(xgrids);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
                        root.Children.Add(scope);

            root.Measure(new Size(50, 50));
            root.Arrange(new Rect(new Point(), new Point(50, 50)));
            PrintColumnDefinitions(grids[0]);
            Assert.Equal(0, grids[0].ColumnDefinitions[0].ActualWidth);

            grids[0].ColumnDefinitions[0].SharedSizeGroup = "A";

            root.Measure(new Size(51, 51));
            root.Arrange(new Rect(new Point(), new Point(51, 51)));
            PrintColumnDefinitions(grids[0]);
            Assert.Equal(30, grids[0].ColumnDefinitions[0].ActualWidth);

            grids[0].ColumnDefinitions[0].SharedSizeGroup = null;

            root.Measure(new Size(52, 52));
            root.Arrange(new Rect(new Point(), new Point(52, 52)));
            PrintColumnDefinitions(grids[0]);
            Assert.Equal(0, grids[0].ColumnDefinitions[0].ActualWidth);
        }

        [WpfFact]
        public void Collection_Changes_Are_Tracked()
        {
            var grid = CreateGrid(
                ("A", new GridLength(20)),
                ("A", new GridLength(30)),
                ("A", new GridLength(40)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
                        root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(40, cd.ActualWidth));

            grid.ColumnDefinitions.RemoveAt(2);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions.Insert(1, new ColumnDefinition { Width = new GridLength(30), SharedSizeGroup = "A" });

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(10), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(50), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
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
            PrintColumnDefinitions(grid);
            // all in group are equal to the first fixed column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(19, cd.ActualWidth -1));

            grid.ColumnDefinitions[0].SharedSizeGroup = null;

            grid.Measure(new Size(100, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            PrintColumnDefinitions(grid);
            // all in group are equal to width (MinWidth) of the sizer in the second column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(20, cd.ActualWidth));

            grid.ColumnDefinitions[1].SharedSizeGroup = null;

            grid.Measure(new Size(double.PositiveInfinity, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            PrintColumnDefinitions(grid);
            // with no constraint star columns default to the MinWidth of the sizer in the column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
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
            foreach(var k in                     columns.Select(c => new ColumnDefinition
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
