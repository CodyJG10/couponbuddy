﻿using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Controllers;
using CouponBuddy.Entities;
using CouponBuddy.Navigation;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CouponBuddy.ViewModels.InactiveScreen
{
    public class InactiveScreenViewModel : ViewModel   
    {
        private List<Uri> Images = new List<Uri>();
        private int currentImageIndex = 0;
        private Uri _currentImage;
        public Uri CurrentImage
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
            Images.Shuffle();
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
            if (Images.Count == 0)
            {
                return;
            }
            CurrentImage = Images[currentImageIndex];
            int adDuration = Properties.Settings.Default.INACTIVE_AD_DURATION;
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