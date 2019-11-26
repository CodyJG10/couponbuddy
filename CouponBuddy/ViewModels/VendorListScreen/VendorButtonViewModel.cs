using CommonServiceLocator;
using CouponBuddy.Api;
using CouponBuddy.Controllers;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CouponBuddy.ViewModels.VendorListScreen
{
    public class VendorButtonViewModel : ViewModel
    {
        private Vendor _vendor;
        public Vendor Vendor
        {
            get
            {
                return _vendor;
            }
            set
            {
                _vendor = value;
                LoadImage();
            }
        }

        private ImageSource _source;
        public ImageSource LogoSource
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged("LogoSource");
            }
        }

        private void LoadImage()
        {
            ImageSource img;
            if (VendorController.Instance.GetVendorMedia(Vendor.Id).logoImage != null)
            {
                img = VendorController.Instance.GetVendorMedia(Vendor.Id).logoImage;
            }
            else
            {
                img = new BitmapImage(new Uri("/Assets/404.png", UriKind.Relative));
            }

            LogoSource = img;
        }
    }
}