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
                throw new ArgumentNullException("SGApiKey");

            client = new SendGridClient(sgKey);
        }

        public async Task Send(string to, string from, string subject, string plainContent)
        {
            if (String.IsNullOrEmpty(to))
                throw new ArgumentNullException("to");
            if (String.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");


            if (String.IsNullOrEmpty(plainContent))
                plainContent = "";

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from),
                Subject = subject,
                PlainTextContent = plainContent,
            };
            msg.AddTo(new EmailAddress(to));

            //var filename = "test.docx";
            //msg.AddAttachment("tr" + filename, System.Convert.ToBase64String(System.IO.File.ReadAllBytes(@".\" + filename)));

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {response.StatusCode},\r\n {error}");
            }
        }
    }
}
