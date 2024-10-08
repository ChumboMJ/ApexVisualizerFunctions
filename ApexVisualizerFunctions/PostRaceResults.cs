using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ApexVisualizerFunctions
{
    public class PostRaceResults
    {
        private readonly ILogger<PostRaceResults> _logger;

        public PostRaceResults(ILogger<PostRaceResults> logger)
        {
            _logger = logger;
        }

        [Function("PostRaceResults")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
