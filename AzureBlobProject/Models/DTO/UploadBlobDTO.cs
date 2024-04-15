using System.ComponentModel.DataAnnotations;

namespace AzureBlobProject.Models.DTO
{
    public class UploadBlobDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContainerName { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
