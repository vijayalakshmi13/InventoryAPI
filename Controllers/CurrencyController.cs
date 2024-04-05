using Azure.Identity;
using Azure.Messaging.ServiceBus;
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
        public async Task sendMessageToTopic()
        {
         

            // the client that owns the connection and can be used to create senders and receivers
            ServiceBusClient client;

            // the sender used to publish messages to the topic
            ServiceBusSender sender;

            // number of messages to be sent to the topic
            const int numOfMessages = 1;

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.

            //TODO: Replace the "<NAMESPACE-NAME>" and "<TOPIC-NAME>" placeholders.
            client = new ServiceBusClient(
                "https://vijicapstone.servicebus.windows.net",
                new DefaultAzureCredential());
            sender = client.CreateSender("vijitopic");

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"vijiMessagereceived {i}")))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }

    }
}
