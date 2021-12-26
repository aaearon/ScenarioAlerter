using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioAlerter.AlertServices
{
    public class DiscordService : IAlertService
    {
        private readonly ILogger<IAlertService> _logger;
        private readonly HttpClient _httpClient;
        private readonly DiscordConfig _discordConfig;

        public string WebhookUri { get; set; }

        public DiscordService(ILogger<IAlertService> logger, HttpClient httpClient, DiscordConfig discordConfig)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _discordConfig = discordConfig ?? throw new ArgumentNullException(nameof(discordConfig));
        }

        public async void SendAlertAsync(string message)
        {
            _logger.LogInformation($"Sending Discord Webhook with message: {message}");

            Dictionary<string, string> webhookContent = new Dictionary<string, string>
            {
                { "content", message }
            };
            var json = JsonConvert.SerializeObject(webhookContent);
            var response = await _httpClient.PostAsync(_discordConfig.WebhookUri, new StringContent(json, UnicodeEncoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

        }
    }

    public class DiscordConfig
    {
        public string WebhookUri { get; set; }
    }
}
