using System;

namespace PsIntegrations.Models
{
    [Serializable, System.Xml.Serialization.XmlRoot("InboundMessage")]
    public class SmsReceived
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        public Guid AccountId { get; set; }

        public string From { get; set; }

        public string MessageText { get; set; }
    }
}
