using CommonServiceLocator;
using CouponBuddy.Controllers;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Entities;
using CouponBuddy.Navigation;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Timers;
using CouponBuddy.Properties;

namespace CouponBuddy.ViewModels.LoadingScreen
{
    public class LoadingScreenViewModel
    {
        private INavigationService _navigation;

        public LoadingScreenViewModel()
        {
            _navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            LoadContent().ConfigureAwait(true);
        }

        private async Task<Task> LoadContent()
        {
            Console.WriteLine("[Loading] Attempting to load content");

            CheckForInternetConnection();

            System.Threading.Thread.Sleep(1500);

            var db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as DatabaseManager;

            string locationId = Settings.Default.LOCATION_ID;

            var vendors = await db.GetVendors(locationId).ConfigureAwait(true);

            await AdController.Instance.LoadAds();
            await VendorController.Instance.LoadVendors(vendors.ToList()).ConfigureAwait(true);

            Console.WriteLine("[Loading] Loaded all vendors and location ads");

#if (!DEBUG)
            var location = await db.GetLocation(Settings.Default.LOCATION_ID);
            UptimeReport report = new UptimeReport()
            {
                Active = true,
                DeviceId = Settings.Default.DEVICE_ID,
                LocationName = location.Name
            };
            await db.UpdateUptime(report);
#endif
            Application.Current.Dispatcher.Invoke(delegate
            {
                var screen = new Views.InactiveScreen.InactiveScreen();
                _navigation.Navigate(screen);
            });
            return Task.CompletedTask;
        }

        private void CheckForInternetConnection()
        {
            Console.WriteLine("[Internet] attempting to establish network connection");
            if (!IsConnectedToInternet())
            {
                //Not connected to the internet
                bool isConnectedToInternet = false;

                Timer timer = new Timer(2500);

                timer.Elapsed += (sender, e) =>
                {
                    isConnectedToInternet = IsConnectedToInternet();
                    if (isConnectedToInternet)
                    {
                        timer.Stop();
                        Console.WriteLine("[Internet] Detected network connection. Proceeding with loading.");
                        isConnectedToInternet = true;
                        timer.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("[Internet] Waiting for network connection...");
                        isConnectedToInternet = false;
                    }
                };

                timer.Start();

                while (!isConnectedToInternet)
                {
                    continue;
                }
            }
        }

        private bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}