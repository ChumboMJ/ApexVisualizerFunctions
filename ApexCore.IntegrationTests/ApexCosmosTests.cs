using ApexCore.DAL;
using ApexCore.DAL.Entities;
using ApexCore.DAL.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApexCore.IntegrationTests
{
    [TestClass]
    public class ApexCosmosTests
    {
        private DrivingEvent _testEvent;
        private ICosmosWriter _cosmosWriter;
        private CosmosClient _cosmosClient;
        private Container _container;

        [TestInitialize]
        public void ApexCosmosSetUp()
        {
            var testResult1 = new RunResult()
            {
                // [Driving Event Id]-[Participant Id]-[Run Number]
                Id = "778899-12345-1", 
                RunNumber = 1,
                RunTime = 47.777m,
                PenaltySeconds = 3,
                DNF = true
            };

            var testResult2 = new RunResult()
            {
                Id = "778899-12345-2",
                RunNumber = 2,
                RunTime = 42.978m,
                PenaltySeconds = 1,
                DNF = false
            };

            var testResult3 = new RunResult()
            {
                Id = "778899-12345-3",
                RunNumber = 3,
                RunTime = 41.897m,
                PenaltySeconds = 0,
                DNF = false
            };

            var testParticipant = new Participant()
            {
                Id = "12345",
                ParticipantName = "Donald Duck",
                CarClass = "STU",
                CarNumber = "1337",
                CarYearMakeModel = "2022 Hyundai Elantra N",
                CarColor = "Red",
                RunResults = [testResult1, testResult2, testResult3]
            };

            var testEvent = new DrivingEvent() {
                Id = "778899.1",
                PartitionKey = "778899",
                EventName = "Apex Test Event Updated",
                EventDate = DateTime.Now,
                Location = "Test Location",
                CarClubName = "PNW N Club",
                Participants = [testParticipant]
            };

            _testEvent = testEvent;

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _cosmosWriter = serviceProvider.GetRequiredService<ICosmosWriter>();
            _cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];
            _container = _cosmosClient.GetContainer(databaseId, containerId);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CosmosClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["CosmosDb:ConnectionString"];
                return new CosmosClient(connectionString);
            });

            services.AddScoped<ICosmosWriter, ApexCosmosWriter>();
            services.AddLogging();
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
        }

        [TestMethod]
        [Priority(0)]
        public async Task InsertTestDrivingEventAsync()
        {
            var response = await _cosmosWriter.UpsertDrivingEventAsync(_testEvent);

            Assert.IsNotNull(response);
            Assert.AreEqual(_testEvent.Id, response.Resource.Id);
        }


        //TODO: This works, but Priority does not seem to garantee that this test runs after the InsertTestDrivingEventAsync test, and it needs to.
        [TestMethod]
        [Priority(1)]
        public async Task DeleteTestDrivingEventAsync()
        {
            var response = await _cosmosWriter.DeleteDrivingEventAsync(_testEvent.PartitionKey, _testEvent.Id);

            Assert.IsNotNull(response);
            //  A successful delete operation in Cosmos DB returns a status code of 204 No Content, so we'll use
            //  the HttpStatusCode to verify that the status code is correct.
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}