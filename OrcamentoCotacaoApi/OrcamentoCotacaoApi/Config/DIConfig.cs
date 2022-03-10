using FormaPagamento;
using InfraBanco;
using MeioPagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrcamentistaEindicador;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Interfaces;
using OrcamentoCotacaoBusiness.Services;
using Produto;
using ProdutoCatalogo;
using Usuario;

namespace OrcamentoCotacaoApi.Config
{
    public static class DIConfig
    {
        public static IServiceCollection AddInjecaoDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            var appSettings = Configuration.GetSection("AppSettings").Get<Configuracao>();
            services.AddTransient<ContextoBdProvider, ContextoBdProvider>();
            services.AddSingleton<ContextoBdGravacaoOpcoes>(c =>
            {
                return new ContextoBdGravacaoOpcoes(appSettings.TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO);
            });
            services.AddDbContext<ContextoBdBasico>(options =>
            {
                options.UseSqlServer(conexaoBasica);
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<AcessoBll, AcessoBll>();
            services.AddTransient<ProdutoGeralBll, ProdutoGeralBll>();
            services.AddTransient<OrcamentistaEIndicadorBll, OrcamentistaEIndicadorBll>();
            services.AddTransient<OrcamentistaEIndicadorData, OrcamentistaEIndicadorData>();
            services.AddTransient<OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll, OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll>();
            services.AddTransient<Orcamento.OrcamentoBll, Orcamento.OrcamentoBll>();
            services.AddTransient<Orcamento.OrcamentoOpcaoBll, Orcamento.OrcamentoOpcaoBll>();
            services.AddTransient<Arquivo.ArquivoBll, Arquivo.ArquivoBll>();
            services.AddTransient<Loja.LojaBll, Loja.LojaBll>();
            services.AddTransient<ProdutoOrcamentoCotacaoBll, ProdutoOrcamentoCotacaoBll>();
            services.AddTransient<Loja.LojaData, Loja.LojaData>();
            services.AddTransient<FormaPagtoBll, FormaPagtoBll>();
            services.AddTransient<FormaPagtoOrcamentoCotacaoBll, FormaPagtoOrcamentoCotacaoBll>();
            services.AddTransient<FormaPagamentoData, FormaPagamentoData>();
            services.AddTransient<MeiosPagamentosBll, MeiosPagamentosBll>();
            services.AddTransient<MeiosPagamentosData, MeiosPagamentosData>();
            services.AddTransient<UsuarioBll, UsuarioBll>();
            services.AddTransient<ProdutoCatalogoBll, ProdutoCatalogoBll>();
            services.AddTransient<CoeficienteBll, CoeficienteBll>();
            services.AddTransient<LojaOrcamentoCotacaoBll, LojaOrcamentoCotacaoBll>();
            services.AddTransient<OrcamentoCotacaoBll, OrcamentoCotacaoBll>();

            return services;
        }
    }
}


//bll

//services.AddTransient<PrepedidoBusiness.Bll.PrepedidoApiBll, PrepedidoBusiness.Bll.PrepedidoApiBll>();
//services.AddTransient<Prepedido.PrepedidoBll, Prepedido.PrepedidoBll>();
//services.AddTransient<PrepedidoBusiness.Bll.ClientePrepedidoBll, PrepedidoBusiness.Bll.ClientePrepedidoBll>();
//services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();
//services.AddTransient<PrepedidoBusiness.Bll.PedidoPrepedidoApiBll, PrepedidoBusiness.Bll.PedidoPrepedidoApiBll>();
//services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
//services.AddTransient<Cep.CepBll, Cep.CepBll>();
//services.AddTransient<PrepedidoBusiness.Bll.CepPrepedidoBll, PrepedidoBusiness.Bll.CepPrepedidoBll>();
//services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll, PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll>();
//services.AddTransient<Prepedido.FormaPagto.ValidacoesFormaPagtoBll, Prepedido.FormaPagto.ValidacoesFormaPagtoBll>();
//services.AddTransient<PrepedidoBusiness.Bll.CoeficientePrepedidoBll, PrepedidoBusiness.Bll.CoeficientePrepedidoBll>();
//services.AddTransient<Prepedido.ValidacoesPrepedidoBll, Prepedido.ValidacoesPrepedidoBll>();
//services.AddTransient<Prepedido.MontarLogPrepedidoBll, Prepedido.MontarLogPrepedidoBll>();
//services.AddTransient<Cep.IBancoNFeMunicipio, Cep.BancoNFeMunicipio>();

