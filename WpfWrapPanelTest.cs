using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Xunit;
using Xunit.Abstractions;

namespace WpfWrapPanelTest
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

    public class WpfWrapPanelTest
    {
        private readonly ITestOutputHelper output;

        public WpfWrapPanelTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [WpfFact]
        public void Lays_Out_Horizontally_On_Separate_Lines()
        {
            var target = new WrapPanel()
            {
                Width = 100,
                Children =
                            {
                                new Border { Height = 50, Width = 100 },
                                new Border { Height = 50, Width = 100 },
                            }
            };

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Size(100, 100), new Size(target.ActualWidth, target.ActualHeight));
            Assert.Equal(new Rect(0, 0, 100, 50), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 50, 100, 50), target.Children[1].BoundsRelativeTo(target));
        }

        [WpfFact]
        public void Lays_Out_Horizontally_On_A_Single_Line()
        {
            var target = new WrapPanel()
            {
                Width = 200,
                Children =
                            {
                                new Border { Height = 50, Width = 100 },
                                new Border { Height = 50, Width = 100 },
                            }
            };

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Size(200, 50), new Size(target.ActualWidth, target.ActualHeight));
            Assert.Equal(new Rect(0, 0, 100, 50), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(100, 0, 100, 50), target.Children[1].BoundsRelativeTo(target));
        }

        [WpfFact]
        public void Lays_Out_Vertically_Children_On_A_Single_Line()
        {
            var target = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Height = 120,
                Children =
                            {
                                new Border { Height = 50, Width = 100 },
                                new Border { Height = 50, Width = 100 },
                            }
            };

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Size(100, 120), new Size(target.ActualWidth, target.ActualHeight));
            Assert.Equal(new Rect(0, 0, 100, 50), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 50, 100, 50), target.Children[1].BoundsRelativeTo(target));
        }

        [WpfFact]
        public void Lays_Out_Vertically_On_Separate_Lines()
        {
            var target = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Height = 60,
                Children =
                            {
                                new Border { Height = 50, Width = 100 },
                                new Border { Height = 50, Width = 100 },
                            }
            };

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Size(200, 60), new Size(target.ActualWidth, target.ActualHeight));
            Assert.Equal(new Rect(0, 0, 100, 50), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(100, 0, 100, 50), target.Children[1].BoundsRelativeTo(target));
        }
    }
}
