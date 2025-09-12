using ArtEShop.Domain.Email;
using ArtEShop.Service.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SendersName, _mailSettings.SmtpUserName));
            email.To.Add(new MailboxAddress(message.MailTo, message.MailTo));
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message.Content };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("localhost", 1025);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


    }
}

