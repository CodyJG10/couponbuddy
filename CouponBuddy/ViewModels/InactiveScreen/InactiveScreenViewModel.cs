using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Controllers;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CouponBuddy.ViewModels.InactiveScreen
{
    public class InactiveScreenViewModel : ViewModel
    {
        private List<ImageSource> Images = new List<ImageSource>();
        private int currentImageIndex = 0;
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
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public InactiveScreenViewModel()
        {
            LoadAds();
            RunAds();
        }

        private void LoadAds()
        {
            var vendors = VendorController.Instance.GetAllVendors();
            foreach (var vendor in vendors)
            {
                var source = VendorController.Instance.GetVendorMedia(vendor.Id).inactiveImage;
                if (source == null) continue;
                Images.Add(source);
            }
        }

        private void RunAds()
        {
            CurrentImage = Images[currentImageIndex];
            int adDuration = int.Parse(Properties.Resources.INACTIVE_AD_DURATION);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = adDuration * 1000;
            timer.Elapsed += (obj, e) =>
            {
                currentImageIndex++;
                if (currentImageIndex >= Images.Count)
                    currentImageIndex = 0;
                CurrentImage = Images[currentImageIndex];
            };
            timer.Start();
            MainWindow.Instance.InitInactivityDetection();
        }
    }
}