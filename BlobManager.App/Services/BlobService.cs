using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            var client = GetClient();

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

        private async Task<IEnumerable<T>> ListItemsAsync<T>(string containerName, string prefix = null)
        {
            CloudBlobClient client = GetClient();
            var container = client.GetContainerReference(containerName);

            BlobContinuationToken token = null;
            var results = new List<T>();

            do
            {
                var response = await container.ListBlobsSegmentedAsync(prefix, token);
                token = response.ContinuationToken;
                results.AddRange(response.Results.OfType<T>());
            } while (token != null);

            return results;
        }

        public async Task<BlobListing> ListBlobsAndDirectoriesAsync(string containerName, string prefix = null)
        {
            return new BlobListing()
            {
                Blobs = await ListBlobsAsync(containerName, prefix),
                Directories = await ListDirectoriesAsync(containerName, prefix)
            };
        }

        public async Task<IEnumerable<CloudBlobDirectory>> ListDirectoriesAsync(string containerName, string prefix = null)
        {
            return await ListItemsAsync<CloudBlobDirectory>(containerName, prefix);
        }

        public async Task<IEnumerable<CloudBlockBlob>> ListBlobsAsync(string containerName, string prefix = null)
        {
            return await ListItemsAsync<CloudBlockBlob>(containerName, prefix);
        }

        public async Task<IEnumerable<CloudBlockBlob>> SearchBlobsAsync(string containerName, string prefix, IProgress<SearchProgress> progress)
        {
            if (string.IsNullOrEmpty(prefix)) throw new ArgumentNullException(nameof(prefix));

            List<CloudBlockBlob> results = new List<CloudBlockBlob>();

            await SearchBlobsInnerAsyncR(results, containerName, string.Empty, prefix, progress);

            return results;
        }

        private async Task SearchBlobsInnerAsyncR(List<CloudBlockBlob> results, string containerName, string directoryName, string prefix, IProgress<SearchProgress> progress)
        {
            progress?.Report(new SearchProgress() { Path = directoryName, ContainerName = containerName, AccountName = _accountName });

            var blobs = await ListBlobsAsync(containerName, directoryName + prefix);
            results.AddRange(blobs);

            var folders = await ListDirectoriesAsync(containerName, directoryName);
            foreach (var folder in folders)
            {
                await SearchBlobsInnerAsyncR(results, containerName, folder.Prefix, prefix, progress);
            }
        }

        private CloudBlobClient GetClient()
        {
            var account = GetAccount();
            var client = account.CreateCloudBlobClient();
            return client;
        }

        public async Task<CloudBlockBlob> UploadFileAsync(string containerName, string path)
        {
            CloudBlobClient client = GetClient();
            var container = client.GetContainerReference(containerName);

            var blob = container.GetBlockBlobReference(Path.GetFileName(path));
            blob.Properties.ContentType = MimeMapping.GetMimeMapping(path);
            await blob.UploadFromFileAsync(path);
            
            return blob;
        }

        private CloudStorageAccount GetAccount()
        {
            return new CloudStorageAccount(new StorageCredentials(_accountName, _accountKey), true);
        }

        public class BlobListing
        {
            public IEnumerable<CloudBlockBlob> Blobs { get; set; }
            public IEnumerable<CloudBlobDirectory> Directories { get; set; }
        }

        public class SearchProgress
        {
            public string AccountName { get; set; }
            public string ContainerName { get; set; }
            public string Path { get; set; }
        }
    }
}
