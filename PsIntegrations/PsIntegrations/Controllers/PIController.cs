using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PSHelpers.SmsSender;
using PsIntegrations.Models;

namespace PsIntegrations.Constants
{
    [ApiController, Route("api/[controller]")]
    public class PIController : ControllerBase
    {
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;

        public PIController(ISmsSender smsSender, IMemoryCache cache)
        {
            _smsSender = smsSender;
            _cache = cache;
        }

        [HttpPost("ContactUpdated")]
        public IActionResult ContactUpdated(ContactUpdated contactUpdated)
        {
            var messageResult = _smsSender.Send(new PSHelpers.SmsSender.Models.SmsMessageData
            {
                Username = "development.professionalservices@esendex.com",
                Password = "X5RvZKsYvd6z",
                EsendexAccountReference = "EX0163963",
                Recipients = contactUpdated.MobileNumber,
                SmsBody = "Thank you for opting in to recieve SMS updates",
                Originator = "Salesforce"
            });

            if (messageResult.Sent)
            {
                var messageId = messageResult.MessagingResult.MessageIds.First().Id;
                _cache.Set(messageId, contactUpdated);
            }

            return Ok();
        }
    }
}
