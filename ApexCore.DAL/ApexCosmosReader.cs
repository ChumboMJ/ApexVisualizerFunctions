using ApexCore.DAL.Entities;
using ApexCore.DAL.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCore.DAL
{
    public class ApexCosmosReader : ICosmosReader
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly ILogger<ApexCosmosReader> _logger;

        public ApexCosmosReader(CosmosClient cosmosClient, IConfiguration configuration, ILogger<ApexCosmosReader> logger)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];

            _container = _cosmosClient.GetContainer(databaseId, containerId); 
        }

        public async Task<ItemResponse<DrivingEvent>> GetDrivingEventAsync(string partitionKeyValue, string id)
        {
            try
            {
                var partitionKey = new PartitionKeyBuilder().Add(id).Add(partitionKeyValue).Build();

                var result = await _container.ReadItemAsync<DrivingEvent>(id, partitionKey);

                return result;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, $"Error getting item with id: {id} and partitionKey {partitionKeyValue}");
                throw;
            }
        }
    }
}
