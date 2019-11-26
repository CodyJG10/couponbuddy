using CouponBuddy.Entities;
using CouponBuddy.ViewModels.VendorScreen;
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

namespace CouponBuddy.Views.VendorScreen
{
    public partial class VendorScreen : Page
    {
        public VendorScreen(Vendor vendor)
        {
            InitializeComponent();
            DataContext = new VendorScreenViewModel(vendor);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VendorScreenViewModel).NextImage();
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VendorScreenViewModel).PreviousImage();
        }
    }
}