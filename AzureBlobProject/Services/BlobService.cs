using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Services.IServices;

namespace AzureBlobProject.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task<bool> DeleteBlobAsync(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            BlobClient blobClient=blobContainerClient.GetBlobClient(name);
            var result = await blobClient.DeleteIfExistsAsync();
            if (result.Value)
            {
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<string>> GetAllBlobsAsync(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            if (blobContainerClient is not null)
            {
                var blobs = blobContainerClient.GetBlobsAsync();
                List<string>blobsList=new List<string>();
                if (blobs is not null)
                {
                    await foreach (var item in blobs)
                    {
                        blobsList.Add(item.Name);
                    }
                }
                return blobsList;
            }
            return Enumerable.Empty<string>();
            
        }

        public async Task<string> GetBlobAsync(string name, string containerName)
        {
           BlobContainerClient blobContainerClient=_blobClient.GetBlobContainerClient(containerName);
           //when we are checking for individual blobs, in that case we have to create client object first.
           BlobClient blobClient=blobContainerClient.GetBlobClient(name);
           //string downloadPath=Directory.GetCurrentDirectory()+"\\download\\";
           //string fileName = downloadPath + blobClient.Name;
           //await blobClient.DownloadToAsync(fileName);
           return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlobAsync(string name, string containerName, IFormFile file)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            bool checkContainerExist = await blobContainerClient.ExistsAsync();
            if (checkContainerExist)
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(name);
                var httpHeaders = new BlobHttpHeaders()
                {

                    ContentType = file.ContentType
                };
                var result =await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
                if (result is not null)
                {
                    return true;
                }
                return false;
            }
            return false;
            

        }
    }
}
