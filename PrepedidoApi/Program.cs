using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace PrepedidoApi
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
                Microsoft.EntityFrameworkCore.DbContextOptions<InfraBanco.ContextoBd> Opt = new Microsoft.EntityFrameworkCore.DbContextOptions<InfraBanco.ContextoBd>();
                var bll = new PrepedidoBusiness.Bll.ClienteBll(new InfraBanco.ContextoProvider(Opt));
                var res = bll.BuscarCliente("34");
                */

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
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
#if DEBUG
                    logging.AddConsole();
                    logging.AddDebug();
#endif
                })
                .UseNLog()
            // .UseUrls("http://localhost:4000") ??
            ;
    }
}
