using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCore.DAL.Entities
{
    internal class RunResult
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }
        public int RunNumber { get; set; }
        public int RunTime { get; set; }
        public int PenaltySeconds { get; set; }
        public bool DNF { get; set; }
    }
}
