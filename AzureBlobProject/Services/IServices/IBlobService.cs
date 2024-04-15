namespace AzureBlobProject.Services.IServices
{
    public interface IBlobService
    {
        Task<string> GetBlobAsync(string name,string containerName);
        Task<IEnumerable<string>> GetAllBlobsAsync(string containerName);
        Task<bool> UploadBlobAsync(string name,string containerName,IFormFile file);
        Task<bool> DeleteBlobAsync(string name,string containerName);
    }
}
