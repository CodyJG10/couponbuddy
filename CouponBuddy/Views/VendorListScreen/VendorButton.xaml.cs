using CommonServiceLocator;
using CouponBuddy.Entities;
using CouponBuddy.Navigation;
using CouponBuddy.ViewModels;
using CouponBuddy.ViewModels.VendorListScreen;
using System;
using System.Collections.Generic;
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
using CouponBuddy.Api.Interfaces;

namespace CouponBuddy.Views.VendorListScreen
{
    public partial class VendorButton : UserControl
    {
        public VendorButton()
        {
            InitializeComponent();
            DataContext = new VendorButtonViewModel();
        }

        public static readonly DependencyProperty SetVendorProperty =
            DependencyProperty.Register("Vendor", typeof(Vendor), typeof(VendorButton),
                new PropertyMetadata(null, new PropertyChangedCallback(OnVendorChanged)));

        public Vendor Vendor
        {
            get { return (Vendor)GetValue(SetVendorProperty); }
            set { SetValue(SetVendorProperty, value); }
        }

        private static void OnVendorChanged(DependencyObject d,
        DependencyPropertyChangedEventArgs e)
        {
            VendorButton control = d as VendorButton;
            control.OnVendorChanged(e);
        }

        private void OnVendorChanged(DependencyPropertyChangedEventArgs e)
        {
            (DataContext as VendorButtonViewModel).Vendor = Vendor;
            lblCategoryName.Content = Vendor.Name;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var navigation = ServiceLocator.Current.GetService(typeof(INavigationService)) as INavigationService;
            var database = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;
            var locationId = CouponBuddy.Properties.Resources.LOCATION_ID;
            database.AddClick(Vendor, locationId);
            Page page = new VendorScreen.VendorScreen(Vendor);
            navigation.Navigate(page);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            imgLogo.ImageSource = new BitmapImage(new Uri("/Assets/404.png", UriKind.Relative));
        }
    }
}