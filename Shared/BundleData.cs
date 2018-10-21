using Microsoft.WindowsAzure.Storage.Table;

namespace AzurecomStatsFunctions.Shared
{
    public class BundleData : TableEntity
    {
        public long HomepageHtmlPayloadSize { get; set; }
        public long HomepageCssBundleSize { get; set; }
        public long HomepageJsBundleSize { get; set; }
        public long CoreCssBundleSize { get; set; }
        public long CoreJsBundleSize { get; set; }
    }
}
