using System;
using System.Net;
using AzurecomStatsFunctions.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AzurecomStatsFunctions
{
    public static class ProcessPatterns
    {
        [FunctionName("process-patterns")]
        [return: Table("patterns")]
        public static PatternData Run(
            [TimerTrigger("0 0 0 */1 * *")]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var components = JsonConvert.DeserializeObject<Component[]>(ApiCall("https://sundog.azure.net/api/components"));
            var productionComponents = components.Where(x => x.Status == 1).GroupBy(x => x.BaseComponentSlug).ToDictionary(x => x.Key, x => x.Count());
            var deprecatedComponents = components.Where(x => x.Status == 0).GroupBy(x => x.BaseComponentSlug).ToDictionary(x => x.Key, x => x.Count());

            var modules = JsonConvert.DeserializeObject<Module[]>(ApiCall("https://sundog.azure.net/api/modules"));
            var productionModules = modules.Where(x => x.Status == 1).GroupBy(x => x.BaseModuleSlug).ToDictionary(x => x.Key, x => x.Count());
            var deprecatedModules = modules.Where(x => x.Status == 0).GroupBy(x => x.BaseModuleSlug).ToDictionary(x => x.Key, x => x.Count());


            return new PatternData
            {
                PartitionKey = "Patterns",
                RowKey = Guid.NewGuid().ToString(),
                ProductionComponents = JsonConvert.SerializeObject(productionComponents),
                DeprecatedComponents = JsonConvert.SerializeObject(deprecatedComponents),
                ProductionModules = JsonConvert.SerializeObject(productionModules),
                DeprecatedModules = JsonConvert.SerializeObject(deprecatedModules),
            };
        }

        private static string ApiCall(string url)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }
    }
}
