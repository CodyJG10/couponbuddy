using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BrochureBuddy.Util
{
    public static class EmailSender
    {
        public static async void SendEmail(VendorCoupon coupon, string destination)
        {
            var apiKey = CouponBuddy.Properties.Resources.SENDGRID_EMAIL_APIKEY;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("coupons@pg-technologies.com", "Coupon Messenger");
            var to = new EmailAddress(destination);
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
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, null);
            client.SendEmailAsync(msg);
        }

        //#region Configuration
        //private static readonly string host = CouponBuddy.Properties.Resources.EMAIL_HOST;
        //private static readonly string password = CouponBuddy.Properties.Resources.EMAIL_PASSWORD;
        //private static readonly string from = CouponBuddy.Properties.Resources.EMAIL_FROM;
        //private static readonly string user = CouponBuddy.Properties.Resources.EMAIL_USER;
        //#endregion

        //public static async void SendEmail(VendorCoupon coupon, string destination)
        //{
        //    var _db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;
        //    var vendor = await _db.GetVendor(coupon.VendorId).ConfigureAwait(true);
        //    string vendorName = vendor.Name;

        //    string subject = "E-Coupon Delivered By PG Technologies";
        //    StringBuilder bodyStringBuilder = new StringBuilder();
        //    bodyStringBuilder.AppendLine("Thank you for using our self service E-Coupon Kiosks");
        //    bodyStringBuilder.AppendLine("Here is your coupon:");
        //    bodyStringBuilder.AppendLine("Coupon offered by " + vendorName);
        //    bodyStringBuilder.AppendLine(coupon.Title);
        //    bodyStringBuilder.AppendLine(coupon.Description);
        //    bodyStringBuilder.AppendLine(coupon.Instructions);
        //    string body = bodyStringBuilder.ToString();

        //    EmailSendConfigure myConfig = new EmailSendConfigure();
        //    myConfig.ClientCredentialUserName = user;
        //    myConfig.ClientCredentialPassword = password;
        //    myConfig.To = destination;
        //    myConfig.From = from;
        //    myConfig.FromDisplayName = "Coupon Network";
        //    myConfig.Priority = MailPriority.Normal;
        //    myConfig.Subject = subject;

        //    EmailContent myContent = new EmailContent();
        //    myContent.Content = body;

        //    EmailManager mailMan = new EmailManager(host, myConfig);
        //    mailMan.SendMail(myConfig, myContent);
        //}

        //private class EmailManager
        //{
        //    private string m_HostName;
        //    private static SmtpClient client;

        //    public EmailManager(string hostName, EmailSendConfigure emailConfig)
        //    {
        //        m_HostName = hostName;
        //        if (client == null)
        //        {
        //            client = new SmtpClient();
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = new System.Net.NetworkCredential(
        //                                  emailConfig.ClientCredentialUserName,
        //                                  emailConfig.ClientCredentialPassword);
        //            client.Host = m_HostName;
        //            client.Port = 25;
        //            client.EnableSsl = true;
        //        }
        //    }

        //    public void SendMail(EmailSendConfigure emailConfig, EmailContent content)
        //    {
        //        MailMessage msg = ConstructEmailMessage(emailConfig, content);
        //        Send(msg, emailConfig);
        //    }

        //    private MailMessage ConstructEmailMessage(EmailSendConfigure emailConfig, EmailContent content)
        //    {
        //        MailMessage msg = new System.Net.Mail.MailMessage();
        //        msg.To.Add(emailConfig.To);

        //        msg.From = new MailAddress(emailConfig.From,
        //                                   emailConfig.FromDisplayName,
        //                                   System.Text.Encoding.UTF8);
        //        msg.IsBodyHtml = content.IsHtml;
        //        msg.Body = content.Content;
        //        msg.Priority = emailConfig.Priority;
        //        msg.Subject = emailConfig.Subject;
        //        msg.BodyEncoding = System.Text.Encoding.UTF8;
        //        msg.SubjectEncoding = System.Text.Encoding.UTF8;

        //        if (content.AttachFileName != null)
        //        {
        //            Attachment data = new Attachment(content.AttachFileName,
        //                                             MediaTypeNames.Application.Zip);
        //            msg.Attachments.Add(data);
        //        }

        //        return msg;
        //    }

        //    private void Send(MailMessage message, EmailSendConfigure emailConfig)
        //    {
        //        //client.SendAsync(emailConfig.From, emailConfig.To, emailConfig.Subject, message.Body, null);
        //        client.SendMailAsync(emailConfig.From, emailConfig.To, emailConfig.Subject, message.Body).GetAwaiter().GetResult();
        //        message.Dispose();
        //    }

        //}

        //private class EmailSendConfigure
        //{
        //    public string To { get; set; }
        //    public string From { get; set; }
        //    public string FromDisplayName { get; set; }
        //    public string Subject { get; set; }
        //    public MailPriority Priority { get; set; }
        //    public string ClientCredentialUserName { get; set; }
        //    public string ClientCredentialPassword { get; set; }
        //}

        //private class EmailContent
        //{
        //    public bool IsHtml { get; set; }
        //    public string Content { get; set; }
        //    public string AttachFileName { get; set; }
        //}
    }
}