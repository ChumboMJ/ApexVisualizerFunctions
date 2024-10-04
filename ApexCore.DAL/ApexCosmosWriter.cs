using ApexCore.DAL.Entities;
using ApexCore.DAL.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ApexCore.DAL
{
    public class ApexCosmosWriter : ICosmosWriter
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<ApexCosmosWriter> _logger;

        public ApexCosmosWriter(CosmosClient cosmosClient, IConfiguration configuration, ILogger<ApexCosmosWriter> logger)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];
            _container = _cosmosClient.GetContainer(databaseId, containerId);
        }

        public async Task<ItemResponse<DrivingEvent>> UpsertDrivingEvent(DrivingEvent drivingEvent)
        {
            try
            {
                var response = await _container.UpsertItemAsync(drivingEvent, new PartitionKey(drivingEvent.PartitionKey));
                _logger.LogInformation("Upserted item with id: {Id}", drivingEvent.Id);
                return response;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Error upserting item with id: {Id}", drivingEvent.Id);
                throw;
            }
        }
    }
}
