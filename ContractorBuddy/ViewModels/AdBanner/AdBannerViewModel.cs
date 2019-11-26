using CommonServiceLocator;
using BrochureBuddy.Ads;
using BrochureBuddy.Api;
using BrochureBuddy.Api.Interfaces;
using BrochureBuddy.Api.Managers;
using BrochureBuddy.Entities;
using BrochureBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BrochureBuddy.ViewModels.AdBanner
{
    public class AdBannerViewModel : ViewModel
    {
        private ImageSource _currentImage;
        public ImageSource CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                _currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }

        public AdBannerViewModel()
        {
            Init();
        }

        private async void Init()
        {
            Timer timer = new Timer();
            timer.Interval = 1000 * int.Parse(Properties.Resources.INACTIVE_AD_DURATION);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadNextAd();
        }

        private async void LoadNextAd()
        {
            Console.WriteLine("[AD] Loading next ad");
            var ad = AdManager.Instance.GetNextAd();
            CurrentImage = ad;
        }
    }
}