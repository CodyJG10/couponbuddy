using CouponBuddy.Util;
using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CouponBuddy.Views.Coupon;
using QRCoder;
using System.Windows.Media;

namespace CouponBuddy.ViewModels.Coupon
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
                Console.WriteLine("[Coupon] error emailing coupon: " + e.Message);
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
                string baseUrl = Properties.Resources.BASE_URL;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(baseUrl + "/coupons?id=" + Coupon.Id, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    var qrCodeImage = qrCode.GetGraphic(20);
                    QrCodeImage = BitmapImageLoader.FromBitmap(qrCodeImage);
                }
            }
        }
    }
}