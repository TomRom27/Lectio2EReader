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
        private IConfigurationRoot config;

        private string kindleConvertRequiredText;
        private string kindleConvertRequiredFormats;
        private string emailFrom;
        private string kindleEmailTo;

        public KindleSender(IConfigurationRoot config)
        {
            kindleConvertRequiredText = config["KindleConvertRequiredText"];
            if (String.IsNullOrEmpty(kindleConvertRequiredText))
                throw new ArgumentNullException("KindleConvertRequiredText in Settings");

            kindleConvertRequiredFormats = config["KindleConvertRequiredFormats"];
            if (String.IsNullOrEmpty(kindleConvertRequiredFormats))
                throw new ArgumentNullException("KindleConvertRequiredFormats in Settings");

            emailFrom = config["EmailFrom"];
            if (String.IsNullOrEmpty(emailFrom))
                throw new ArgumentNullException("EmailFrom in Settings");

            kindleEmailTo = config["KindleEmailTo"];
            if (String.IsNullOrEmpty(kindleEmailTo))
                throw new ArgumentNullException("KindleEmailTo in Settings");

            // KindleEmailTo
            emailSender = new EmailSender(config);
            this.config = config;
        }

        public async Task SendFileFromLinkAsync(string link)
        {
            var contentBase64 = await GetFileAsBase64(link);
            var filename = ExtractFilename(link);


            var subject = "no-subject";
            if (RequiresConversion(filename))
                subject = kindleConvertRequiredText;

            System.Collections.Generic.Dictionary<string, string> att = new System.Collections.Generic.Dictionary<string, string>();
            att.Add(filename, contentBase64);

            await emailSender.SendText(emailFrom, kindleEmailTo, subject, "", att);
        }

        private bool RequiresConversion(string filename)
        {
            var fileExt = System.IO.Path.GetExtension(filename);

            return (kindleConvertRequiredFormats.Contains(fileExt));
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
