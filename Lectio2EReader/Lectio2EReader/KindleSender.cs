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

        public void SendFile(string filename)
        {

        }
    }
}
