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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMTVO.Api;
using TMTVO.Data;
using TMTVO.Data.Modules;

namespace TM_CarStats
{
	/// <summary>
	/// Interaktionslogik für MonitorElement.xaml
	/// </summary>
	public partial class MonitorElement : UserControl
	{
		public MonitorElement()
		{
			this.InitializeComponent();
		}

        public void Update(LiveStandingsItem item, LiveStandingsItem prev)
        {
            Position.Text = item.Position.ToString();
            Name.Text = item.Driver.FullName;
            BestLap.Text = convert(item.FastestLapTime);
            LastLap.Text = convert(item.LastLapTime);

            if (item.Position == 1 || prev == null)
            {
                Gap.Text = "Lap";
                Interval.Text = item.CurrentTrackPct.ToString("0");
            }
            else
            {
                Gap.Text = item.GapLaps == 0 ? convert(item.GapTime) : item.GapLaps.ToString();
                Interval.Text = convert(item.GapTime - prev.GapTime);
            }
        }

        private static string convert(float time)
        {
            if (time <= 0)
                return "No Time";

            int min = (int)(time / 60);
            float sectime = time % 60;
            StringBuilder sb = new StringBuilder();
            if (min > 0)
            {
                sb.Append(min).Append(':');
                sb.Append(sectime.ToString("00.000"));
            }
            else
                sb.Append(sectime.ToString("0.000"));

            return sb.ToString().Replace(',', '.');
        }
	}
}