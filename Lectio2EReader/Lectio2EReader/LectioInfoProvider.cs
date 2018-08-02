using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace Lectio2EReader
{
    class LectioInfoProvider
    {
        public enum LectioFiles { RozwazaniaKrotkie, Lectio, LectioBroszura, LectioMobi };

        private string lectioUrl;
        public LectioInfoProvider(IConfigurationRoot config)
        {
            lectioUrl = config["LectioPageUrl"];
            if (String.IsNullOrEmpty(lectioUrl))
                throw new ArgumentNullException("LectioPageUrl in Settings");
        }

        private HttpClient httpClient = null;
        private HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                    httpClient = new HttpClient();
                return httpClient;
            }
        }

        public async Task<IEnumerable<string>> GetLectioLinks(IList<LectioFiles> files)
        {
            string[] links = new string[files.Count];

            var lectioHtml = await HttpClient.GetStringAsync(lectioUrl);


            return await Task.Run(() =>
            {
                // get web page and extract the links
 
                var parser = new HtmlParser();
                var lectioDocument = parser.Parse(lectioHtml);

                for (int i = 0; i <= files.Count - 1; i++)
                    switch (files[i])
                    {
                        case LectioFiles.RozwazaniaKrotkie:
                            {
                                links[i] = GetRozwazaniaKrotkieUrl(lectioDocument);
                                break;
                            }
                        case LectioFiles.LectioMobi:
                            {
                                links[i] = GetMobiUrl(lectioDocument);
                                break;
                            }
                        default: throw new Exception(files[i].ToString() + " not supported");
                    }

                return links;
            });
        }


        private string GetRozwazaniaKrotkieUrl(IHtmlDocument document)
        {
            var rkList = document.All.
                            Where(m => m.LocalName == "a" && m.GetAttribute("name") == "rozwazania_krotkie").
                            ToList();
            if (rkList.Count > 0)
                return rkList[0].GetAttribute("href");
            else
                return "";
        }


        private string GetMobiUrl(IHtmlDocument document)
        {
            // mobi url is created dynamically i.e. via javascript
            // but the file itself is updated at the same moment, as Lectio file is
            // therefore we look for Lectio url and then process it to get mobi url
            
            var rkList = document.All.
                            Where(m => m.LocalName == "a" && m.GetAttribute("name") == "lectio_divina").
                            ToList();
            if (rkList.Count > 0)
            {
                var foundRef = rkList[0].GetAttribute("href");
                // ld180729.pdf -> ebook180729.mobi
                if (!String.IsNullOrEmpty(foundRef))
                    return foundRef.Replace("ld", "ebook").Replace("pdf", "mobi");
            }

            return "";
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/html"));

            return httpClient;
        }
    }
}
