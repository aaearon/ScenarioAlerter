using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScenarioAlerter;
using ScenarioAlerter.AlertServices;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.AddXmlFile("App.config");
    })
    .ConfigureServices((_, services) =>
    {
        services.AddHttpClient();
        services.AddLogging();

        // "Options" pattern    
        services.AddSingleton(new AlerterOptions
        {
            LogFile = _.Configuration["logFile"],
        })
        .AddSingleton<IScenarioAlerter, Alerter>();

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
                ApplicationToken = _.Configuration["pushoverConfig:applicationToken"],
            });
            services.AddSingleton<IAlertService, PushoverService>();
        }
    })
    .Build();


host.Services.GetRequiredService<IScenarioAlerter>();

await host.RunAsync();