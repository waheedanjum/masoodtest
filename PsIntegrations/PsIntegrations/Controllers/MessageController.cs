using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PsIntegrations.Interfaces;
using PsIntegrations.Models;
using System.Xml.Serialization;

namespace PsIntegrations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IJwtService _jwtHandler;
        private readonly IHttpService _httpClient;
        public MessageController(IJwtService jwtHandler, IMemoryCache cache, IHttpService httpCleint)
        {
            _jwtHandler = jwtHandler;
            _cache = cache;
            _httpClient = httpCleint;
        }

        [HttpGet("Delivered")]
        public async Task<IActionResult> Delivered()
        {
            try
            {
                SmsDelivered smsReceipt = new SmsDelivered();

                var body = await new StreamReader(Request.Body).ReadToEndAsync();

                using (var reader = new StringReader(body))
                {
                    var serializer = new XmlSerializer(typeof(SmsDelivered));
                    if (string.IsNullOrEmpty(body))
                    {
                        smsReceipt = new SmsDelivered();
                    }
                    else
                    {
                        smsReceipt = (SmsDelivered)serializer.Deserialize(reader);
                    }
                }

                if (_cache.TryGetValue<ContactUpdated>(smsReceipt.MessageId, out var contactUpdated))
                {
                    ContactUpdated contactUpdated1 = new ContactUpdated();
                    var responseSMSDeliver = await _httpClient.PostAsync("https://api.useparagon.com/projects/b8636b6f-b180-4269-8502-67ea33c2b1d1/sdk/events/trigger",
                            new
                            {
                                name = "Sms Delivered (Contact Update)",
                                payload = new
                                {
                                    contactUpdated1.ContactId,
                                    NoteContents = GetNoteContents(smsReceipt.OccurredAt, contactUpdated1.UpdaterName)
                                }
                            });

                    var responseBodyDeliver = await responseSMSDeliver.Content.ReadAsStringAsync();
                }

                if (_cache.TryGetValue<Sms>(smsReceipt.MessageId, out var sms))
                {
                    var response1 = await _httpClient.PostAsync("https://api.useparagon.com/projects/b8636b6f-b180-4269-8502-67ea33c2b1d1/sdk/events/trigger",
                         new
                         {
                             name = "Sms Delivered (Sms)",
                             payload = new
                             {
                                 sms.SmsId,
                                 smsReceipt.OccurredAt
                             }
                         });
                    var responseBody1 = await response1.Content.ReadAsStringAsync();
                }

                var response2 = await _httpClient.PostAsync("https://api.useparagon.com/projects/b8636b6f-b180-4269-8502-67ea33c2b1d1/sdk/events/trigger",
                        new
                        {
                            name = "Sms Delivered (Campaign Message)",
                            payload = new
                            {
                                smsReceipt.MessageId,
                                smsReceipt.OccurredAt
                            }
                        });
                var responseBody2 = await response2.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [HttpPost("Received")]
        public async Task<IActionResult> Received()
        {
            try
            {
                SmsReceived smsReceived;
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                using (var reader = new StringReader(body))
                {
                    var serializer = new XmlSerializer(typeof(SmsReceived));
                    if (string.IsNullOrEmpty(body))
                    {
                        smsReceived = new SmsReceived();
                    }
                    else
                    {
                        smsReceived = (SmsReceived)serializer.Deserialize(reader);
                    }
                }
                var response = await _httpClient.PostAsync("https://api.useparagon.com/projects/b8636b6f-b180-4269-8502-67ea33c2b1d1/sdk/events/trigger",
                        new
                        {
                            name = "Sms Received",
                            payload = new
                            {
                                smsReceived.From,
                                smsReceived.MessageText,
                                OccurredAt = DateTime.UtcNow
                            }
                        });
            }
            catch (Exception e)
            {

            }
            return Ok();
        }

        private string GetNoteContents(DateTime occurredAt, string updaterName)
        {
            var noteContents = $"Opt In SMS Delivered at {occurredAt:HH:mm} (Triggered by: {updaterName})";
            var noteBytes = System.Text.Encoding.UTF8.GetBytes(noteContents);
            return Convert.ToBase64String(noteBytes);
        }
    }
}
