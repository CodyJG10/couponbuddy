using CommonServiceLocator;
using CouponBuddy.Api;
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
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using QRCoder;
using System.Drawing;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using BrochureBuddy.Views.Coupon;

namespace CouponBuddy.ViewModels.VendorScreen
{
    public class VendorScreenViewModel : ViewModel
    {
        public Vendor Vendor { get; set; }
        private ImageSource _image; 
        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private ImageSource _qrCodeImage;
        public ImageSource QrCodeImage
        {
            get
            {
                return _qrCodeImage;
            }
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged("QrCodeImage");
            }
        }

        public ObservableCollection<VendorCoupon> Coupons { get; } = new ObservableCollection<VendorCoupon>();

        public VendorScreenViewModel(Vendor vendor)
        {
            Vendor = vendor;
            LoadMedia();
            LoadQrCode();
            LoadCoupons();
        }

        private void LoadCoupons()
        {
            var coupons = VendorController.Instance.GetVendorCoupons(Vendor.Id);
            coupons.ForEach(x => Coupons.Add(x));
        }

        private void LoadMedia()
        {
            BitmapImageLoader imgLoader = new BitmapImageLoader();
            var vendorMedia = VendorController.Instance.GetVendorMedia(Vendor.Id);
            Image = vendorMedia.logoImage;
        }

        private void LoadQrCode()
        {
            if (String.IsNullOrEmpty(Vendor.WebsiteUrl)) return;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(Vendor.WebsiteUrl, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    QrCodeImage = BitmapImageLoader.ToBitmapImage(qrCodeImage) as ImageSource;
                }
            }
        }

        public void CouponButtonClicked(int couponId)
        {
            var coupon = Coupons.ToList().Single(x => x.Id == couponId);
            CouponView view = new CouponView(coupon);
            MainWindow.Instance.NavigateToPage(view);
        }
    }
}