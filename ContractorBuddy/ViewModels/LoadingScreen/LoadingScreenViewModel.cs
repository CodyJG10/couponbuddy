using CommonServiceLocator;
using BrochureBuddy.Ads;
using BrochureBuddy.Api.Interfaces;
using BrochureBuddy.Api.Managers;
using BrochureBuddy.Controllers;
using BrochureBuddy.Entities;
using BrochureBuddy.Navigation;
using BrochureBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Timers;

namespace BrochureBuddy.ViewModels.LoadingScreen
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
            CheckForInternetConnection();

            BitmapImageLoader imgLoader = new BitmapImageLoader();
            var db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as DatabaseManager;
            
            string locationId = Properties.Resources.LOCATION_ID;

            var vendors = await db.GetVendors(locationId).ConfigureAwait(true);

            await AdManager.Instance.LoadAds().ConfigureAwait(true);
            await VendorController.Instance.LoadVendors(vendors.ToList()).ConfigureAwait(true);

            Console.WriteLine("Loaded all vendors and location ads");

            MainWindow.Instance.Activate();
            MainWindow.Instance.Focus();

            Application.Current.Dispatcher.Invoke((Action)delegate {
                var screen = new Views.InactiveScreen.InactiveScreen(); 
                _navigation.Navigate(screen);
            });
            return Task.CompletedTask;
        }

        private void CheckForInternetConnection()
        {
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
                        Console.WriteLine("Detected network connection. Proceeding with loading.");
                        isConnectedToInternet = true;
                        timer.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("Waiting for network connection...");
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