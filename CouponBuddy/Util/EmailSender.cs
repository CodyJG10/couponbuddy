using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BrochureBuddy.Util
{
    public static class EmailSender
    {
        public static void SendEmail(VendorCoupon coupon, string destination)
        {
            using (SmtpClient smtpClient = new SmtpClient("mail.pg-technologies.com", 465))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("coupons@pg-technologies.com", "Airplane10");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                //var from  = new MailAddress("coupons@pg-technologies.com", "PG Technologies Coupon");

                var to = new MailAddressCollection();
                to.Add(new MailAddress(destination));

                string subject = "E-Coupon Delivered By PG Technologies";

                StringBuilder bodyStringBuilder = new StringBuilder();
                bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosks");
                bodyStringBuilder.AppendLine("Here is your coupon:");
                bodyStringBuilder.AppendLine(coupon.Title);
                bodyStringBuilder.AppendLine(coupon.Description);
                bodyStringBuilder.AppendLine(coupon.Instructions);
                string body = bodyStringBuilder.ToString();

                using (MailMessage mail = new MailMessage("coupons@pg-technologies.com", destination, subject, body))
                {
                    //smtpClient.SendAsync(mail, null);
                    smtpClient.Send(mail);
                }
            }
        }
    }
}