using System.Windows;
using System.Windows.Controls;
using Xunit;
using Xunit.Abstractions;

namespace WpfUnitTests
{
    public class DockPanelTests
    {
        private readonly ITestOutputHelper output;

        public DockPanelTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [WpfFact]
        public void Should_Dock_Controls_Horizontal_First()
        {
            var target = new DockPanel
            {
                Children =
                {
                    new Border { Width = 500, Height = 50},
                    new Border { Width = 500, Height = 50 },
                    new Border { Width = 50, Height = 400 },
                    new Border { Width = 50, Height = 400 },
                    new Border { },
                }
            };

            target.Children[0].SetValue(DockPanel.DockProperty, Dock.Top);
            target.Children[1].SetValue(DockPanel.DockProperty, Dock.Bottom);
            target.Children[2].SetValue(DockPanel.DockProperty, Dock.Left);
            target.Children[3].SetValue(DockPanel.DockProperty, Dock.Right);

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Rect(0, 0, 500, 500), target.BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 0, 500, 50), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 450, 500, 50), target.Children[1].BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 50, 50, 400), target.Children[2].BoundsRelativeTo(target));
            Assert.Equal(new Rect(450, 50, 50, 400), target.Children[3].BoundsRelativeTo(target));
            Assert.Equal(new Rect(50, 50, 400, 400), target.Children[4].BoundsRelativeTo(target));
        }

        [WpfFact]
        public void Should_Dock_Controls_Vertical_First()
        {
            var target = new DockPanel
            {
                Children =
                {
                    new Border { Width = 50, Height = 400 },
                    new Border { Width = 50, Height = 400 },
                    new Border { Width = 500, Height = 50},
                    new Border { Width = 500, Height = 50 },
                    new Border { },
                }
            };

            target.Children[0].SetValue(DockPanel.DockProperty, Dock.Left);
            target.Children[1].SetValue(DockPanel.DockProperty, Dock.Right);
            target.Children[2].SetValue(DockPanel.DockProperty, Dock.Top);
            target.Children[3].SetValue(DockPanel.DockProperty, Dock.Bottom);

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));

            Assert.Equal(new Rect(0, 0, 600, 400), target.BoundsRelativeTo(target));
            Assert.Equal(new Rect(0, 0, 50, 400), target.Children[0].BoundsRelativeTo(target));
            Assert.Equal(new Rect(550, 0, 50, 400), target.Children[1].BoundsRelativeTo(target));
            Assert.Equal(new Rect(50, 0, 500, 50), target.Children[2].BoundsRelativeTo(target));
            Assert.Equal(new Rect(50, 350, 500, 50), target.Children[3].BoundsRelativeTo(target));
            Assert.Equal(new Rect(50, 50, 500, 300), target.Children[4].BoundsRelativeTo(target));
        }

        [WpfFact]
        public void Changing_Child_Dock_Invalidates_Measure()
        {
            Border child;
            var target = new DockPanel
            {
                Children =
                {
                    (child = new Border
                    {
                    }),
                }
            };

            child.SetValue(DockPanel.DockProperty, Dock.Left);

            target.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            target.Arrange(new Rect(target.DesiredSize));
            Assert.True(target.IsMeasureValid);

            DockPanel.SetDock(child, Dock.Right);

            Assert.False(target.IsMeasureValid);
        }
    }
}
