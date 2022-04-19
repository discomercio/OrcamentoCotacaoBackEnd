using InfraBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Interfaces;
using OrcamentoCotacaoBusiness.Services;

namespace OrcamentoCotacaoApi.Config
{
    public static class DIConfig
    {
        public static IServiceCollection AddInjecaoDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            var appSettings = Configuration.GetSection("AppSettings").Get<Configuracao>();
            services.AddTransient<ContextoBdProvider, ContextoBdProvider>();
            services.AddTransient<ContextoCepProvider, ContextoCepProvider>();
            services.AddDbContext<ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
                options.EnableSensitiveDataLogging();
            });
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

            //#GLOBAL
            services.AddTransient<Arquivo.ArquivoBll, Arquivo.ArquivoBll>();

            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<Cep.IBancoNFeMunicipio, Cep.BancoNFeMunicipio>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<Coeficiente.CoeficienteBll, Coeficiente.CoeficienteBll>();
            services.AddTransient<Coeficiente.CoeficienteData, Coeficiente.CoeficienteData>();

            services.AddTransient<FormaPagamento.FormaPagtoBll, FormaPagamento.FormaPagtoBll>();
            services.AddTransient<FormaPagamento.ValidacoesFormaPagtoBll, FormaPagamento.ValidacoesFormaPagtoBll>();
            services.AddTransient<FormaPagamento.FormaPagamentoData, FormaPagamento.FormaPagamentoData>();

            services.AddTransient<Loja.LojaBll, Loja.LojaBll>();
            services.AddTransient<Loja.LojaData, Loja.LojaData>();

            services.AddTransient<MeioPagamentos.MeiosPagamentosBll, MeioPagamentos.MeiosPagamentosBll>();
            services.AddTransient<MeioPagamentos.MeiosPagamentosData, MeioPagamentos.MeiosPagamentosData>();

            services.AddTransient<Orcamento.OrcamentoBll, Orcamento.OrcamentoBll>();
            services.AddTransient<Orcamento.OrcamentoOpcaoBll, Orcamento.OrcamentoOpcaoBll>();
            services.AddTransient<OrcamentistaEindicador.OrcamentistaEIndicadorBll, OrcamentistaEindicador.OrcamentistaEIndicadorBll>();
            services.AddTransient<OrcamentistaEindicador.OrcamentistaEIndicadorData, OrcamentistaEindicador.OrcamentistaEIndicadorData>();
            services.AddTransient<OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll, OrcamentistaEIndicadorVendedor.OrcamentistaEIndicadorVendedorBll>();
            services.AddTransient<OrcamentoCotacao.OrcamentoCotacaoBll, OrcamentoCotacao.OrcamentoCotacaoBll>();
            services.AddTransient<OrcamentoCotacao.OrcamentoCotacaoData, OrcamentoCotacao.OrcamentoCotacaoData>();
            services.AddTransient<OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll, OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoBll>();
            services.AddTransient<OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoData, OrcamentoCotacaoOpcao.OrcamentoCotacaoOpcaoData>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll, OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoData, OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoData>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll, OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoData, OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoData>();

            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<ProdutoCatalogo.ProdutoCatalogoBll, ProdutoCatalogo.ProdutoCatalogoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<Prepedido.PrepedidoBll, Prepedido.PrepedidoBll>();
            services.AddTransient<Prepedido.MontarLogPrepedidoBll, Prepedido.MontarLogPrepedidoBll>();
            services.AddTransient<Prepedido.ValidacoesPrepedidoBll, Prepedido.ValidacoesPrepedidoBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();

            services.AddTransient<Usuario.UsuarioBll, Usuario.UsuarioBll>();
            


            //#PRE-PEDIDO
            services.AddTransient<PrepedidoBusiness.Bll.AcessoBll, PrepedidoBusiness.Bll.AcessoBll>(); 

            services.AddTransient<PrepedidoBusiness.Bll.CepPrepedidoBll, PrepedidoBusiness.Bll.CepPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ClientePrepedidoBll, PrepedidoBusiness.Bll.ClientePrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficientePrepedidoBll, PrepedidoBusiness.Bll.CoeficientePrepedidoBll>();

            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll, PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll>();

            services.AddTransient<PrepedidoBusiness.Bll.PedidoPrepedidoApiBll, PrepedidoBusiness.Bll.PedidoPrepedidoApiBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoApiBll, PrepedidoBusiness.Bll.PrepedidoApiBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            

            //#ORCAMENTO-COTACAO
            services.AddTransient<AcessoBll, AcessoBll>();

            services.AddTransient<CoeficienteBll, CoeficienteBll>();

            services.AddTransient<FormaPagtoOrcamentoCotacaoBll, FormaPagtoOrcamentoCotacaoBll>();

            services.AddTransient<LojaOrcamentoCotacaoBll, LojaOrcamentoCotacaoBll>();

            services.AddTransient<OrcamentoCotacaoBll, OrcamentoCotacaoBll>();
            services.AddTransient<OrcamentistaEIndicadorVendedorBll, OrcamentistaEIndicadorVendedorBll>();
            services.AddTransient<OrcamentistaEIndicadorBll, OrcamentistaEIndicadorBll>();
            services.AddTransient<OrcamentoCotacaoOpcaoBll, OrcamentoCotacaoOpcaoBll>();

            services.AddTransient<ProdutoOrcamentoCotacaoBll, ProdutoOrcamentoCotacaoBll>();
            services.AddTransient<ProdutoCatalogoOrcamentoCotacaoBll, ProdutoCatalogoOrcamentoCotacaoBll>();


            return services;
        }
    }
}
