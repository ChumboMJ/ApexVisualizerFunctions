using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCore.DAL.Entities
{
    public class RunResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int RunNumber { get; set; }
        public decimal RunTime { get; set; }
        public decimal PenaltySeconds { get; set; }
        public bool DNF { get; set; }
    }
}
