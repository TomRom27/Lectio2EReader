using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Extensions.Configuration;

namespace Lectio2EReader
{
    public static class Main
    {
        [FunctionName("SendToEReader")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log, ExecutionContext context)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var cstr = config.GetConnectionString("SqlConnectionString");

            var setting1 = config["Setting1"];

            log.Info(cstr);
            log.Info(setting1);


            var sender = new KindleSender();
            sender.Send();
            log.Info($"C# Sent at: {DateTime.Now}");
        }
    }
}
