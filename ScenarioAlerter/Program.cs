using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ScenarioAlerter.AlertServices;
using ScenarioAlerter;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.AddXmlFile("App.config");
    })
    .ConfigureServices((_, services) =>
    {
        // "Options" pattern    
        services.AddSingleton(new AlerterOptions
        {
            LogFile = _.Configuration["logFile"],
        })
        .AddSingleton<IScenarioAlerter, Alerter>()
        .AddHttpClient()
        .AddLogging();

        var alertMethod = _.Configuration["alertMethod"];

        if (alertMethod.Equals("Discord"))
        {
            services.AddSingleton(new DiscordConfig()
            {
                WebhookUri = _.Configuration["discordConfig:webhookUri"]
            });
            services.AddSingleton<IAlertService, DiscordService>();
        }
        else
        {
            services.AddSingleton(new PushoverConfig
            {
                UserToken = _.Configuration["pushoverConfig:userToken"],
                // Environmental variable used at build time.
                ApplicationToken = _.Configuration["PUSHOVER_APPLICATIONTOKEN"]
            });
            services.AddSingleton<IAlertService, PushoverService>();
        }
    })
    .Build();


host.Services.GetRequiredService<IScenarioAlerter>();

await host.RunAsync();