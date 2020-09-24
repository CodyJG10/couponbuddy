using CommonServiceLocator;
using CouponBuddy.Ads;
using CouponBuddy.Api;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CouponBuddy.ViewModels.AdBanner
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
            timer.Interval = 1000 * Properties.Settings.Default.INACTIVE_AD_DURATION;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadNextAd();
        }

        private void LoadNextAd()
        {
            Console.WriteLine("[Ads] Loading next ad");
            var ad = AdManager.Instance.GetNextAd();
            CurrentImage = ad;
        }
    }
}