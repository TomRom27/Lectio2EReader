using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Lectio2EReader
{
    class LectioInfoProvider
    {
        public enum LectioFiles { RozwazaniaKrotkie, Lectio, LectioBroszura, LectioMobi };

        public LectioInfoProvider(IConfigurationRoot config)
        {
            // todo - get Lectio url from config
        }

        /*
	<div class="textwidget"><p><a name="rozwazania_krotkie" 
href="http://www.onjest.pl/slowo/wp-content/uploads/2018/07/rk180729_br.pdf">Rozważania krótkie</a><br />
<a href="http://www.onjest.pl/slowo/wp-content/uploads/2018/07/ld180729.pdf">Rozważania</a><br />
<a href="http://www.onjest.pl/slowo/wp-content/uploads/2018/07/ld180729_br.pdf">Rozważania &#8211; broszura</a></p>
<center><a  style="display:true" name = "mobi" href="http://www.onjest.pl/slowo/wp-content/uploads/2018/02/ebook180211.mobi"  target="_blank">          
          
         */

        public IEnumerable<string> GetLectioLinks(IList<LectioFiles> files)
        {
            string[] links = new string[files.Count];

            // todo
            // get web page, analyse and extract the links
            string current = "http://www.onjest.pl/slowo/wp-content/uploads/2018/07" + "/";
            for (int i = 0; i <= files.Count - 1; i++)
                switch (files[i])
                {
                    case LectioFiles.RozwazaniaKrotkie:
                        {
                            links[i] = current + "rk180729_br.pdf";
                            break;
                        }
                    case LectioFiles.LectioMobi:
                        {
                            links[i] = current + "ebook180729.mobi";
                            break;
                        }
                    default: throw new Exception(files[i].ToString() + " not supported");
                }

            return links;
        }
    }
}
