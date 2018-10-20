using Microsoft.WindowsAzure.Storage.Table;

namespace Sitemap.Shared
{
    public class SitemapData : TableEntity
    {
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
