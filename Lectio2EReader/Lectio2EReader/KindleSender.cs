using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lectio2EReader
{
    class KindleSender
    {
        private EmailSender emailSender;

        public KindleSender(IConfigurationRoot config)
        {
            // todo settings  check
            /*
            var cstr = config.GetConnectionString("SqlConnectionString");

            var setting1 = config["Setting1"];
            */
            emailSender = new EmailSender(config);
        }

        public async Task SendFileFromLinkAsync(string link)
        {
            // todo

            /*
                System.Collections.Generic.Dictionary<string, string> att = new System.Collections.Generic.Dictionary<string, string>();
                att.Add("zał.docx", System.Convert.ToBase64String(System.IO.File.ReadAllBytes(@".\test.docx")));
             */
            await emailSender.SendText("tomekr.kindle@or.pl", "tomek.romanowski@gmail.com", "TEST 1",link + "Sent at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
