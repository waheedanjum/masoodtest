using System;

namespace PsIntegrations.Models
{
    [Serializable, System.Xml.Serialization.XmlRoot("MessageDelivered")]
    public class SmsDelivered
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        public Guid AccountId { get; set; }

        public DateTime OccurredAt { get; set; }
    }
}
