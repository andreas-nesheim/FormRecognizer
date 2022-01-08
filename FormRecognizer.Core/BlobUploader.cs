using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class BlobUploader
    {
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=anesheimstformrecognizer;AccountKey=uit9sUxfodPwG7SQwryjsVj0PNJ56D7SnGB+1FGb7J3Az9TzycYZQoV1TopvY33ZqNUYgRvf0zKrk8gs3/39AQ==;EndpointSuffix=core.windows.net";
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
