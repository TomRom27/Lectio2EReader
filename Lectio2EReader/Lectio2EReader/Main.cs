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
        // 0 */1 * * * *   <- every minute
        // 0 0 1 ? * SUN * <- every Synday 1 AM
        public static async Task Run([TimerTrigger("%Lectio2EReaderSchedule%")]TimerInfo myTimer, TraceWriter log, ExecutionContext context)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            string sentLinks = "";
            bool errorWhenMailing = false;
            EmailSender mailSender = null;
            try
            {
                mailSender = new EmailSender(config);
            }
            catch
            {
                errorWhenMailing = true;
            }
            try
            {
                log.Verbose("Getting list of links ...");
                var infoProvider = new LectioInfoProvider(config);
                var fileLinks = await infoProvider.GetLectioLinks(new LectioInfoProvider.LectioFiles[] { LectioInfoProvider.LectioFiles.RozwazaniaKrotkie, LectioInfoProvider.LectioFiles.LectioMobi });

                log.Verbose("Preparing to send to ereader");
                var fileSender = new KindleSender(config);



                foreach (var l in fileLinks)
                {
                    log.Verbose("Now sending for a link: " + l);
                    await fileSender.SendFileFromLinkAsync(l);
                    sentLinks += l + Environment.NewLine;
                }


                try
                {
                    if (!errorWhenMailing)
                        await mailSender.SendText(config["KindleEmailTo"], config["NotifyEmailTo"], "Sent to Kindle", sentLinks);
                }
                catch
                {
                    errorWhenMailing = true;
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                if (!errorWhenMailing)
                    await mailSender.SendText(config["KindleEmailTo"], config["NotifyEmailTo"], "NOT Sent to Kindle", sentLinks + "\r\n" + ex.Message);
            }

            log.Info($"C# Finished at: {DateTime.Now}");
        }
    }
}
