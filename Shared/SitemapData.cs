using Microsoft.WindowsAzure.Storage.Table;

namespace AzurecomStatsFunctions.Shared
{
    public class SitemapData : TableEntity
    {
        public string FriendlyDate => Timestamp.DateTime.ToString("MMMM dd, yyyy hh:mm:ss");
        public int UrlCount { get; set; }
        public int UniqueUrlCount { get; set; }

        public int BlogUrlCount { get; set; }
        public int EventsUrlCount { get; set; }
        public int KnowledgeCentersUrlCount { get; set; }
        public int PodcastsUrlCount { get; set; }
        public int PricingDetailsUrlCount { get; set; }
        public int ResourcesUrlCount { get; set; }
        public int ServicesUrlCount { get; set; }
        public int SolutionArchitecturesUrlCount { get; set; }
        public int TemplatesUrlCount { get; set; }
        public int UpdatesUrlCount { get; set; }
        public int VideoUrlCount { get; set; }
    }

}
