using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AreaBg.Web.Services
{
    public class MailService
    {
        private SmtpClient client { get; set; }

        private MailMessage mailMsg { get; set; }

        public MailService()
        {
            client = new SmtpClient("mail.area.bg");
            mailMsg = new MailMessage();
        }

        public void SendMail(string email, string body, string subject)
        {
            LinkedResource img = new LinkedResource("wwwroot/logoIcons/logoR-small.png", "image/png");
            img.ContentId = "logo";
            AlternateView altView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            altView.LinkedResources.Add(img);

            this.client.UseDefaultCredentials = false;
            this.client.Credentials = new NetworkCredential("sales@area.bg", "toni9363343");

            this.mailMsg.From = new MailAddress("sales@area.bg", "Area.bg / Книжарницата");
            this.mailMsg.To.Add(email);
            this.mailMsg.AlternateViews.Add(altView);
            this.mailMsg.Subject = subject;
            this.mailMsg.IsBodyHtml = true;

            client.Send(mailMsg);
        }
    }
}
