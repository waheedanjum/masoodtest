using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PSHelpers.SmsSender;
using PsIntegrations.Models;

namespace PsIntegrations.Constants
{
    [ApiController, Route("api/[controller]")]
    public class SmsController : Controller
    {
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;

        public SmsController(ISmsSender smsSender, IMemoryCache cache)
        {
            _smsSender = smsSender;
            _cache = cache;
        }

        [HttpPost]
        public IActionResult Post(Sms sms)
        {
            var messageResult = _smsSender.Send(new PSHelpers.SmsSender.Models.SmsMessageData
            {
                Username = "development.professionalservices@esendex.com",
                Password = "X5RvZKsYvd6z",
                EsendexAccountReference = "EX0163963",
                Recipients = sms.MobileNumber,
                SmsBody = sms.Text,
                Originator = "Salesforce"
            });

            if (messageResult.Sent)
            {
                var messageId = messageResult.MessagingResult.MessageIds.First().Id;
                _cache.Set(messageId, sms);
            }

            return Ok();
        }
    }
}
