using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using SendGrid;
using SendGrid.Helpers.Mail;


namespace Lectio2EReader
{
    class EmailSender
    {
        private SendGridClient client;
        public EmailSender(IConfigurationRoot config)
        {
            var sgKey = config.GetConnectionString("SGApiKey");
            if (String.IsNullOrEmpty(sgKey))
                throw new ArgumentNullException("SGApiKey in Settings");

            client = new SendGridClient(sgKey);
        }

        public async Task SendText(string from, string to, string subject, string textContent, IDictionary<string, string> attachments = null)
        {
            if (String.IsNullOrEmpty(to))
                throw new ArgumentNullException("to");
            if (String.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");


            if (String.IsNullOrEmpty(textContent))
                textContent = ".";

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from),
                Subject = subject,
                PlainTextContent = textContent,
            };
            msg.AddTo(new EmailAddress(to));

            if (attachments != null)
                foreach (var k in attachments.Keys)
                    msg.AddAttachment(k, attachments[k]);

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {response.StatusCode},\r\n {error}");
            }
        }
    }
}
