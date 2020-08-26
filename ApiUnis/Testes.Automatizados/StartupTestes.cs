using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrepedidoUnisBusiness.UnisBll.CepUnisBll;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

[assembly: TestFramework("Testes.Automatizados.StartupTestes", "Testes.Automatizados")]

namespace Testes.Automatizados
{
    public class StartupTestes : DependencyInjectionTestFramework
    {
        public StartupTestes(IMessageSink messageSink) : base(messageSink) { }

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("conexaoLocal"));
                options.UseInMemoryDatabase("bancomemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseInMemoryDatabase("bancocepmemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<InicializarBanco.InicializarBancoCep, InicializarBanco.InicializarBancoCep>();
            services.AddTransient<InicializarBanco.InicializarBancoGeral, InicializarBanco.InicializarBancoGeral>();

            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepPrepedidoBll, PrepedidoBusiness.Bll.CepPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll, PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll, PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll, PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficienteBll, PrepedidoBusiness.Bll.CoeficienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll>();

            services.AddTransient<PrepedidoBusiness.UtilsNfe.IBancoNFeMunicipio, Utils.TestesBancoNFeMunicipio>();

            services.AddTransient<TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll, TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll>();

            services.AddSingleton<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>(c =>
            {
                var ret = new PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis();
                //para nao dar erro...
                ret.LimitePrepedidos.LimitePrepedidosExatamenteIguais_Numero = 1000;
                return ret;
            });
        }

        protected override IHostBuilder CreateHostBuilder(AssemblyName assemblyName) =>
            base.CreateHostBuilder(assemblyName)
                .ConfigureServices(ConfigureServices);
    }
}

