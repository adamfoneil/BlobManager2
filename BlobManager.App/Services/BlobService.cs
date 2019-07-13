using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobManager.App.Services
{
    /// <summary>
    /// Handles low-level storage access
    /// </summary>
    public class BlobService
    {
        private readonly string _accountName;
        private readonly string _accountKey;

        public BlobService(string accountName, string accountKey)
        {
            _accountName = accountName;
            _accountKey = accountKey;
        }

        public async Task<IEnumerable<string>> ListContainersAsync(string prefix = null)
        {
            var account = GetAccount();
            var client = account.CreateCloudBlobClient();

            BlobContinuationToken token = null;
            List<string> results = new List<string>();
            do
            {
                var response = await client.ListContainersSegmentedAsync(prefix, token);
                token = response.ContinuationToken;
                results.AddRange(response.Results.Select(c => c.Name));
            }
            while (token != null);

            return results;
        }

        private CloudStorageAccount GetAccount()
        {
            return new CloudStorageAccount(new StorageCredentials(_accountName, _accountKey), true);
        }
    }
}
