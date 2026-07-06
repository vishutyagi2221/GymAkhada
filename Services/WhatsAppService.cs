using GymAkhada.Models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace GymAkhada.Services
{
    public interface IWhatsAppService
    {
        Task SendWhatsAppMessageAsync(string toMobileNumber, string message);
    }

    public class WhatsAppService : IWhatsAppService
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(IOptions<TwilioSettings> twilioSettings, ILogger<WhatsAppService> logger)
        {
            _twilioSettings = twilioSettings.Value;
            _logger = logger;
        }

        public async Task SendWhatsAppMessageAsync(string toMobileNumber, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_twilioSettings.AccountSid) || _twilioSettings.AccountSid == "your-twilio-account-sid")
                {
                    _logger.LogWarning("WhatsApp sending skipped: Twilio settings not configured properly.");
                    return; // Skip if dummy settings
                }

                TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

                // Format number if it doesn't have whatsapp: prefix
                var toWhatsApp = toMobileNumber.StartsWith("whatsapp:") ? toMobileNumber : $"whatsapp:+91{toMobileNumber}";
                var fromWhatsApp = _twilioSettings.WhatsAppNumber;

                var messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber(toWhatsApp))
                {
                    From = new Twilio.Types.PhoneNumber(fromWhatsApp),
                    Body = message
                };

                var messageResource = await MessageResource.CreateAsync(messageOptions);
                _logger.LogInformation($"WhatsApp message sent successfully to {toWhatsApp}. SID: {messageResource.Sid}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send WhatsApp to {toMobileNumber}. Error: {ex.Message}");
            }
        }
    }
}
