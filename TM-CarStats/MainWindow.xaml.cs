using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TMTVO.Api;
using TMTVO.Data.Modules;
using TMTVO_Modules.Data.Modules;
using MahApps.Metro.Controls;

namespace TM_CarStats
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static readonly int TICKS_PER_SECOND = 20;

        public API Api { get; private set; }

        private DispatcherTimer updateTimer;
        private LiveStandingsModule module;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Api = new API(TICKS_PER_SECOND);
            Api.Run = true;

            Api.AddModule(new SessionsModule());
            Api.AddModule(new SessionTimerModule());
            Api.AddModule(new TeamRadioModule());
            Api.AddModule(new DriverModule());
            Api.AddModule(module = new LiveStandingsModule());
            Api.AddModule(new CameraModule());
            Api.AddModule(new TimeDeltaModule());
            Api.AddModule(new GridModule());
            Api.AddModule(new IRatingModule());

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(50);
            updateTimer.Tick += Api.Connect;
            updateTimer.Tick += updateStatusBar;
            updateTimer.Tick += updateRacingMonitor;
            updateTimer.Tick += updateCarStats;

            updateTimer.Start();
        }

        private void updateStatusBar(object sender, EventArgs e)
        {
            if (Api.IsConnected)
                Item1.Content = "iRacing simulatior connected.";
            else
                Item1.Content = "iRacing simulatior not connected.";
        }

        private void updateRacingMonitor(object sender, EventArgs e)
        {
            RacingMonitor.Update(module);
        }

        private void updateCarStats(object sender, EventArgs e)
        {
            MyCarStats.Update(Api);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Api.Run = false;
        }
    }
}
