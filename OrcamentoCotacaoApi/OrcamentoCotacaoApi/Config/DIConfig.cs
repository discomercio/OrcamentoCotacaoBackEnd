﻿using Cfg.CfgOrcamentoCotacaoEndpointFilter;
using InfraBanco;
using InfraBanco.Modelos;
using InfraIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoBusiness.Bll;
using UtilsGlobais;
using UtilsGlobais.Parametros;

namespace OrcamentoCotacaoApi.Config
{
    public static class DIConfig
    {
        public static IServiceCollection AddInjecaoDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            string conexaoRelatorio = Configuration.GetConnectionString("conexaoRelatorio");
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            var appSettings = Configuration.GetSection("AppSettings").Get<Configuracao>();
            services.AddTransient<ContextoBdProvider, ContextoBdProvider>();
            services.AddTransient<ContextoCepProvider, ContextoCepProvider>();
            services.AddTransient<ContextoRelatorioProvider, ContextoRelatorioProvider>();
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
            services.AddDbContext<ContextoRelatorioBd>(options =>
            {
                options.UseSqlServer(conexaoRelatorio);
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<ITokenService, TokenService>();

            //#GLOBAL
            services.AddTransient<TcfgOrcamentoCotacaoEndpointFilterBll, TcfgOrcamentoCotacaoEndpointFilterBll>();
            services.AddTransient<TcfgOrcamentoCotacaoEndpointFilterData, TcfgOrcamentoCotacaoEndpointFilterData>();
            services.AddTransient<ParametroOrcamentoCotacaoBll, ParametroOrcamentoCotacaoBll>();
            services.AddTransient<ParametroOrcamentoCotacaoData, ParametroOrcamentoCotacaoData>();
            services.AddTransient<PublicoBll, PublicoBll>();
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
            services.AddTransient<OrcamentoCotacaoMensagem.OrcamentoCotacaoMensagemBll, OrcamentoCotacaoMensagem.OrcamentoCotacaoMensagemBll>();
            services.AddTransient<OrcamentoCotacaoMensagem.OrcamentoCotacaoMensagemData, OrcamentoCotacaoMensagem.OrcamentoCotacaoMensagemData>();
            services.AddTransient<OrcamentoCotacaoMensagemStatus.OrcamentoCotacaoMensagemStatusBll, OrcamentoCotacaoMensagemStatus.OrcamentoCotacaoMensagemStatusBll>();
            services.AddTransient<OrcamentoCotacaoMensagemStatus.OrcamentoCotacaoMensagemStatusData, OrcamentoCotacaoMensagemStatus.OrcamentoCotacaoMensagemStatusData>();
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
            services.AddTransient<OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll, OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoBll>();
            services.AddTransient<OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoData, OrcamentoCotacaoOpcaoPagto.OrcamentoCotacaoOpcaoPagtoData>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll, OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll>();
            services.AddTransient<OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinData, OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinData>();
            services.AddTransient<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll>();
            services.AddTransient<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueData, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueData>();
            services.AddTransient<OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll, OrcamentoCotacaoEmail.OrcamentoCotacaoEmailBll>();
            services.AddTransient<OrcamentoCotacaoEmail.OrcamentoCotacaoEmailData, OrcamentoCotacaoEmail.OrcamentoCotacaoEmailData>();
            services.AddTransient<OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll, OrcamentoCotacaoLink.OrcamentoCotacaoLinkBll>();
            services.AddTransient<OrcamentoCotacaoLink.OrcamentoCotacaoLinkData, OrcamentoCotacaoLink.OrcamentoCotacaoLinkData>();
            services.AddTransient<Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll, Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll>();
            services.AddTransient<Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateData, Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateData>();
            services.AddTransient<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll, Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll>();
            services.AddTransient<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioData, Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioData>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<ProdutoCatalogo.ProdutoCatalogoBll, ProdutoCatalogo.ProdutoCatalogoBll>();
            services.AddTransient<ProdutoCatalogo.ProdutoCatalogoData, ProdutoCatalogo.ProdutoCatalogoData>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<Prepedido.Bll.PrepedidoBll, Prepedido.Bll.PrepedidoBll>();
            services.AddTransient<Prepedido.Bll.MontarLogPrepedidoBll, Prepedido.Bll.MontarLogPrepedidoBll>();
            services.AddTransient<Prepedido.Bll.ValidacoesPrepedidoBll, Prepedido.Bll.ValidacoesPrepedidoBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();
            services.AddTransient<Usuario.UsuarioBll, Usuario.UsuarioBll>();
            services.AddTransient<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll, OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll>();
            services.AddTransient<Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll, Cfg.CfgOrcamentoCotacaoEmailTemplate.CfgOrcamentoCotacaoEmailTemplateBll>();
            services.AddTransient<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll, Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll>();
            services.AddTransient<Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroBll, Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroBll>();
            services.AddTransient<Operacao.OperacaoBll, Operacao.OperacaoBll>();
            services.AddTransient<Operacao.OperacaoData, Operacao.OperacaoData>();
            services.AddTransient<Cfg.CfgOperacao.CfgOperacaoBll, Cfg.CfgOperacao.CfgOperacaoBll>();
            services.AddTransient<Cfg.CfgOperacao.CfgOperacaoData, Cfg.CfgOperacao.CfgOperacaoData>();
            services.AddTransient<Cfg.CfgParametro.CfgParametroBll, Cfg.CfgParametro.CfgParametroBll>();
            services.AddTransient<Cfg.CfgParametro.CfgParametroData, Cfg.CfgParametro.CfgParametroData>();
            services.AddTransient<LoginHistorico.LoginHistoricoBll, LoginHistorico.LoginHistoricoBll>();
            services.AddTransient<LoginHistorico.LoginHistoricoData, LoginHistorico.LoginHistoricoData>();
            services.AddTransient<Cfg.CfgModulo.CfgModuloBll, Cfg.CfgModulo.CfgModuloBll>();
            services.AddTransient<Cfg.CfgModulo.CfgModuloData, Cfg.CfgModulo.CfgModuloData>();

            //#PRE-PEDIDO
            services.AddTransient<Prepedido.Bll.AcessoBll, Prepedido.Bll.AcessoBll>(); 
            services.AddTransient<Prepedido.Bll.CepPrepedidoBll, Prepedido.Bll.CepPrepedidoBll>();
            services.AddTransient<Prepedido.Bll.ClientePrepedidoBll, Prepedido.Bll.ClientePrepedidoBll>();
            services.AddTransient<Prepedido.Bll.CoeficientePrepedidoBll, Prepedido.Bll.CoeficientePrepedidoBll>();
            services.AddTransient<Prepedido.Bll.FormaPagtoPrepedidoBll, Prepedido.Bll.FormaPagtoPrepedidoBll>();
            services.AddTransient<PedidoPrepedidoApiBll, PedidoPrepedidoApiBll>();
            services.AddTransient<Prepedido.Bll.PrepedidoApiBll, Prepedido.Bll.PrepedidoApiBll>();
            services.AddTransient<Prepedido.Bll.ProdutoPrepedidoBll, Prepedido.Bll.ProdutoPrepedidoBll>();
            

            //#ORCAMENTO-COTACAO
            services.AddTransient<AcessoBll, AcessoBll>();
            services.AddTransient<CoeficienteBll, CoeficienteBll>();
            services.AddTransient<FormaPagtoOrcamentoCotacaoBll, FormaPagtoOrcamentoCotacaoBll>();
            services.AddTransient<LojaOrcamentoCotacaoBll, LojaOrcamentoCotacaoBll>();
            services.AddTransient<MensagemOrcamentoCotacaoBll, MensagemOrcamentoCotacaoBll>();
            services.AddTransient<OrcamentoCotacaoBll, OrcamentoCotacaoBll>();
            services.AddTransient<OrcamentistaEIndicadorVendedorBll, OrcamentistaEIndicadorVendedorBll>();
            services.AddTransient<OrcamentistaEIndicadorBll, OrcamentistaEIndicadorBll>();
            services.AddTransient<TorcamentoCotacaoLink, TorcamentoCotacaoLink>();
            services.AddTransient<OrcamentoCotacaoOpcaoBll, OrcamentoCotacaoOpcaoBll>();
            services.AddTransient<ProdutoOrcamentoCotacaoBll, ProdutoOrcamentoCotacaoBll>();
            services.AddTransient<ProdutoCatalogoOrcamentoCotacaoBll, ProdutoCatalogoOrcamentoCotacaoBll>();
            services.AddTransient<PermissaoBll, PermissaoBll>();
            services.AddTransient<ClienteBll, ClienteBll>();
            services.AddTransient<UsuarioBll, UsuarioBll>();

            services.AddTransient<Operacao.OperacaoBll, Operacao.OperacaoBll>();
            services.AddTransient<Operacao.OperacaoData, Operacao.OperacaoData>();

            services.AddTransient<Cfg.CfgOperacao.CfgOperacaoBll, Cfg.CfgOperacao.CfgOperacaoBll>();
            services.AddTransient<Cfg.CfgOperacao.CfgOperacaoData, Cfg.CfgOperacao.CfgOperacaoData>();

            services.AddTransient<Cfg.CfgParametro.CfgParametroBll, Cfg.CfgParametro.CfgParametroBll>();
            services.AddTransient<Cfg.CfgParametro.CfgParametroData, Cfg.CfgParametro.CfgParametroData>();

            services.AddTransient<CodigoDescricao.CodigoDescricaoBll, CodigoDescricao.CodigoDescricaoBll>();
            services.AddTransient<CodigoDescricao.CodigoDescricaoData, CodigoDescricao.CodigoDescricaoData>();
            services.AddTransient<CodigoDescricaoBll, CodigoDescricaoBll>();
            services.AddTransient<LoginHistoricoBll, LoginHistoricoBll>();

            services.AddTransient<Relatorios.RelatoriosBll, Relatorios.RelatoriosBll>();
            services.AddTransient<Relatorios.RelatoriosData, Relatorios.RelatoriosData>();
            services.AddTransient<RelatoriosBll, RelatoriosBll>();

            return services;
        }
    }
}
