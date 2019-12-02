using BrochureBuddy.Util;
using CouponBuddy;
using CouponBuddy.Entities;
using CouponBuddy.Views.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrochureBuddy.ViewModels.Coupon
{
    public class CouponScreenViewModel
    {
        public VendorCoupon Coupon { get; set; }

        public CouponScreenViewModel(VendorCoupon coupon)
        {
            Coupon = coupon;
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
        }
    }
}