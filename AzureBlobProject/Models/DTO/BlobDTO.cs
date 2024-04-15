using System.ComponentModel.DataAnnotations;

namespace AzureBlobProject.Models.DTO
{
    public class BlobDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContainerName { get; set; }
    }
}
