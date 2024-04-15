using Azure.Storage.Blobs;
using AzureBlobProject.Models;
using AzureBlobProject.Models.DTO;
using AzureBlobProject.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace AzureBlobProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IBlobService _blobService;
        private readonly APIResponse _resp;
        public BlobController(BlobServiceClient blobServiceClient, IBlobService blobService)
        {
            _blobServiceClient = blobServiceClient;
            _blobService = blobService;
            _resp = new();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> GetAllBlobs([FromBody] string containerName)
        {
            try
            {
                IEnumerable<string> blobs = await _blobService.GetAllBlobsAsync(containerName);
                if (containerName != null)
                {
                    _resp.StatusCode = HttpStatusCode.OK;
                    _resp.Message = $"Total Number Of blobs: {blobs.Count()} in container {containerName}";
                    _resp.Result = blobs;
                    return Ok(_resp);
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No Container Found";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.StatusCode = HttpStatusCode.ExpectationFailed;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> GetBlobAsync([FromBody] BlobDTO blobDTO)
        {
            try
            {               
                if (blobDTO != null)
                {
                    var blob = await _blobService.GetBlobAsync(blobDTO.Name, blobDTO.ContainerName);
                    if (blob is not null)
                    {
                        _resp.StatusCode = HttpStatusCode.OK;                       
                        _resp.Result = blob;
                        return Ok(_resp);
                    }                    
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No blob Found";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.StatusCode = HttpStatusCode.ExpectationFailed;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> UploadFileAsync(IFormFile file, string containerName,Blob blob)
        {
            try
            {
                if (file.Length>0 && containerName is not null)
                {
                    var fileName=Path.GetFileNameWithoutExtension(file.FileName)+"_"+Guid.NewGuid()+Path.GetExtension(file.FileName);
                    var blobUpload = await _blobService.UploadBlobAsync(fileName, containerName, file);
                    if (blobUpload)
                    {
                        _resp.StatusCode = HttpStatusCode.OK;
                        _resp.Message = $"{fileName} uploaded successfully";
                        return Ok(_resp);
                    }
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No blob Found";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.StatusCode = HttpStatusCode.ExpectationFailed;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<APIResponse>> DeleteBlobAsync([FromBody] BlobDTO blobDTO)
        {
            try
            {
                if (blobDTO != null)
                {
                    var blob = await _blobService.DeleteBlobAsync(blobDTO.Name, blobDTO.ContainerName);
                    if (blob)
                    {
                        _resp.StatusCode = HttpStatusCode.OK;
                        _resp.Result = blob;
                        _resp.Message = $"{blobDTO.Name} deleted successfully.";
                        return Ok(_resp);
                    }
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No blob Found";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.StatusCode = HttpStatusCode.BadRequest;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }
    }
}
