using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Util
{
    //public static class EmailSender
    //{
    //    public static void SendEmail(VendorCoupon coupon, string destination)
    //    {
    //        using (SmtpClient smtpClient = new SmtpClient("mail.pg-technologies.com", 465))
    //        {
    //            smtpClient.Credentials = new System.Net.NetworkCredential("coupons@pg-technologies.com", "Airplane10");
    //            smtpClient.UseDefaultCredentials = true;
    //            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    //            smtpClient.EnableSsl = true;

    //            //var from  = new MailAddress("coupons@pg-technologies.com", "PG Technologies Coupon");

    //            var to = new MailAddressCollection();
    //            to.Add(new MailAddress(destination));

    //            string subject = "E-Coupon Delivered By PG Technologies";

    //            StringBuilder bodyStringBuilder = new StringBuilder();
    //            bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosks");
    //            bodyStringBuilder.AppendLine("Here is your coupon:");
    //            bodyStringBuilder.AppendLine(coupon.Title);
    //            bodyStringBuilder.AppendLine(coupon.Description);
    //            bodyStringBuilder.AppendLine(coupon.Instructions);
    //            string body = bodyStringBuilder.ToString();

    //            using (MailMessage mail = new MailMessage("coupons@pg-technologies.com", destination, subject, body))
    //            {
    //                //smtpClient.SendAsync(mail, null);
    //                smtpClient.Send(mail);
    //            }
    //        }
    //    }
    //}

    public static class EmailSender
    {
        #region Configuration
        private static readonly string host = CouponBuddy.Properties.Resources.EMAIL_HOST;
        private static readonly string password = CouponBuddy.Properties.Resources.EMAIL_PASSWORD;
        private static readonly string from = CouponBuddy.Properties.Resources.EMAIL_FROM;
        private static readonly string user = CouponBuddy.Properties.Resources.EMAIL_USER;
        #endregion

        public static async void SendEmail(VendorCoupon coupon, string destination)
        {
            var _db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;
            var vendor = await _db.GetVendor(coupon.VendorId).ConfigureAwait(true);
            string vendorName = vendor.Name;

            string subject = "E-Coupon Delivered By PG Technologies";
            StringBuilder bodyStringBuilder = new StringBuilder();
            bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosks");
            bodyStringBuilder.AppendLine("Here is your coupon:");
            bodyStringBuilder.AppendLine("Coupon offered by " + vendorName);
            bodyStringBuilder.AppendLine(coupon.Title);
            bodyStringBuilder.AppendLine(coupon.Description);
            bodyStringBuilder.AppendLine(coupon.Instructions);
            string body = bodyStringBuilder.ToString();

            EmailManager mailMan = new EmailManager(host);

            EmailSendConfigure myConfig = new EmailSendConfigure();
            myConfig.ClientCredentialUserName = user;
            myConfig.ClientCredentialPassword = password;
            myConfig.To = destination;
            myConfig.From = from;
            myConfig.FromDisplayName = "Coupon Network";
            myConfig.Priority = MailPriority.Normal;
            myConfig.Subject = subject;

            EmailContent myContent = new EmailContent();
            myContent.Content = body;

            mailMan.SendMail(myConfig, myContent);
        }

        private class EmailManager
        {
            private string m_HostName;

            public EmailManager(string hostName)
            {
                m_HostName = hostName;
            }

            public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
            {
                MailMessage msg = ConstructEmailMessage(emailConfig, content);
                Send(msg, emailConfig);
            }

            private MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
            {
                MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add(emailConfig.To);

                msg.From = new MailAddress(emailConfig.From,
                                           emailConfig.FromDisplayName,
                                           System.Text.Encoding.UTF8);
                msg.IsBodyHtml = content.IsHtml;
                msg.Body = content.Content;
                msg.Priority = emailConfig.Priority;
                msg.Subject = emailConfig.Subject;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;

                if (content.AttachFileName != null)
                {
                    Attachment data = new Attachment(content.AttachFileName,
                                                     MediaTypeNames.Application.Zip);
                    msg.Attachments.Add(data);
                }

                return msg;
            }

            private void Send(MailMessage message, EmailSendConfigure emailConfig)
            {
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(
                                      emailConfig.ClientCredentialUserName,
                                      emailConfig.ClientCredentialPassword);
                client.Host = m_HostName;
                client.Port = 25;
                client.EnableSsl = true;

                client.SendAsync(emailConfig.From, emailConfig.To, emailConfig.Subject, message.Body, null);

                message.Dispose();
            }

        }

        private class EmailSendConfigure
        {
            public string To { get; set; }
            public string From { get; set; }
            public string FromDisplayName { get; set; }
            public string Subject { get; set; }
            public MailPriority Priority { get; set; }
            public string ClientCredentialUserName { get; set; }
            public string ClientCredentialPassword { get; set; }
            public EmailSendConfigure()
            {
            }
        }

        private class EmailContent
        {
            public bool IsHtml { get; set; }
            public string Content { get; set; }
            public string AttachFileName { get; set; }
        }
    }
}