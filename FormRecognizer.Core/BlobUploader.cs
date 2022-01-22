using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class BlobUploader
    {
        private readonly string connectionString = "storageAccountConnectionString";
        private BlobContainerClient containerClient;

        public async Task<string> UploadSync(string fileName, string filePath)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);

            var containerName = "formrecognizerblobs" + Guid.NewGuid().ToString();

            containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(filePath, true);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task DeleteBlobAsync()
        {
            await containerClient.DeleteAsync();
        }
    }
}
