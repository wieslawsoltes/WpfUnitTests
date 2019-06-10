using System.Windows;
using System.Windows.Media;

namespace WpfUnitTests
{
    public static class FrameworkElementExtensions
    {
        public static Rect BoundsRelativeTo(this FrameworkElement element, Visual relativeTo)
        {
            return element.TransformToVisual(relativeTo).TransformBounds(new Rect(element.RenderSize));
        }

        public static Rect BoundsRelativeTo(this UIElement element, Visual relativeTo)
        {
            return BoundsRelativeTo((FrameworkElement)element, relativeTo);
        }
    }
}
