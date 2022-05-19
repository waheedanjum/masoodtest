using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PSHelpers.SmsSender;
using System;
using System.Linq;

namespace PsIntegrations.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CampaignMembersSms : ControllerBase
    {
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;

        public CampaignMembersSms(ISmsSender smsSender, IMemoryCache cache)
        {
            _smsSender = smsSender;
            _cache = cache;
        }

        [HttpPost]
        public IActionResult Post(Models.CampaignMembersSmsRequest campaignMembersSms)
        {
            Guid messageId;

            var messageResult = _smsSender.Send(new PSHelpers.SmsSender.Models.SmsMessageData
            {
                Username = "development.professionalservices@esendex.com",
                Password = "X5RvZKsYvd6z",
                EsendexAccountReference = "EX0340995",
                Recipients = campaignMembersSms.MobileNumber,
                SmsBody = campaignMembersSms.SmsBody,
                //Originator = "447908675252"
            });

            if (messageResult.Sent)
            {
                messageId = messageResult.MessagingResult.MessageIds.First().Id;
            }
            else
            {
                return BadRequest();
            }

            return new OkObjectResult(new
            {
                MessageId = messageId,
                SendTime = DateTimeOffset.UtcNow
            });
        }

        [HttpPost("AddCampaignMembersSms")]
        public IActionResult AddCampaignMembersSms(Models.CampaignMembersSmsRequest campaignMembersSms)
        {
            Guid messageId;

            var messageResult = _smsSender.Send(new PSHelpers.SmsSender.Models.SmsMessageData
            {
                Username = "development.professionalservices@esendex.com",
                Password = "X5RvZKsYvd6z",
                EsendexAccountReference = "EX0163963",
                Recipients = campaignMembersSms.MobileNumber,
                SmsBody = campaignMembersSms.SmsBody,
                Originator = campaignMembersSms.Originator
            });

            if (messageResult.Sent)
            {
                messageId = messageResult.MessagingResult.MessageIds.First().Id;
            }
            else
            {
                return BadRequest();
            }

            return new OkObjectResult(new
            {
                MessageId = messageId,
                SendTime = DateTimeOffset.UtcNow
            });
        }
    }
}
