using Newtonsoft.Json;

namespace PsIntegrations.Models
{
    public class SmsMessage
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("body")]
        public string Message { get; set; }
    }
}
