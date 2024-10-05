using ApexCore.DAL.Entities;

namespace ApexCore.IntegrationTests
{
    [TestClass]
    public class ApexCosmosTests
    {
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
                Id = "778899",
                PartitionKey = "Test Partition Key",
                EventName = "Apex Test Event",
                EventDate = DateTime.Now,
                Location = "Test Location",
                CarClubName = "PNW N Club",
                Participants = [testParticipant]
            };
        }

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}