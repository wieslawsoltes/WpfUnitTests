using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Xunit;

namespace WpfUnitTests
{
    public class PathTests
    {
        [WpfTheory]
        [InlineData(Stretch.None, 100, 200)]
        [InlineData(Stretch.Fill, 500, 500)]
        [InlineData(Stretch.Uniform, 250, 500)]
        [InlineData(Stretch.UniformToFill, 500, 500)]
        public void Calculates_Correct_DesiredSize_For_Finite_Bounds(Stretch stretch, double expectedWidth, double expectedHeight)
        {
            var target = new Path()
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 200) },
                Stretch = stretch,
            };

            target.Measure(new Size(500, 500));

            Assert.Equal(new Size(expectedWidth, expectedHeight), target.DesiredSize);
        }

        [WpfTheory]
        [InlineData(Stretch.None)]
        [InlineData(Stretch.Fill)]
        [InlineData(Stretch.Uniform)]
        [InlineData(Stretch.UniformToFill)]
        public void Calculates_Correct_DesiredSize_For_Infinite_Bounds(Stretch stretch)
        {
            var target = new Path()
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 200) },
                Stretch = stretch,
            };

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            Assert.Equal(new Size(100, 200), target.DesiredSize);
        }

        [WpfFact]
        public void Measure_Does_Not_Update_RenderedGeometry_Transform()
        {
            var target = new Path
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 200) },
                Stretch = Stretch.Fill,
            };

            target.Measure(new Size(500, 500));

            Assert.Equal(Matrix.Identity, target.RenderedGeometry.Transform.Value);
        }

        [WpfTheory]
        [InlineData(Stretch.None, 1, 1)]
        [InlineData(Stretch.Fill, 5, 2.5)]
        [InlineData(Stretch.Uniform, 2.5, 2.5)]
        [InlineData(Stretch.UniformToFill, 5, 5)]
        public void Arrange_Updates_RenderedGeometry_Transform(Stretch stretch, double expectedScaleX, double expectedScaleY)
        {
            var target = new Path
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 200) },
                Stretch = stretch,
            };

            target.Measure(new Size(500, 500));
            target.Arrange(new Rect(0, 0, 500, 500));

            Assert.Equal(CreateScale(expectedScaleX, expectedScaleY), target.RenderedGeometry.Transform.Value);
        }

        [WpfFact]
        public void Arrange_Reserves_All_Of_Arrange_Rect()
        {
            RectangleGeometry geometry;
            var target = new Path
            {
                Data = geometry = new RectangleGeometry { Rect = new Rect(0, 0, 100, 200) },
                Stretch = Stretch.Uniform,
            };

            target.Measure(new Size(400, 400));
            target.Arrange(new Rect(0, 0, 400, 400));

            Assert.Equal(new Rect(0, 0, 100, 200), geometry.Rect);
            Assert.Equal(CreateScale(2, 2), target.RenderedGeometry.Transform.Value);

            var slot = LayoutInformation.GetLayoutSlot(target);
            Assert.Equal(new Rect(0, 0, 400, 400), slot);
        }

        [WpfFact]
        public void Measure_Without_Arrange_Does_Not_Clear_RenderedGeometry_Transform()
        {
            var target = new Path
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 100) },
                Stretch = Stretch.Fill,
            };

            target.Measure(new Size(200, 200));
            target.Arrange(new Rect(0, 0, 200, 200));

            Assert.Equal(CreateScale(2, 2), target.RenderedGeometry.Transform.Value);

            target.Measure(new Size(300, 300));

            Assert.Equal(CreateScale(2, 2), target.RenderedGeometry.Transform.Value);
        }

        [WpfFact]
        public void Arrange_Without_Measure_Updates_RenderedGeometry_Transform()
        {
            var target = new Path
            {
                Data = new RectangleGeometry { Rect = new Rect(0, 0, 100, 100) },
                Stretch = Stretch.Fill,
            };

            target.Measure(new Size(200, 200));
            target.Arrange(new Rect(0, 0, 200, 200));

            Assert.Equal(CreateScale(2, 2), target.RenderedGeometry.Transform.Value);

            target.Arrange(new Rect(0, 0, 300, 300));
            Assert.Equal(CreateScale(3, 3), target.RenderedGeometry.Transform.Value);
        }

        private static Matrix CreateScale(double x, double y)
        {
            var m = Matrix.Identity;
            m.Scale(x, y);
            return m;
        }
    }
}
