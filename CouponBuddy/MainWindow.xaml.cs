using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Navigation;
using CouponBuddy.Util;
using CouponBuddy.Views;
using CouponBuddy.Views.ActiveScreen;
using CouponBuddy.Views.InactiveScreen;
using CouponBuddy.Views.LoadingScreen;
using CouponBuddy.Views.VendorScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace CouponBuddy
{
    public partial class MainWindow : Window
    {
        private bool inactiveScreenTimerRunning = false;

        private static MainWindow _instance;
        public static MainWindow Instance { get { return _instance; } }

        public MainWindow()
        {
            _instance = this;
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test");
            }
            (ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService).SetMainFrame(frameMain);
            //WindowState = WindowState.Maximized;
            //WindowStyle = WindowStyle.None;
            Page page = new LoadingScreen();
            frameMain.Navigate(page);
        }

        public void NavigateToPage(Page page)
        {
            frameMain.Navigate(page);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void FrameMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var ta = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.6),
                DecelerationRatio = 0.7,
                To = new Thickness(0, 0, 0, 0)
            };
            if (e.NavigationMode == NavigationMode.New)
            {
                ta.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ta.From = new Thickness(0, 0, 500, 0);
            }
            (e.Content as Page).BeginAnimation(MarginProperty, ta);
        }

        public void InitInactivityDetection()
        {
            if (inactiveScreenTimerRunning) return;
            inactiveScreenTimerRunning = true;
            int timeoutSeconds = int.Parse(Properties.Resources.INACTIVITY_TIMEOUT);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * int.Parse(Properties.Resources.INACTIVE_AD_DURATION);
            timer.Elapsed += (obj, e) =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (frameMain.Content.GetType() != null && frameMain.Content.GetType() == typeof(InactiveScreen)) return;
                        var idleTime = IdleTimeDetector.GetIdleTimeInfo();
                        if (idleTime.IdleTime.TotalSeconds >= timeoutSeconds)
                        {
                            Console.WriteLine("Switching to inactive screen");
                            var screen = new InactiveScreen();
                            NavigateToPage(screen);
                        }

                    });
                }
                catch (Exception) {}
            };
            timer.Start();
        }
    }
}
