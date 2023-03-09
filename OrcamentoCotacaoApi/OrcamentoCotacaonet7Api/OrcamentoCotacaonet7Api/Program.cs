//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace OrcamentoCotacaonet7Api
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
                var app = CreateHostBuilder(args).Build();

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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             }).ConfigureLogging(logging =>
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
             }).UseNLog();

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //   Host.CreateDefaultBuilder(args)
        //       .ConfigureWebHostDefaults(webBuilder =>
        //       {
        //           webBuilder.UseStartup<Startup>();
        //       });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
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
                }).UseNLog();
    }
}
