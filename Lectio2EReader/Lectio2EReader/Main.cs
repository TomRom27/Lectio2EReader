using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Extensions.Configuration;

namespace Lectio2EReader
{
    public static class Main
    {
        [FunctionName("SendToEReader")]
        [Disable("IsFuncDisabled")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log, ExecutionContext context)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            log.Info("Sending email...");

            var sender = new EmailSender(config);
            try
            {
                await sender.Send("tomek.romanowski@gmail.com", "tomekr.kindle@or.pl", "TEST 1", "Sent at "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
            
            log.Info($"C# Sent at: {DateTime.Now}");
        }
    }
}
