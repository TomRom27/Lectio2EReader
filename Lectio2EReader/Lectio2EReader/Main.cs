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

            try
            {
                log.Verbose("Getting list of links ...");
                var infoProvider = new LectioInfoProvider(config);
                var fileLinks = await infoProvider.GetLectioLinks(new LectioInfoProvider.LectioFiles[] { LectioInfoProvider.LectioFiles.RozwazaniaKrotkie, LectioInfoProvider.LectioFiles.LectioMobi });

                log.Verbose("Preparing to send to ereader");
                var sender = new KindleSender(config);

                foreach (var l in fileLinks)
                {
                    log.Verbose("Now sending for a link: " + l);
                    await sender.SendFileFromLinkAsync(l);
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }

            log.Info($"C# Finished at: {DateTime.Now}");
        }
    }
}
