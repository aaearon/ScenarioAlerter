using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioAlerter.AlertServices
{
    public class PushoverService : IAlertService
    {
        private const string MESSAGE_URI = "https://api.pushover.net/1/messages.json";

        private readonly ILogger<IAlertService> _logger;
        private readonly HttpClient _httpClient;
        private readonly PushoverConfig _pushoverConfig;

        public PushoverService(ILogger<IAlertService> logger, HttpClient httpClient, PushoverConfig pushoverConfig)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _pushoverConfig = pushoverConfig ?? throw new ArgumentNullException(nameof(pushoverConfig));
        }

        public async void SendAlertAsync(string message)
        {
            _logger.LogInformation($"Sending Pushover with message: {message}");

            Dictionary<string, string> messageContent = new Dictionary<string, string>
            {
                { "message", message },
                { "user", _pushoverConfig.UserToken },
                { "token", _pushoverConfig.ApplicationToken }
            };

            var json = JsonConvert.SerializeObject(messageContent);
            var response = await _httpClient.PostAsync(MESSAGE_URI, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }

    public class PushoverConfig
    {
        public string UserToken { get; set; }
        public string ApplicationToken { get; set; }
    }
}
