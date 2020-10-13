using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Xunit;

namespace WpfUnitTests
{
    public class EllipseTests
    {
        [WpfFact]
        public void Measure_Does_Not_Set_RenderedGeometry_Rect()
        {
            var target = new Ellipse();

            target.Measure(new Size(100, 100));

            // Uhh, ok that's interesting.
            Assert.IsType<StreamGeometry>(target.RenderedGeometry);
        }

        [WpfFact]
        public void Arrange_Sets_RenderedGeometry_Properties()
        {
            var target = new Ellipse();

            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(0, 0, 100, 100));

            var geometry = Assert.IsType<EllipseGeometry>(target.RenderedGeometry);
            Assert.Equal(new Point(50, 50), geometry.Center);
            Assert.Equal(50, geometry.RadiusX);
            Assert.Equal(50, geometry.RadiusY);
        }

        [WpfFact]
        public void Rearranging_Updates_RenderedGeometry_Rect()
        {
            var target = new Ellipse();

            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(0, 0, 100, 100));

            var geometry = Assert.IsType<EllipseGeometry>(target.RenderedGeometry);
            Assert.Equal(50, geometry.RadiusX);

            target.Measure(new Size(200, 200));
            target.Arrange(new Rect(0, 0, 200, 200));

            geometry = Assert.IsType<EllipseGeometry>(target.RenderedGeometry);
            Assert.Equal(100, geometry.RadiusX);
        }
    }
}
