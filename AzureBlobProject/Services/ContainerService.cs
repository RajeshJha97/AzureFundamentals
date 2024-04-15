using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Services.IServices;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;
        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            //whenever we are targeting individual container then in that case we have to use client .
            //for individual container: container client
            //for individual blob have to use blob client
            BlobContainerClient blobContainerClient=_blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        }
        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }
        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new();
            var blobContainers= _blobClient.GetBlobContainersAsync();
            await foreach (var blobContainer in blobContainers)
            {
                containerName.Add(blobContainer.Name);
            }
            return containerName;
        }
        //To retrieve the list of Blobs within the container
        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerBlobList = new();
            var blobContainerItems =  _blobClient.GetBlobContainersAsync();
            containerBlobList.Add("Account Name : " + _blobClient.AccountName);
            containerBlobList.Add("-----------------------------------------------------------------------------");
            await foreach (var blobContainerItem in blobContainerItems)
            {
                containerBlobList.Add($"---{blobContainerItem.Name}");
                BlobContainerClient _blobContainerClient = _blobClient.GetBlobContainerClient(blobContainerItem.Name);
                var _blobItems = _blobContainerClient.GetBlobsAsync();

                await foreach (var blobItem in _blobItems)
                {
                    containerBlobList.Add($"------{blobItem.Name}");
                }
                containerBlobList.Add("-----------------------------------------------------------------------------");
            }
            return containerBlobList;
        }
    }
}
