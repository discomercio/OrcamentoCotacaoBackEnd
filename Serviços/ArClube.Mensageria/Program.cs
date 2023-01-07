using ArClube.Mensageria;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using NLog.Extensions.Logging;

var configuration = new ConfigurationBuilder()
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .AddJsonFile("appsettings.json")
       .Build();


IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "ArClube - Mensageria";
     })
    .ConfigureServices(services =>
    {
        var eventLogSettings = new EventLogSettings()
        {
            SourceName = "NLog - ArClube"
        };
        eventLogSettings.Filter = (_, logLevel) => logLevel >= LogLevel.Debug;

        services.AddHostedService<Worker>();

        services.AddTransient<IMensageriaRepositorio>(s => new MensageriaRepositorio(configuration.GetSection("DatabaseConnection").Value));

        services.AddSingleton<ConfigurationBuilder, ConfigurationBuilder>();
        services.AddLogging(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Trace)
                .AddProvider(new NLogLoggerProvider())
                .AddEventLog(eventLogSettings)
                .AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
        });
    }).Build();

await host.RunAsync();