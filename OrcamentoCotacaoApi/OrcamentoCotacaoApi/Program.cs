using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace OrcamentoCotacaoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //NLog copiado do tutorial deles
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var app = CreateWebHostBuilder(args).Build();

                /*
                 * 
                 * para acessar diretamente do debug
                 * 
                  
                {
                    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddJsonFile("AppSettings.json");
                    IConfiguration configuration = configurationBuilder.Build();
                    var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InfraBanco.ContextoBd>();
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("conexaoLocal"));
                    var bll = new PrepedidoBusiness.Bll.ProdutoBll(new InfraBanco.ContextoProvider(optionsBuilder.Options));
                    var res = bll.BuscarTodosProdutos("teste");
                    var res2 = res.Result;
                }
                * */


                app.Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().ConfigureLogging(logging =>
                {
                    /*
                    para remover o |10400|WARN|Microsoft.EntityFrameworkCore.Model.Validation|Sensitive data logging is enabled. Log entries and exception messages may include sensitive application data, this mode should only be enabled during development.|
                    devemos colocar no nlog.config:
                    <logger name="Microsoft.EntityFrameworkCore.Model.Validation" minlevel="WARN" writeTo="logrequisicao" final="true" />
                    como o segundo logger
                    */

                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
#if DEBUG
                    logging.AddConsole();
                    logging.AddDebug();
#endif
                })
                .UseNLog();
        //.UseUrls("http://arclubeorcamentocotacaoapi.itssolucoes.com.br/");
    }
}
