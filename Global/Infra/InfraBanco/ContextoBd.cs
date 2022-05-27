using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraBanco
{
    public class ContextoBd : IDisposable
    {
        /*
         * scripts em K:\desenvolvimento\Ar Clube\banco de dados - atualizacoes
         * scripts já executados:
         * 0192-atualiza-PagtoAntecipado.sql
         * 
         * scripts em andamento:
         * todo: coloca os sequingttes scripts, somente para a loja:
         * 0187-atualiza-Ajustes-API-Magento2.sql
         * 0190-atualiza-t_PEDIDO_ITEM_SERVICO.sql
         * 0196-atualiza-API-Magento.sql (já foi executado no banco ARCLUBE_DIS20201204)
         * 0197-atualiza-FinSvc.sql
         * */

        private readonly ContextoBdBasico contexto;
        internal ContextoBd(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            //sem nenhum rastreamento de mudanças na conexao (a rigor, com isto não precisamos dos AsNoTracking)
            contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        //necessário apra poder mandar comandos SQL diretamente
        public ContextoBdBasico GetContextoBdBasicoParaSql()
        {
            return contexto;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
#endif

        public IQueryable<Tcliente> Tclientes { get => contexto.Tclientes.AsNoTracking(); }
        public IQueryable<Torcamento> Torcamentos { get => contexto.Torcamentos.AsNoTracking(); }

        public IQueryable<TorcamentoCotacaoLink> TorcamentoCotacaoLinks { get => contexto.TorcamentoCotacaoLinks.AsNoTracking(); }
        public IQueryable<TcfgUnidadeNegocioParametro> TcfgUnidadeNegocioParametro { get => contexto.TcfgUnidadeNegocioParametro.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagens { get => contexto.TorcamentoCotacaoMensagens.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcao> torcamentoCotacaoOpcaos { get => contexto.TorcamentoCotacaoOpcao.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoEmailQueue> torcamentoCotacaoEmailQueue { get => contexto.TorcamentoCotacaoEmailQueue.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPgtos { get => contexto.TorcamentoCotacaoOpcaoPagtos.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get => contexto.TorcamentoCotacaoItemUnificados.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFins { get => contexto.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomicos { get => contexto.TorcamentoCotacaoOpcaoItemAtomicos.AsNoTracking(); }
        public IQueryable<TclienteRefBancaria> TclienteRefBancarias { get => contexto.TclienteRefBancarias.AsNoTracking(); }
        public IQueryable<Tpedido> Tpedidos { get => contexto.Tpedidos.AsNoTracking(); }
        public IQueryable<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors.AsNoTracking(); }
        public IQueryable<TorcamentistaEIndicadorVendedor> TorcamentistaEindicadorVendedors { get => contexto.TorcamentistaEindicadorVendedors.AsNoTracking(); }
        public IQueryable<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos.AsNoTracking(); }
        public IQueryable<Tproduto> Tprodutos { get => contexto.Tprodutos.AsNoTracking(); }
        public IQueryable<TprodutoCatalogo> TprodutoCatalogos { get => contexto.TprodutoCatalogos.AsNoTracking(); }
        public IQueryable<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedades { get => contexto.TProdutoCatalogoPropriedades.AsNoTracking(); }
        public IQueryable<TprodutoCatalogoItem> TprodutoCatalogoItems { get => contexto.TprodutoCatalogoItems.AsNoTracking(); }
        public IQueryable<TProdutoCatalogoPropriedadeOpcao> TProdutoCatalogoPropriedadeOpcoes { get => contexto.TProdutoCatalogoPropriedadeOpcoes.AsNoTracking(); }
        public IQueryable<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas.AsNoTracking(); }
        public IQueryable<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvido> TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos.AsNoTracking(); }
        public IQueryable<TpedidoPerda> TpedidoPerdas { get => contexto.TpedidoPerdas.AsNoTracking(); }
        public IQueryable<TpedidoPagamento> TpedidoPagamentos { get => contexto.TpedidoPagamentos.AsNoTracking(); }
        public IQueryable<Ttransportadora> Ttransportadoras { get => contexto.Ttransportadoras.AsNoTracking(); }
        public IQueryable<TpedidoBlocosNotas> TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas.AsNoTracking(); }
        public IQueryable<TcodigoDescricao> TcodigoDescricaos { get => contexto.TcodigoDescricaos.AsNoTracking(); }
        public IQueryable<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagems { get => contexto.TpedidoOcorrenciaMensagems.AsNoTracking(); }
        public IQueryable<TpedidoOcorrencia> TpedidoOcorrencias { get => contexto.TpedidoOcorrencias.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas.AsNoTracking(); }
        public IQueryable<TorcamentoItem> TorcamentoItems { get => contexto.TorcamentoItems.AsNoTracking(); }
        public IQueryable<Tbanco> Tbancos { get => contexto.Tbancos.AsNoTracking(); }
        public IQueryable<TclienteRefComercial> TclienteRefComercials { get => contexto.TclienteRefComercials.AsNoTracking(); }
        public IQueryable<Tlog> Tlogs { get => contexto.Tlogs.AsNoTracking(); }
        public IQueryable<Tloja> Tlojas { get => contexto.Tlojas.AsNoTracking(); }
        public IQueryable<Tcontrole> Tcontroles { get => contexto.Tcontroles.AsNoTracking(); }
        public IQueryable<TnfEmitente> TnfEmitentes { get => contexto.TnfEmitentes.AsNoTracking(); }
        public IQueryable<TecProdutoComposto> TecProdutoCompostos { get => contexto.TecProdutoCompostos.AsNoTracking(); }
        public IQueryable<Tfabricante> Tfabricantes { get => contexto.Tfabricantes.AsNoTracking(); }
        public IQueryable<Tparametro> Tparametros { get => contexto.Tparametros.AsNoTracking(); }
        public IQueryable<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors.AsNoTracking(); }
        public IQueryable<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistoricos { get => contexto.TpercentualCustoFinanceiroFornecedorHistoricos.AsNoTracking(); }
        public IQueryable<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds.AsNoTracking(); }
        public IQueryable<TwmsRegraCd> TwmsRegraCds { get => contexto.TwmsRegraCds.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUf> TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds.AsNoTracking(); }
        public IQueryable<TecProdutoCompostoItem> TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems.AsNoTracking(); }
        public IQueryable<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems.AsNoTracking(); }
        public IQueryable<TprodutoXAlerta> TprodutoXAlertas { get => contexto.TprodutoXAlertas.AsNoTracking(); }
        public IQueryable<TalertaProduto> TalertaProdutos { get => contexto.TalertaProdutos.AsNoTracking(); }
        public IQueryable<TformaPagto> TformaPagtos { get => contexto.TformaPagtos.AsNoTracking(); }
        public IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> TorcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.TorcamentistaEIndicadorRestricaoFormaPagtos.AsNoTracking(); }
        public IQueryable<TprazoPagtoVisanet> TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets.AsNoTracking(); }
        public IQueryable<TprodutoSubgrupo> TprodutoSubgrupos { get => contexto.TprodutoSubgrupos.AsNoTracking(); }
        public IQueryable<Tusuario> Tusuarios { get => contexto.Tusuarios.AsNoTracking(); }
        public IQueryable<Tperfil> Tperfils { get => contexto.Tperfils.AsNoTracking(); }
        public IQueryable<TperfilUsuario> TperfilUsuarios { get => contexto.TperfilUsuarios.AsNoTracking(); }
        public IQueryable<Testoque> Testoques { get => contexto.Testoques.AsNoTracking(); }
        public IQueryable<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos.AsNoTracking(); }
        public IQueryable<TcfgModulo> TcfgModulos { get => contexto.TcfgModulos.AsNoTracking(); }
        public IQueryable<TcfgPagtoForma> TcfgPagtoFormas { get => contexto.TcfgPagtoFormas.AsNoTracking(); }
        public IQueryable<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get => contexto.TcfgPagtoFormaStatus.AsNoTracking(); }
        public IQueryable<TcfgPagtoMeio> TcfgPagtoMeios { get => contexto.TcfgPagtoMeios.AsNoTracking(); }
        public IQueryable<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get => contexto.TcfgPagtoMeioStatus.AsNoTracking(); }
        public IQueryable<TcfgTipoParcela> TcfgTipoParcelas { get => contexto.TcfgTipoParcelas.AsNoTracking(); }
        public IQueryable<TcfgTipoPessoa> TcfgTipoPessoas { get => contexto.TcfgTipoPessoas.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuario> TcfgTipoUsuarios { get => contexto.TcfgTipoUsuarios.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuarioPerfil> TcfgTipoUsuarioPerfis { get => contexto.TcfgTipoUsuarioPerfis.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuarioContexto> TcfgTipoUsuarioContextos { get => contexto.TcfgTipoUsuarioContextos.AsNoTracking(); }
        public IQueryable<TcfgOrcamentoCotacaoStatus> TcfgOrcamentoCotacaoStatus { get => contexto.TcfgOrcamentoCotacaoStatus.AsNoTracking(); }
        public IQueryable<TcfgOrcamentoCotacaoEmailTemplate> TcfgOrcamentoCotacaoEmailTemplates { get => contexto.TcfgOrcamentoCotacaoEmailTemplates.AsNoTracking(); }


#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        public IQueryable<Tdesconto> Tdescontos { get => contexto.Tdescontos.AsNoTracking(); }

        public IQueryable<TtransportadoraCep> TtransportadoraCeps { get => contexto.TtransportadoraCeps.AsNoTracking(); }

        public IQueryable<TusuarioXLoja> TusuarioXLojas { get => contexto.TusuarioXLojas.AsNoTracking(); }
        public IQueryable<Taviso> Tavisos { get => contexto.Tavisos.AsNoTracking(); }
        public IQueryable<TavisoExibido> TavisoExibidos { get => contexto.TavisoExibidos.AsNoTracking(); }
        public IQueryable<TcfgUnidadeNegocio> TcfgUnidadeNegocio { get => contexto.TcfgUnidadeNegocio.AsNoTracking(); }
        public IQueryable<TavisoLido> TavisoLidos { get => contexto.TavisoLidos.AsNoTracking(); }
        public IQueryable<Toperacao> Toperacaos { get => contexto.Toperacaos.AsNoTracking(); }
        public IQueryable<TperfilItem> TperfilItens { get => contexto.TperfilItens.AsNoTracking(); }
        public IQueryable<TestoqueLog> TestoqueLogs { get => contexto.TestoqueLogs.AsNoTracking(); }
        public IQueryable<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos.AsNoTracking(); }
        public IQueryable<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseEnderecoConfrontacaos { get => contexto.TpedidoAnaliseConfrontacaos.AsNoTracking(); }

#endif

    }
}
