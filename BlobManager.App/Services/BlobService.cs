using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BlobListing> ListBlobsAsync(string containerName, string prefix = null)
        {
            var account = GetAccount();
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);

            BlobContinuationToken token = null;
            var blobs = new List<CloudBlockBlob>();
            var folders = new List<CloudBlobDirectory>();
            do
            {                
                var response = await container.ListBlobsSegmentedAsync(prefix, token);
                token = response.ContinuationToken;
                blobs.AddRange(response.Results.OfType<CloudBlockBlob>());
                folders.AddRange(response.Results.OfType<CloudBlobDirectory>());
            } while (token != null);

            return new BlobListing() { Blobs = blobs, Folders = folders };
        }

        private CloudStorageAccount GetAccount()
        {
            return new CloudStorageAccount(new StorageCredentials(_accountName, _accountKey), true);
        }

        public class BlobListing
        {
            public IEnumerable<CloudBlockBlob> Blobs { get; set; }
            public IEnumerable<CloudBlobDirectory> Folders { get; set; }
        }
    }
}
