using BrochureBuddy.Util;
using CouponBuddy.Entities;
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
            EmailSender.SendEmail(Coupon, email);
            Console.WriteLine("Sent email to: " + email);
        }

        public void SendText(string number)
        {
            TextSender.SendText(Coupon, number);
        }
    }
}