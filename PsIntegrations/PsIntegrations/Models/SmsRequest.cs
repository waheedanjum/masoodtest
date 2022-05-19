using System.Collections.Generic;
using Newtonsoft.Json;

namespace PsIntegrations.Models
{
    public class SmsRequest
    {
        [JsonProperty("accountreference")]
        public string AccountReference { get; set; }

        [JsonProperty("messages")]
        public List<SmsMessage> Messages { get; set; }
    }
}
