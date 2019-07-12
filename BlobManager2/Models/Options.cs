using JsonSettings;
using System.Security.Cryptography;
using WinForms.Library.Models;

namespace BlobManager2.Models
{
    public class Options : JsonSettingsBase
    {
        public override Scope Scope => Scope.User;
        public override string CompanyName => "Adam O'Neil";
        public override string ProductName => "HelloBlob";
        public override string Filename => "settings.json";

        public FormPosition FormPosition { get; set; }

        public StorageAccount[] Accounts { get; set; }

        public class StorageAccount
        {
            [JsonProtect(DataProtectionScope.CurrentUser)]
            public string Name { get; set; }

            [JsonProtect(DataProtectionScope.CurrentUser)]
            public string Key { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
