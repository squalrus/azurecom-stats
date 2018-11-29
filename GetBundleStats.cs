using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using AzurecomStatsFunctions.Shared;

namespace AzurecomStatsFunctions
{
    public static class GetBundleStats
    {
        [FunctionName("bundles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("bundles")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            TableQuery<BundleData> rangeQuery = new TableQuery<BundleData>().Where(
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, DateTimeOffset.UtcNow.AddDays(-30))
            );

            List<BundleData> bundleData = new List<BundleData>();
            foreach (BundleData entity in await cloudTable.ExecuteQuerySegmentedAsync(rangeQuery, null))
            {
                bundleData.Add(entity);
                log.LogInformation($"{entity.PartitionKey}\t{entity.RowKey}\t{entity.Timestamp}");
            }

            return new JsonResult(bundleData.OrderByDescending(x => x.Timestamp));
        }
    }
}
