using Microsoft.WindowsAzure.Storage.Table;

namespace AzurecomStatsFunctions.Shared
{
    public class SitemapData : TableEntity
    {
        public string FriendlyDate => Timestamp.DateTime.ToString("MMMM dd, yyyy hh:mm:ss");
        public int UrlCount { get; set; }
        public int UniqueUrlCount { get; set; }
        public int BlogUrlCount { get; set; }
        public int ResourcesUrlCount { get; set; }
        public int SamplesUrlCount { get; set; }
        public int TemplatesUrlCount { get; set; }
        public int UpdatesUrlCount { get; set; }
        public int VideoUrlCount { get; set; }
    }

}
