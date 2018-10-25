using Microsoft.WindowsAzure.Storage.Table;

namespace AzurecomStatsFunctions.Shared
{
    public class PatternData : TableEntity
    {
        public string FriendlyDate => Timestamp.DateTime.ToString("MMMM dd, yyyy hh:mm:ss");
        public string ProductionComponents { get; set; }
        public string DeprecatedComponents { get; set; }
        public string ProductionModules { get; set; }
        public string DeprecatedModules { get; set; }
    }

    public class Component
    {
        public string Slug { get; set; }
        public string BaseComponentSlug { get; set; }
        public int Status { get; set; }
    }

    public class Module
    {
        public string Slug { get; set; }
        public string BaseModuleSlug { get; set; }
        public int Status { get; set; }
    }
}
