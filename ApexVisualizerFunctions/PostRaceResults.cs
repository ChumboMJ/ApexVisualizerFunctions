using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApexVisualizerFunctions
{
    public class PostRaceResults
    {
        private readonly ILogger<PostRaceResults> _logger;
        private readonly string _connectionString;

        public PostRaceResults(ILogger<PostRaceResults> logger, IConfiguration configuration)
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
                SqlCommand command = new SqlCommand("SELECT TOP (1) * FROM [SalesLT].[Address]", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Process the data (example)
                    var result = reader["AddressLine1"].ToString();
                    return new OkObjectResult($"Race result: {result}");
                }
                else
                {
                    return new NotFoundResult();
                }
            }

            //return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
