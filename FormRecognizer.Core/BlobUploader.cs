using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FormRecognizer.Core
{
    public class BlobUploader
    {
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=anesheimstformrecognizer;AccountKey=uit9sUxfodPwG7SQwryjsVj0PNJ56D7SnGB+1FGb7J3Az9TzycYZQoV1TopvY33ZqNUYgRvf0zKrk8gs3/39AQ==;EndpointSuffix=core.windows.net";

        public async Task<string> UploadSync(string fileName, string filePath)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            //Create a unique name for the container
            string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.Blob);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file
            await blobClient.UploadAsync(filePath, true);

            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            var blobs = containerClient.GetBlobs();
            foreach (BlobItem blobItem in blobs)
            {
                Console.WriteLine("\t" + blobItem.Name);
                Console.WriteLine("\t" + blobClient.Uri.AbsoluteUri);
            }


            Console.WriteLine("Done");

            return blobClient.Uri.AbsoluteUri;
        }
    }
}
