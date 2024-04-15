using AzureBlobProject.Models;
using AzureBlobProject.Models.DTO;
using AzureBlobProject.Services;
using AzureBlobProject.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AzureBlobProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;
        private readonly List<Container> _containers;
        private readonly APIResponse _resp;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
            _containers = new List<Container>();
            _resp = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<string>>> GetAllContainerAsync()
        {
            try 
            {
                List<string> containerName = await _containerService.GetAllContainer();
                if (containerName != null)
                {
                    _resp.StatusCode=HttpStatusCode.OK;
                    _resp.Message = $"Total Number Of Container: {containerName.Count}";
                    _resp.Result = containerName;
                    return Ok(_resp);
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No Container Found";
                return Ok(_resp);
            }
            catch(Exception ex) 
            {
                _resp.StatusCode = HttpStatusCode.ExpectationFailed;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> CreateContainerAsync([FromBody] Container container)
        {
            if (container is not null)
            {
                await _containerService.CreateContainer(container.Name);                
                return Ok(container);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteContainerAsync([FromBody] Container container)
        {
            if (container is not null)
            {
                await _containerService.DeleteContainer(container.Name);
                return Created();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> GetAllContainerBlobsAsync()
        {
            try
            {
                IEnumerable<string> containersBlobsList = await _containerService.GetAllContainerAndBlobs();
                if (containersBlobsList != null)
                {
                    _resp.StatusCode = HttpStatusCode.OK;                  
                    _resp.Result = containersBlobsList;
                    return Ok(_resp);
                }
                _resp.StatusCode = HttpStatusCode.OK;
                _resp.Message = "No Container and Blobs found";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.StatusCode = HttpStatusCode.ExpectationFailed;
                _resp.Error = ex.Message;
                return BadRequest(_resp);
            }
        }
    }
}
