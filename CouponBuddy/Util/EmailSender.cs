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
            string host = CouponBuddy.Properties.Resources.EMAIL_HOST;
            string password = CouponBuddy.Properties.Resources.EMAIL_PASSWORD;
            string from = CouponBuddy.Properties.Resources.EMAIL_FROM;
            string user = CouponBuddy.Properties.Resources.EMAIL_USER;
            using (SmtpClient smtpClient = new SmtpClient(host, 465))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(user, password);
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                string subject = "E-Coupon Delivered By PG Technologies";

                StringBuilder bodyStringBuilder = new StringBuilder();
                bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosks");
                bodyStringBuilder.AppendLine("Here is your coupon:");
                bodyStringBuilder.AppendLine(coupon.Title);
                bodyStringBuilder.AppendLine(coupon.Description);
                bodyStringBuilder.AppendLine(coupon.Instructions);
                string body = bodyStringBuilder.ToString();

                using (MailMessage mail = new MailMessage(from, destination, subject, body))
                {
                    //smtpClient.SendAsync(mail, null);
                    try
                    {
                        smtpClient.Send(mail);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}