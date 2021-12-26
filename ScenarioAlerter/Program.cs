using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using ScenarioAlerter.AlertServices;
using ScenarioAlerter;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.AddXmlFile("App.config");
    })
    .ConfigureServices((_, services) =>
        // "Options" pattern    
        services.AddSingleton(new AlerterOptions
        {
            LogFile = _.Configuration["logFile"],
            AlertMethod = _.Configuration["alertMethod"]
        })
        .AddSingleton<IScenarioAlerter, Alerter>()
        .AddSingleton(new PushoverConfig
        {
            UserToken = _.Configuration["pushoverConfig:userToken"],
            ApplicationToken = _.Configuration["PUSHOVER_APPLICATIONTOKEN"]
        })
        .AddSingleton<IAlertService, PushoverService>()
        .AddHttpClient()
        .AddLogging())
        .Build();

host.Services.GetRequiredService<IScenarioAlerter>();

await host.RunAsync();
