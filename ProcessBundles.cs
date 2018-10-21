using System;
using System.Net;
using AzurecomStatsFunctions.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzurecomStatsFunctions
{
    public static class ProcessBundles
    {
        [FunctionName("process-bundles")]
        [return: Table("bundles")]
        public static BundleData Run(
            [TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var homeHtmlUrl = "https://azure.microsoft.com/en-us/";
            var homeCssUrl = "https://azure.microsoft.com/dest/bundles/home.css";
            var homeJsUrl = "https://azure.microsoft.com/dest/bundles/home.js";
            var coreCssUrl = "https://azure.microsoft.com/dest/bundles/core.css";
            var coreJsUrl = "https://azure.microsoft.com/dest/bundles/core.js";

            return new BundleData
            {
                PartitionKey = "Bundle",
                RowKey = Guid.NewGuid().ToString(),
                HomepageHtmlPayloadSize = WebRequest.Create(homeHtmlUrl).GetResponse().ContentLength,
                HomepageCssBundleSize = WebRequest.Create(homeCssUrl).GetResponse().ContentLength,
                HomepageJsBundleSize = WebRequest.Create(homeJsUrl).GetResponse().ContentLength,
                CoreCssBundleSize = WebRequest.Create(coreCssUrl).GetResponse().ContentLength,
                CoreJsBundleSize = WebRequest.Create(coreJsUrl).GetResponse().ContentLength,
            };
        }
    }
}
