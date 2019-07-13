using JsonSettings;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using WinForms.Library.Models;

namespace BlobManager.App.Models
{
    public class Options : JsonSettingsBase
    {
        public override Scope Scope => Scope.User;
        public override string CompanyName => "Adam O'Neil";
        public override string ProductName => "HelloBlob";
        public override string Filename => "settings.json";

        public FormPosition FormPosition { get; set; }

        public HashSet<StorageAccount> Accounts { get; set; } = new HashSet<StorageAccount>();

        public Dictionary<string, StorageAccount> GetAccount
        {
            get { return Accounts?.ToDictionary(row => row.Name); }
        }

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

            public override bool Equals(object obj)
            {
                var test = obj as StorageAccount;
                return (test != null) ? test.Name.ToLower().Equals(test.Name.ToLower()) : false;
            }

            public override int GetHashCode()
            {
                return Name.ToLower().GetHashCode();
            }
        }
    }
}
