using Microsoft.WindowsAzure.Storage.Table;

namespace AzurecomStatsFunctions.Shared
{
    public class BundleData : TableEntity
    {
        public string FriendlyDate => Timestamp.DateTime.ToString("MMMM dd, yyyy hh:mm:ss");
        public long HomepageHtmlPayloadSize { get; set; }
        public long HomepageCssBundleSize { get; set; }
        public long HomepageJsBundleSize { get; set; }
        public long CoreCssBundleSize { get; set; }
        public long CoreJsBundleSize { get; set; }
    }
}
