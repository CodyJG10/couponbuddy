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

namespace CouponBuddy.ViewModels.VendorScreen
{
    public class VendorScreenViewModel : ViewModel
    {
        private int currentIndex = 1;

        public Vendor Vendor { get; set; }
        private List<ImageSource> VendorMedia { get; set; } = new List<ImageSource>();
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

        public VendorScreenViewModel(Vendor vendor)
        {
            Vendor = vendor;
            LoadMedia();
            LoadQrCode();
        }

        private void LoadMedia()
        {
            BitmapImageLoader imgLoader = new BitmapImageLoader();
            var vendorMedia = VendorController.Instance.GetVendorMedia(Vendor.Id).VendorMediaObjects;
            vendorMedia.ForEach(x => VendorMedia.Add(x));
            if (VendorMedia[0] != null) Image = VendorMedia[0];
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

        public void NextImage()
        {
            currentIndex++;
            if (currentIndex >= VendorMedia.Count) currentIndex = 0;
            Image = VendorMedia[currentIndex];
        }

        public void PreviousImage()
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = VendorMedia.Count - 1;
            Image = VendorMedia[currentIndex];
        }
    }
}