using AzurecomStatsFunctions.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace AzurecomStatsFunctions
{
    public static class ProcessSitemap
    {
        [FunctionName("process-sitemap")]
        [return: Table("sitemap")]
        public static SitemapData Run(
            [TimerTrigger("0 0 */1 * * *")]TimerInfo myTimer,
            [Blob("latest/full.txt", FileAccess.Write)] Stream sitemapBlob,
            ILogger log)
        {
            var pageNum = 1;
            var completed = false;
            var baseUrl = "https://azure.microsoft.com/robotsitemap/en-us/{0}/";
            List<string> urls = new List<string>();

            while (!completed)
            {
                try
                {
                    WebRequest request = WebRequest.Create(string.Format(baseUrl, pageNum));
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string payload = reader.ReadToEnd();
                    log.LogInformation($"Processing page {pageNum}...");

                    var matches = Regex.Matches(payload, @"<loc>(.*?)<\/loc>");
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            urls.Add(match.Groups[1].Value);
                        }
                        log.LogInformation($"found {matches.Count} URLs.\n");
                    }
                    else
                    {
                        completed = true;
                        log.LogInformation($"found 0 URLs -- completed!\n");
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                }

                pageNum++;
            }

            log.LogInformation($"Found {urls.Count()} URLs, {urls.Distinct().Count()} unique.");
            log.LogInformation($"Writing to Storage.");

            var distinctUrls = urls.Distinct();
            sitemapBlob.Write(Encoding.Default.GetBytes(string.Join('\n', distinctUrls.OrderBy(x => x).ToList())));

            return new SitemapData
            {
                PartitionKey = "Sitemap",
                RowKey = Guid.NewGuid().ToString(),

                UrlCount = urls.Count(),
                UniqueUrlCount = distinctUrls.Count(),

                BlogUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/blog\/(.*?)\/")).Count(),
                PodcastsUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/industries\/podcast\/(.*?)\/")).Count(),
                PricingDetailsUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/pricing\/details\/(.*?)\/")).Count(),
                ResourcesUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/resources\/(.*?)([a-z]{2}-[a-z]{2})\/")).Count(),
                ServicesUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/services\/(.*?)\/")).Count(),
                TemplatesUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/resources\/templates\/(.*?)\/")).Count(),
                UpdatesUrlCount = distinctUrls.Where(x => Regex.IsMatch(x, @"https:\/\/azure\.microsoft\.com\/en-us\/updates\/(.*?)\/")).Count(),
            };
        }
    }
}
