namespace PsIntegrations.Models
{
    public class CampaignMembersSmsRequest
    {
        public string Originator { get; set; }

        public string SmsBody { get; set; }

        public string MobileNumber { get; set; }
    }
}
