using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PrepedidoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateWebHostBuilder(args).Build();

            /*
            Microsoft.EntityFrameworkCore.DbContextOptions<InfraBanco.ContextoBd> Opt = new Microsoft.EntityFrameworkCore.DbContextOptions<InfraBanco.ContextoBd>();
            var bll = new PrepedidoBusiness.Bll.ClienteBll(new InfraBanco.ContextoProvider(Opt));
            var res = bll.BuscarCliente("34");
            */

            app.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            // .UseUrls("http://localhost:4000") ??
            ;
    }
}
