using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfUnitTests
{
    public static class Extensions
    {
        public static Rect BoundsRelativeTo(this FrameworkElement element, Visual relativeTo)
        {
            return element.TransformToVisual(relativeTo).TransformBounds(new Rect(element.RenderSize));
        }

        public static Rect BoundsRelativeTo(this UIElement element, Visual relativeTo)
        {
            return BoundsRelativeTo((FrameworkElement)element, relativeTo);
        }

        public static Rect ClippedBoundsRelativeTo(this FrameworkElement element, Visual relativeTo)
        {
            var clip = LayoutInformation.GetLayoutClip(element);
            var rect = new Rect(element.RenderSize);

            if (clip != null)
            {
                rect.Intersect(((RectangleGeometry)clip).Bounds);
            }

            return element.TransformToVisual(relativeTo).TransformBounds(rect);
        }

        public static Rect ClippedBoundsRelativeTo(this UIElement element, Visual relativeTo)
        {
            return ClippedBoundsRelativeTo((FrameworkElement)element, relativeTo);
        }

        public static IEnumerable<UIElement> AsEnumerable(this UIElementCollection children)
        {
            foreach (var child in children)
            {
                yield return child as UIElement;
            }
        }
    }
}
