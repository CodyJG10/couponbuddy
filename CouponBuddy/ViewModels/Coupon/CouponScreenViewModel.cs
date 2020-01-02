using BrochureBuddy.Util;
using CouponBuddy;
using CouponBuddy.Entities;
using CouponBuddy.Views.Coupon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CouponBuddy.Util;
using CouponBuddy.ViewModels;
using QRCoder;

namespace BrochureBuddy.ViewModels.Coupon
{
    public class CouponScreenViewModel : ViewModel
    {
        public VendorCoupon Coupon { get; set; }

        private ImageSource _qrCodeImage;

        public ImageSource QrCodeImage
        {
            get { return _qrCodeImage; }
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged(nameof(_qrCodeImage));
            }
        }

        public CouponScreenViewModel(VendorCoupon coupon)
        {
            Coupon = coupon;
            LoadQrImage();
        }

        public void SendEmail(string email)
        {
            try
            {
                EmailSender.SendEmail(Coupon, email);
                MainWindow.Instance.NavigateToPage(new CouponSentScreen());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendText(string number)
        {
            TextSender.SendText(Coupon, number);
            MainWindow.Instance.NavigateToPage(new CouponSentScreen());
        }

        private void LoadQrImage()
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                string baseUrl = CouponBuddy.Properties.Resources.BASE_URL;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(baseUrl + "/coupons?id=" + Coupon.Id, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    QrCodeImage = BitmapImageLoader.ToBitmapImage(qrCodeImage) as ImageSource;
                }
            }
        }
    }

}