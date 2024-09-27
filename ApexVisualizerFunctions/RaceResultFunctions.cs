using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApexVisualizerFunctions
{
    public class RaceResultFunctions
    {
        private readonly ILogger<RaceResultFunctions> _logger;
        private readonly string _connectionString;

        public RaceResultFunctions(ILogger<RaceResultFunctions> logger, IConfiguration configuration)
        {
            logger.LogInformation("Configuration loaded: {config}", configuration.ToString());
            _logger = logger;

            //TODO: This works, but need to add a SqlConnection section to local.settings.json so we can use "GetConnectioString" instead
            _connectionString = configuration.GetSection("SqlConnection")?.Value ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'SqlConnection' not found.");

        }

        [Function("PostRaceResults")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //TODO: This currently returns the number 9, which is the value in the first column (ID) that matches the
                //      type used with QueryFirstOrDefault (string). Use a Full Object to represent the record and then
                //      select the full row for processing.
                var result = connection.QueryFirstOrDefault<string>("SELECT TOP (1) * FROM [SalesLT].[Address]");

                if (result != null)
                {
                    // Process the data (example)
                    return new OkObjectResult($"Race result: {result}");
                }
                else
                {
                    return new NotFoundResult();
                }
            }
        }
    }
}
