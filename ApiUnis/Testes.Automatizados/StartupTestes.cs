using FormaPagamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                options.UseInMemoryDatabase("bancomemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseInMemoryDatabase("bancocepmemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddSingleton<InfraBanco.ContextoBdGravacaoOpcoes>(c =>
            {
                return new InfraBanco.ContextoBdGravacaoOpcoes(true);
            });
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<InicializarBanco.InicializarBancoCep, InicializarBanco.InicializarBancoCep>();
            services.AddTransient<InicializarBanco.InicializarBancoGeral, InicializarBanco.InicializarBancoGeral>();

            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<Prepedido.Bll.ClientePrepedidoBll, Prepedido.Bll.ClientePrepedidoBll>();
            services.AddTransient<Prepedido.Bll.CepPrepedidoBll, Prepedido.Bll.CepPrepedidoBll>();
            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll, PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll>();
            services.AddTransient<PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll, PrepedidoUnisBusiness.UnisBll.CepUnisBll.CepUnisBll>();
            services.AddTransient<PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll, PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll>();
            services.AddTransient<Prepedido.Bll.PrepedidoBll, Prepedido.Bll.PrepedidoBll>();
            services.AddTransient<Prepedido.Bll.PrepedidoApiBll, Prepedido.Bll.PrepedidoApiBll>();
            services.AddTransient<Prepedido.Bll.ValidacoesPrepedidoBll, Prepedido.Bll.ValidacoesPrepedidoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<Prepedido.Bll.CoeficientePrepedidoBll, Prepedido.Bll.CoeficientePrepedidoBll>();
            services.AddTransient<ValidacoesFormaPagtoBll, ValidacoesFormaPagtoBll>();
            services.AddTransient<Prepedido.Bll.MontarLogPrepedidoBll, Prepedido.Bll.MontarLogPrepedidoBll>();
            services.AddTransient<FormaPagtoBll, FormaPagtoBll>();
            services.AddTransient<Prepedido.Bll.FormaPagtoPrepedidoBll, Prepedido.Bll.FormaPagtoPrepedidoBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();

            services.AddTransient<Cep.IBancoNFeMunicipio, Utils.TestesBancoNFeMunicipio>();

            services.AddTransient<TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll, TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll>();

            services.AddSingleton<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>(c =>
            {
                var ret = new PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis();
                //para nao dar erro...
                ret.LimitePrepedidos.LimitePrepedidosExatamenteIguais_Numero = 1000;
                ret.LimitePrepedidos.LimitePrepedidosMesmoCpfCnpj_Numero = 1000;
                return ret;
            });
        }

        protected override IHostBuilder CreateHostBuilder(AssemblyName assemblyName) =>
            base.CreateHostBuilder(assemblyName)
                .ConfigureServices(ConfigureServices);
    }
}

