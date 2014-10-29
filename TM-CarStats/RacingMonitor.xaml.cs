using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMTVO.Data.Modules;

namespace TM_CarStats
{
	public partial class RacingMonitor : UserControl
	{
        private static readonly SolidColorBrush titleBrush = new SolidColorBrush(Colors.Red);

        private List<MonitorElement> elements;

		public RacingMonitor()
		{
			this.InitializeComponent();
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            elements = new List<MonitorElement>();

            TitleItem.Position.Text = "P";
            TitleItem.Name.Text = "Drivername";
            TitleItem.BestLap.Text = "Best Lap";
            TitleItem.LastLap.Text = "Last Lap";
            TitleItem.Gap.Text = "Gap";
            TitleItem.Interval.Text = "Int";

            TitleItem.Position.Foreground = titleBrush;
            TitleItem.Name.Foreground = titleBrush;
            TitleItem.BestLap.Foreground = titleBrush;
            TitleItem.LastLap.Foreground = titleBrush;
            TitleItem.Gap.Foreground = titleBrush;
            TitleItem.Interval.Foreground = titleBrush;
        }

        public void Update(LiveStandingsModule module)
        {
            LiveStandingsItem current = null;
            LiveStandingsItem prev = null;
            MonitorElement elem = null;

            int i = 1;
            while (i <= module.Items.Count)
            {
                current = module.FindDriverByPosNL(i);

                try
                {
                    elem = elements[i - 1];
                }
                catch
                {
                    elem = null;
                }

                if (current == null)
                {
                    try
                    {
                        LayoutRoot.Children.RemoveAt(i);
                    }
                    catch { }

                    break;
                }
                if (elem == null && current != null)
                {
                    elem = new MonitorElement();
                    elements.Add(elem);
                    elem.VerticalAlignment = VerticalAlignment.Top;
                    elem.Margin = new Thickness(0, (i - 1) * 30, 0, 0);
                    Grid.SetRow(elem, 1);
                    LayoutRoot.Children.Add(elem);
                }

                elem.Update(current, prev);
                prev = current;
                i++;
            }

            while (i++ <= elements.Count)
                elements.RemoveAt(i - 2);
        }
	}
}