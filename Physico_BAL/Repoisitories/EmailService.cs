using Physico_BAL.Contracts;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class EmailService : IEmailService
    {
        private readonly System.Net.Mail.SmtpClient _smtpClient;
        private string _senderEmail;

        //public EmailService(SmtpClient smtpClient, string senderEmail)
        //{
        //    _smtpClient = smtpClient;
        //    _senderEmail = senderEmail;
        //}
        public async Task SendReservationEmail(string email , string name,TimeSpan time , DateOnly date)
        {

            var setting = new EmailSettings
            {
                SenderEmail = "physiotime3@gmail.com",
                SmtpServer = "smtp.gmail.com",
                EnableSsl = true,
                SmtpPassword = "cvulhrslnjvxddhg",
                SmtpUsername = "physiotime3@gmail.com",
                SmtpPort = 587
            };
            _senderEmail = setting.SenderEmail;

            var dayName = date.DayOfWeek.ToString();
            var formattedTime = time.ToString(@"hh\:mm");
            var subject = $"Hi {name}! Your Session with Physio_Time_clinic at {formattedTime} on {dayName}, {date} is scheduled.";
            var body = $"Location: <a href=\"https://maps.app.goo.gl/VJA8RfMu4Yu8fWGy9?g_st=iw\">Google Maps</a><br><br>"
                       + "To cancel your session please contact us on:<br>"
                       + "+20 102 824 2712<br>"
                       + "+20 122 220 9731";
            var _smtpClient = new SmtpClient(setting.SmtpServer, setting.SmtpPort ?? 0)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(setting.SmtpUsername, setting.SmtpPassword),
                EnableSsl = setting.EnableSsl,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
