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
            var contentBase64 = await GetFileAsBase64(link);
            var filename = ExtractFilename(link);
            // todo


            System.Collections.Generic.Dictionary<string, string> att = new System.Collections.Generic.Dictionary<string, string>();
            att.Add(filename, contentBase64);

            await emailSender.SendText("tomekr.kindle@or.pl", "tomek.romanowski@gmail.com", "TEST 1", link + "  Sent at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), att);
        }

        private System.Net.Http.HttpClient httpClient = null;
        private System.Net.Http.HttpClient HTTPClient
        {
            get
            {
                if (httpClient == null)
                    httpClient = new System.Net.Http.HttpClient();
                return httpClient;
            }
        }

        private async Task<string> GetFileAsBase64(string url)
        {
            var bytes = await HTTPClient.GetByteArrayAsync(url);
            return System.Convert.ToBase64String(bytes);
        }

        private string ExtractFilename(string link)
        {
            var uri = new Uri(link);

            return System.IO.Path.GetFileName(uri.LocalPath);
        }

    }
}
