using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private const string ContainerName = "vijicapstonecontainer";
        public const string SuccessMessageKey = "SuccessMessage";
        public const string ErrorMessageKey = "ErrorMessage";
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly IConfiguration _configuration;
        string connectionstring = "DefaultEndpointsProtocol=https;AccountName=vijicapstonecontainer;AccountKey=MxIcpgRGjjR0OCCOiUaR3cWjPle8OStgSo2z51wGYi7dG0IZdzwk5ddG6FgTyaSWpH1n4RCOL3dk+AStBBUygw==;EndpointSuffix=core.windows.net";
            public CurrencyController(IConfiguration configuration)
        {


           _configuration = configuration;
           // connectionstring = _configuration.GetConnectionString("StorageAccount");
            _blobServiceClient = new BlobServiceClient(connectionstring); 
                _containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                _containerClient.CreateIfNotExists();
            
        }

        [HttpGet]
        public string HealthCheck()
        {
            return "OK";
        }


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            bool status;
            try
            {
                var blobClient = _containerClient.GetBlobClient(file.FileName);
                await blobClient.UploadAsync(file.OpenReadStream(), true);
                status = true;
                return Ok();
            }
            catch (Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Route("Download")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(fileName);
                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;
                var contentType = blobClient.GetProperties().Value.ContentType;
                //return File(memoryStream, contentType, fileName);
                return Ok();
            }
            catch (Exception ex)
            {               
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }


    }
}
