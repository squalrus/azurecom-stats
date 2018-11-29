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
    public static class GetSitemapStats
    {
        [FunctionName("sitemap")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("sitemap")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            TableQuery<SitemapData> rangeQuery = new TableQuery<SitemapData>().Where(
                TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, DateTimeOffset.UtcNow.AddDays(-15))
            );

            List<SitemapData> sitemapData = new List<SitemapData>();
            foreach (SitemapData entity in await cloudTable.ExecuteQuerySegmentedAsync(rangeQuery, null))
            {
                sitemapData.Add(entity);
                log.LogInformation($"{entity.PartitionKey}\t{entity.RowKey}\t{entity.Timestamp}");
            }

            return new JsonResult(sitemapData.OrderByDescending(x => x.Timestamp));
        }
    }
}
