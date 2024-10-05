using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexCore.DAL.Entities
{
    public class Participant
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string ParticipantName { get; set; }
        public string CarClass { get; set; }
        public string CarNumber { get; set; }
        //This can be Split into Year, Make, Model, but we're going with a single string for now
        // because that is what MotorsportsReg.com uses, which is what most clubs use for registration.
        public string CarYearMakeModel { get; set; }
        public string CarColor { get; set; }
        public RunResult[] RunResults { get; set; }
    }
}
