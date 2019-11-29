using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BrochureBuddy.Util
{
    public static class TextSender
    {
        private static string accountSid = CouponBuddy.Properties.Resources.TEXT_ACCOUNT_SID;
        private static string authToken = CouponBuddy.Properties.Resources.TEXT_ACCOUNT_TOKEN;
        private static string from = CouponBuddy.Properties.Resources.TEXT_ACCOUNT_FROM;

        private static bool isInitialized = false;

        public static void SendText(VendorCoupon coupon, string number)
        {
            if (!isInitialized)
            {
                TwilioClient.Init(accountSid, authToken);
                isInitialized = true;
            }

            StringBuilder bodyStringBuilder = new StringBuilder();
            bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosk");
            bodyStringBuilder.AppendLine("Here is your coupon:");
            bodyStringBuilder.AppendLine(coupon.Title);
            bodyStringBuilder.AppendLine(coupon.Description);
            bodyStringBuilder.AppendLine(coupon.Instructions);
            string body = bodyStringBuilder.ToString();

            var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(from),
                to: new Twilio.Types.PhoneNumber("+1" + number)
            );

            Console.WriteLine(message.Sid);
        }
    }
}