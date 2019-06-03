using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xunit;
using Xunit.Abstractions;

namespace WpfUnitTests
{
    public class GridTests
    {
        private readonly ITestOutputHelper output;

        public GridTests(ITestOutputHelper output)
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
            output.WriteLine($"[RowDefinitions]");
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var rd = grid.ColumnDefinitions[i];
                output.WriteLine($"[{i}] ActualWidth: {rd.ActualWidth} SharedSizeGroup: {rd.SharedSizeGroup}");
            }
        }

        [WpfFact]
        public void Grid_GridLength_Same_Size_Pixel_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                (null, new GridLength()),
                (null, new GridLength()),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, false);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == null), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void Grid_GridLength_Same_Size_Pixel_50()
        {
            var grid = CreateGrid(
                (null, new GridLength(50)),
                (null, new GridLength(50)),
                (null, new GridLength(50)),
                (null, new GridLength(50)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, false);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == null), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void Grid_GridLength_Same_Size_Auto()
        {
            var grid = CreateGrid(
                (null, new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength(0, GridUnitType.Auto)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, false);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == null), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void Grid_GridLength_Same_Size_Star()
        {
            var grid = CreateGrid(
                (null, new GridLength(1, GridUnitType.Star)),
                (null, new GridLength(1, GridUnitType.Star)),
                (null, new GridLength(1, GridUnitType.Star)),
                (null, new GridLength(1, GridUnitType.Star)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, false);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == null), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0()
        {
            var grid = CreateGrid(
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50()
        {
            var grid = CreateGrid(
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto()
        {
            var grid = CreateGrid(
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star()
        {
            var grid = CreateGrid(
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)));  // Star sizing is treated as Auto, 1 is ignored

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_First_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_First_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_First_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_First_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star))); // Star sizing is treated as Auto, 1 is ignored

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_Last_Column_0()
        {
            var grid = CreateGrid(
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_Last_Column_0()
        {
            var grid = CreateGrid(
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_Last_Column_0()
        {
            var grid = CreateGrid(
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_Last_Column_0()
        {
            var grid = CreateGrid(
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_First_And_Last_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                ("A", new GridLength()),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_First_And_Last_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_First_And_Last_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_First_And_Last_Column_0()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength()),
                ("B", new GridLength()),
                ("B", new GridLength()),
                ("A", new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(25)),
                ("B", new GridLength(75)),
                ("B", new GridLength(75)),
                ("A", new GridLength(25)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)));  // Star sizing is treated as Auto, 1 is ignored

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_First_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength()),
                ("B", new GridLength()),
                ("B", new GridLength()),
                ("A", new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_First_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(25)),
                ("B", new GridLength(75)),
                ("B", new GridLength(75)),
                ("A", new GridLength(25)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_First_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_First_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star))); // Star sizing is treated as Auto, 1 is ignored

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength()),
                ("B", new GridLength()),
                ("B", new GridLength()),
                ("A", new GridLength()),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(25)),
                ("B", new GridLength(75)),
                ("B", new GridLength(75)),
                ("A", new GridLength(25)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_0_First_And_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength()),
                ("B", new GridLength()),
                ("B", new GridLength()),
                ("A", new GridLength()),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Pixel_50_First_And_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(25)),
                ("B", new GridLength(75)),
                ("B", new GridLength(75)),
                ("A", new GridLength(25)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Auto_First_And_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("B", new GridLength(0, GridUnitType.Auto)),
                ("A", new GridLength(0, GridUnitType.Auto)),
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
        }

        [WpfFact]
        public void SharedSize_Grid_GridLength_Same_Size_Star_First_And_Last_Column_0_Two_Groups()
        {
            var grid = CreateGrid(
                (null, new GridLength()),
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("B", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                ("A", new GridLength(1, GridUnitType.Star)), // Star sizing is treated as Auto, 1 is ignored
                (null, new GridLength()));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(0, cd.ActualWidth));
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
            foreach (var xgrids in grids)
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

            grid.ColumnDefinitions.RemoveAt(2); // ("A", new GridLength(40))

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions.Insert(1, new ColumnDefinition { Width = new GridLength(30), SharedSizeGroup = "A" });

            // NOTE: THIS IS BROKEN IN WPF
            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));

            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(10), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(30, cd.ActualWidth));
            /* output after previous line:
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 40 SharedSizeGroup: A
            [1] ActualWidth: 40 SharedSizeGroup: A
            [2] ActualWidth: 40 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 30 SharedSizeGroup: A
            [1] ActualWidth: 30 SharedSizeGroup: A
            [2] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 30 SharedSizeGroup: A
            [1] ActualWidth: 30 SharedSizeGroup: A
            [2] ActualWidth: 30 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 0 SharedSizeGroup: A
            [1] ActualWidth: 60 SharedSizeGroup: A
            [2] ActualWidth: 30 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            */

            // NOTE: THIS IS BROKEN IN WPF
            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(50), SharedSizeGroup = "A" };

            // NOTE: THIS IS BROKEN IN WPF
            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            /* output after previous line:
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 40 SharedSizeGroup: A
            [1] ActualWidth: 40 SharedSizeGroup: A
            [2] ActualWidth: 40 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 30 SharedSizeGroup: A
            [1] ActualWidth: 30 SharedSizeGroup: A
            [2] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 30 SharedSizeGroup: A
            [1] ActualWidth: 30 SharedSizeGroup: A
            [2] ActualWidth: 30 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 200 ActualHeight: 200
            [ColumnDefinitions]
            [0] ActualWidth: 0 SharedSizeGroup: A
            [1] ActualWidth: 60 SharedSizeGroup: A
            [2] ActualWidth: 30 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            */
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
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(20, cd.ActualWidth));

            grid.ColumnDefinitions[0].SharedSizeGroup = null;

            grid.Measure(new Size(100, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            PrintColumnDefinitions(grid);
            // ??
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(6 + 2 * 6, cd.ActualWidth));
            /* output after previous line:
            [AddSizer] Column: 0 MinWidth: 6 MinHeight: 6
            [AddSizer] Column: 1 MinWidth: 12 MinHeight: 12
            [AddSizer] Column: 2 MinWidth: 18 MinHeight: 18
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: A
            [1] ActualWidth: 20 SharedSizeGroup: A
            [2] ActualWidth: 20 SharedSizeGroup: A
            [3] ActualWidth: 20 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: 
            [1] ActualWidth: 12 SharedSizeGroup: A
            [2] ActualWidth: 18 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            */

            grid.ColumnDefinitions[1].SharedSizeGroup = null;

            grid.Measure(new Size(100, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            PrintColumnDefinitions(grid);
            // all in group are equal to width (MinWidth) of the sizer in the second column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(6 + 1 * 6, cd.ActualWidth));
            /* output after previous line:
            [AddSizer] Column: 0 MinWidth: 6 MinHeight: 6
            [AddSizer] Column: 1 MinWidth: 12 MinHeight: 12
            [AddSizer] Column: 2 MinWidth: 18 MinHeight: 18
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: A
            [1] ActualWidth: 20 SharedSizeGroup: A
            [2] ActualWidth: 20 SharedSizeGroup: A
            [3] ActualWidth: 20 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: 
            [1] ActualWidth: 12 SharedSizeGroup: 
            [2] ActualWidth: 18 SharedSizeGroup: A
            [3] ActualWidth: 0 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            */
            // NOTE: THIS IS BROKEN IN WPF
            grid.ColumnDefinitions[2].SharedSizeGroup = null;

            // NOTE: THIS IS BROKEN IN WPF
            grid.Measure(new Size(double.PositiveInfinity, 100));
            grid.Arrange(new Rect(new Point(), new Point(100, 100)));
            PrintColumnDefinitions(grid);
            // with no constraint star columns default to the MinWidth of the sizer in the column
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(0, cd.ActualWidth));
            /* output after previous line:
            [AddSizer] Column: 0 MinWidth: 6 MinHeight: 6
            [AddSizer] Column: 1 MinWidth: 12 MinHeight: 12
            [AddSizer] Column: 2 MinWidth: 18 MinHeight: 18
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: A
            [1] ActualWidth: 20 SharedSizeGroup: A
            [2] ActualWidth: 20 SharedSizeGroup: A
            [3] ActualWidth: 20 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            [Grid] ActualWidth: 100 ActualHeight: 100
            [ColumnDefinitions]
            [0] ActualWidth: 20 SharedSizeGroup: 
            [1] ActualWidth: 12 SharedSizeGroup: 
            [2] ActualWidth: 68 SharedSizeGroup: 
            [3] ActualWidth: 0 SharedSizeGroup: A
            [4] ActualWidth: 0 SharedSizeGroup: 
            [RowDefinitions]
            */
        }

        [WpfFact]
        public void ColumnDefinitions_Collection_Is_ReadOnly()
        {
            var grid = CreateGrid(
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)),
                ("A", new GridLength(50)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(50, cd.ActualWidth));

            grid.ColumnDefinitions[0] = new ColumnDefinition { Width = new GridLength(25), SharedSizeGroup = "A" };
            grid.ColumnDefinitions[1] = new ColumnDefinition { Width = new GridLength(75), SharedSizeGroup = "B" };
            grid.ColumnDefinitions[2] = new ColumnDefinition { Width = new GridLength(75), SharedSizeGroup = "B" };
            grid.ColumnDefinitions[3] = new ColumnDefinition { Width = new GridLength(25), SharedSizeGroup = "A" };

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));
        }

        [WpfFact]
        public void ColumnDefinitions_Collection_Reset_SharedSizeGroup()
        {
            var grid = CreateGrid(
                ("A", new GridLength(25)),
                ("B", new GridLength(75)),
                ("B", new GridLength(75)),
                ("A", new GridLength(25)));

            var scope = new Grid();
            scope.Children.Add(grid);

            var root = new Grid();
            root.SetValue(Grid.IsSharedSizeScopeProperty, true);
            root.Children.Add(scope);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "A"), cd => Assert.Equal(25, cd.ActualWidth));
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == "B"), cd => Assert.Equal(75, cd.ActualWidth));

            grid.ColumnDefinitions[0].SharedSizeGroup = null;
            grid.ColumnDefinitions[0].Width = new GridLength(50);
            grid.ColumnDefinitions[1].SharedSizeGroup = null;
            grid.ColumnDefinitions[1].Width = new GridLength(50);
            grid.ColumnDefinitions[2].SharedSizeGroup = null;
            grid.ColumnDefinitions[2].Width = new GridLength(50);
            grid.ColumnDefinitions[3].SharedSizeGroup = null;
            grid.ColumnDefinitions[3].Width = new GridLength(50);

            grid.Measure(new Size(200, 200));
            grid.Arrange(new Rect(new Point(), new Point(200, 200)));
            PrintColumnDefinitions(grid);
            Assert.All(grid.ColumnDefinitions.Where(cd => cd.SharedSizeGroup == null), cd => Assert.Equal(50, cd.ActualWidth));
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
            output.WriteLine($"[AddSizer] Column: {column} MinWidth: {size} MinHeight: {size}");
            return ctrl;
        }
    }
}
