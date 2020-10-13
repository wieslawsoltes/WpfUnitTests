using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Xunit;

namespace WpfUnitTests
{
    public class RectangleTests
    {
        [WpfFact]
        public void Measure_Does_Not_Set_RenderedGeometry_Rect()
        {
            var target = new Rectangle();

            target.Measure(new Size(100, 100));

            var geometry = Assert.IsType<RectangleGeometry>(target.RenderedGeometry);
            Assert.Equal(Rect.Empty, geometry.Rect);
        }

        [WpfFact]
        public void Arrange_Sets_RenderedGeometry_Rect()
        {
            var target = new Rectangle();

            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(0, 0, 100, 100));

            var geometry = Assert.IsType<RectangleGeometry>(target.RenderedGeometry);
            Assert.Equal(new Rect(0, 0, 100, 100), geometry.Rect);
        }

        [WpfFact]
        public void Rearranging_Updates_RenderedGeometry_Rect()
        {
            var target = new Rectangle();

            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(0, 0, 100, 100));

            var geometry = Assert.IsType<RectangleGeometry>(target.RenderedGeometry);
            Assert.Equal(new Rect(0, 0, 100, 100), geometry.Rect);

            target.Measure(new Size(200, 200));
            target.Arrange(new Rect(0, 0, 200, 200));

            geometry = Assert.IsType<RectangleGeometry>(target.RenderedGeometry);
            Assert.Equal(new Rect(0, 0, 200, 200), geometry.Rect);
        }
    }
}
