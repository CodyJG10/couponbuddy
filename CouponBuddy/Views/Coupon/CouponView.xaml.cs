using CouponBuddy.ViewModels.Coupon;
using CouponBuddy.Entities;
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

namespace CouponBuddy.Views.Coupon
{
    public partial class CouponView : Page
    {
        private CouponScreenViewModel ViewModel
        {
            get
            {
                return DataContext as CouponScreenViewModel;
            }
        }

        public CouponView(VendorCoupon coupon)
        {
            InitializeComponent();
            DataContext = new CouponScreenViewModel(coupon);
        }

        private void BtnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            ViewModel.SendEmail(email);           
        }

        private void BtnSendText_Click(object sender, RoutedEventArgs e)
        {
            string number = txtPhoneNumber.Text;
            ViewModel.SendText(number);
        }
    }
}