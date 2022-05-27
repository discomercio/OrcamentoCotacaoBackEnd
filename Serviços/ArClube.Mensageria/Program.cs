using ArClube.Mensageria;
using InfraBanco;
using Microsoft.EntityFrameworkCore;
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
        //services.AddHttpClient<GruposAD>();
        services.AddDbContext<ContextoBdBasico>(options =>
        {
            options.UseSqlServer(configuration.GetSection("DatabaseConnection").Value);
            options.EnableSensitiveDataLogging();
        });
        //services.AddDbContext<Contexto>(options =>
        //{
        //    options.UseSqlServer(configuration.GetSection("DatabaseConnection").Value, null);
        //    //options.UseOracle(configuration.GetSection("DatabaseConnection").Value, null);
        //    options.EnableSensitiveDataLogging();
        //});
        services.AddSingleton<ContextoBdGravacaoOpcoes>(c =>
        {
            return new ContextoBdGravacaoOpcoes(bool.Parse(configuration.GetSection("TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO").Value));
        });
        services.AddTransient<ContextoBdProvider, ContextoBdProvider>();
        services.AddTransient<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll>();
        services.AddTransient<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueData, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueData>();

        services.AddTransient<Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroBll, Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroBll>();
        services.AddTransient<Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroData, Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroData>();

        services.AddTransient<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll, Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll>();
        services.AddTransient<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioData, Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioData>();

        services.AddTransient<Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll, Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll>();
        services.AddTransient<Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateData, Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateData>();

        services.AddSingleton<ConfigurationBuilder, ConfigurationBuilder>();
        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddProvider(new NLogLoggerProvider())
                .AddEventLog(eventLogSettings);
            //builder.SetMinimumLevel(LogLevel.);
            builder.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });
        });
    }).Build();

await host.RunAsync();