using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Lectio2EReader
{
    class KindleSender
    {
        public KindleSender(IConfigurationRoot config)
        {

            /*
            var cstr = config.GetConnectionString("SqlConnectionString");

            var setting1 = config["Setting1"];
            */
        }

        public void SendFileFromLink(string link)
        {
            // todo

            /*
                         var sender = new EmailSender(config);

                System.Collections.Generic.Dictionary<string, string> att = new System.Collections.Generic.Dictionary<string, string>();
                att.Add("zał.docx", System.Convert.ToBase64String(System.IO.File.ReadAllBytes(@".\test.docx")));
                await sender.SendText("tomekr.kindle@or.pl", "tomek.romanowski@gmail.com", "TEST 1", "Sent at "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),att);
             */
        }
    }
}
