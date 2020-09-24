using CouponBuddy.Util;
using CouponBuddy.ViewModels;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CouponBuddy.Views.AdvertiseWithUs
{
    /// <summary>
    /// Interaction logic for AdvertiseWithUs.xaml
    /// </summary>
    public partial class AdvertiseWithUs : Page
    {
        public AdvertiseWithUs()
        {
            InitializeComponent();
            DataContext = new AdvertiseWithusViewModel();
        }
    }

    public class AdvertiseWithusViewModel : ViewModel
    {
        private ImageSource _qrCodeImageSource;
        public ImageSource QrCodeImageSource
        {
            get
            {
                return _qrCodeImageSource;
            }
            set
            {
                _qrCodeImageSource = value;
                OnPropertyChanged(nameof(QrCodeImageSource));
            }
        }

        public AdvertiseWithusViewModel()
        {
            LoadQrCode();
        }

        private void LoadQrCode()
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://www.pg-technologies.com", QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    QrCodeImageSource = BitmapImageLoader.FromBitmap(qrCodeImage) as ImageSource;
                }
            }
        }
    }
}