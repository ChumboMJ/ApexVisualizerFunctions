using ApexCore.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApexVisualizerFunctions
{
    public class DrivingEventFunctions
    {
        private readonly ILogger<DrivingEventFunctions> _logger;
        private readonly string _connectionString;
        private readonly ICosmosWriter _cosmosWriter;

        public DrivingEventFunctions(ILogger<DrivingEventFunctions> logger, IConfiguration configuration, ICosmosWriter cosmosWriter)
        {
            _logger = logger;
            _cosmosWriter = cosmosWriter;
            _connectionString = configuration.GetSection("SqlConnection")?.Value ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'SqlConnection' not found.");
        }

        [Function("DrivingEventFunctions")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
