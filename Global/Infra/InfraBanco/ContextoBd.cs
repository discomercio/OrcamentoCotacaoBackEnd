using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraBanco
{
    public class ContextoBd : IDisposable
    {
        private readonly ContextoBdBasico contexto;
        internal ContextoBd(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            //sem nenhum rastreamento de mudanças na conexao (a rigor, com isto não precisamos dos AsNoTracking)
            contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    contexto.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        //necessário apra poder mandar comandos SQL diretamente
        public ContextoBdBasico GetContextoBdBasicoParaSql()
        {
            return contexto;
        }

        /*SEMPRE NO SINGULAR*/
        /*SEMPRE NO SINGULAR*/
        /*SEMPRE NO SINGULAR*/
        public IQueryable<Tcliente> Tcliente { get => contexto.Tcliente.AsNoTracking(); }
        public IQueryable<Torcamento> Torcamento { get => contexto.Torcamento.AsNoTracking(); }
        public IQueryable<TorcamentoCotacao> TorcamentoCotacao { get => contexto.TorcamentoCotacao.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoLink> TorcamentoCotacaoLink { get => contexto.TorcamentoCotacaoLink.AsNoTracking(); }
        public IQueryable<TcfgUnidadeNegocioParametro> TcfgUnidadeNegocioParametro { get => contexto.TcfgUnidadeNegocioParametro.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagem { get => contexto.TorcamentoCotacaoMensagem.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoMensagemStatus> TorcamentoCotacaoMensagemStatus { get => contexto.TorcamentoCotacaoMensagemStatus.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcao> torcamentoCotacaoOpcao { get => contexto.TorcamentoCotacaoOpcao.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoEmailQueue> torcamentoCotacaoEmailQueue { get => contexto.TorcamentoCotacaoEmailQueue.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoEmail> torcamentoCotacaoEmail { get => contexto.TorcamentoCotacaoEmail.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPgto { get => contexto.TorcamentoCotacaoOpcaoPagto.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificado { get => contexto.TorcamentoCotacaoItemUnificado.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFin { get => contexto.TorcamentoCotacaoOpcaoItemAtomicoCustoFin.AsNoTracking(); }
        public IQueryable<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomico { get => contexto.TorcamentoCotacaoOpcaoItemAtomico.AsNoTracking(); }
        public IQueryable<TclienteRefBancaria> TclienteRefBancaria { get => contexto.TclienteRefBancaria.AsNoTracking(); }
        public IQueryable<Tpedido> Tpedido { get => contexto.Tpedido.AsNoTracking(); }
        public IQueryable<TorcamentistaEindicador> TorcamentistaEindicador { get => contexto.TorcamentistaEindicador.AsNoTracking(); }
        public IQueryable<TorcamentistaEIndicadorVendedor> TorcamentistaEindicadorVendedor { get => contexto.TorcamentistaEindicadorVendedor.AsNoTracking(); }
        public IQueryable<TsessaoHistorico> TsessaoHistorico { get => contexto.TsessaoHistorico.AsNoTracking(); }
        public IQueryable<Tproduto> Tproduto { get => contexto.Tproduto.AsNoTracking(); }
        public IQueryable<TprodutoCatalogo> TprodutoCatalogo { get => contexto.TprodutoCatalogo.AsNoTracking(); }
        public IQueryable<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedade { get => contexto.TProdutoCatalogoPropriedade.AsNoTracking(); }
        public IQueryable<TprodutoCatalogoItem> TprodutoCatalogoItem { get => contexto.TprodutoCatalogoItem.AsNoTracking(); }
        public IQueryable<TProdutoCatalogoPropriedadeOpcao> TProdutoCatalogoPropriedadeOpcao { get => contexto.TProdutoCatalogoPropriedadeOpcao.AsNoTracking(); }
        public IQueryable<TprodutoLoja> TprodutoLoja { get => contexto.TprodutoLoja.AsNoTracking(); }
        public IQueryable<TpedidoItem> TpedidoItem { get => contexto.TpedidoItem.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvido> TpedidoItemDevolvido { get => contexto.TpedidoItemDevolvido.AsNoTracking(); }
        public IQueryable<TpedidoPerda> TpedidoPerda { get => contexto.TpedidoPerda.AsNoTracking(); }
        public IQueryable<TpedidoPagamento> TpedidoPagamento { get => contexto.TpedidoPagamento.AsNoTracking(); }
        public IQueryable<Ttransportadora> Ttransportadora { get => contexto.Ttransportadora.AsNoTracking(); }
        public IQueryable<TpedidoBlocosNotas> TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas.AsNoTracking(); }
        public IQueryable<TcodigoDescricao> TcodigoDescricao { get => contexto.TcodigoDescricao.AsNoTracking(); }
        public IQueryable<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagem { get => contexto.TpedidoOcorrenciaMensagem.AsNoTracking(); }
        public IQueryable<TpedidoOcorrencia> TpedidoOcorrencia { get => contexto.TpedidoOcorrencia.AsNoTracking(); }
        public IQueryable<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas.AsNoTracking(); }
        public IQueryable<TorcamentoItem> TorcamentoItem { get => contexto.TorcamentoItem.AsNoTracking(); }
        public IQueryable<Tbanco> Tbanco { get => contexto.Tbanco.AsNoTracking(); }
        public IQueryable<TclienteRefComercial> TclienteRefComercial { get => contexto.TclienteRefComercial.AsNoTracking(); }
        public IQueryable<Tlog> Tlog { get => contexto.Tlog.AsNoTracking(); }
        public IQueryable<Tloja> Tloja { get => contexto.Tloja.AsNoTracking(); }
        public IQueryable<Tcontrole> Tcontrole { get => contexto.Tcontrole.AsNoTracking(); }
        public IQueryable<TnfEmitente> TnfEmitente { get => contexto.TnfEmitente.AsNoTracking(); }
        public IQueryable<TecProdutoComposto> TecProdutoComposto { get => contexto.TecProdutoComposto.AsNoTracking(); }
        public IQueryable<Tfabricante> Tfabricante { get => contexto.Tfabricante.AsNoTracking(); }
        public IQueryable<Tparametro> Tparametro { get => contexto.Tparametro.AsNoTracking(); }
        public IQueryable<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedor { get => contexto.TpercentualCustoFinanceiroFornecedor.AsNoTracking(); }
        public IQueryable<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistorico { get => contexto.TpercentualCustoFinanceiroFornecedorHistorico.AsNoTracking(); }
        public IQueryable<TprodutoXwmsRegraCd> TprodutoXwmsRegraCd { get => contexto.TprodutoXwmsRegraCd.AsNoTracking(); }
        public IQueryable<TwmsRegraCd> TwmsRegraCd { get => contexto.TwmsRegraCd.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUf> TwmsRegraCdXUf { get => contexto.TwmsRegraCdXUf.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoa { get => contexto.TwmsRegraCdXUfPessoa.AsNoTracking(); }
        public IQueryable<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCd { get => contexto.TwmsRegraCdXUfXPessoaXCd.AsNoTracking(); }
        public IQueryable<TecProdutoCompostoItem> TecProdutoCompostoItem { get => contexto.TecProdutoCompostoItem.AsNoTracking(); }
        public IQueryable<TestoqueItem> TestoqueItem { get => contexto.TestoqueItem.AsNoTracking(); }
        public IQueryable<TprodutoXAlerta> TprodutoXAlerta { get => contexto.TprodutoXAlerta.AsNoTracking(); }
        public IQueryable<TalertaProduto> TalertaProduto { get => contexto.TalertaProduto.AsNoTracking(); }
        public IQueryable<TformaPagto> TformaPagto { get => contexto.TformaPagto.AsNoTracking(); }
        public IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> TorcamentistaEIndicadorRestricaoFormaPagto { get => contexto.TorcamentistaEIndicadorRestricaoFormaPagto.AsNoTracking(); }
        public IQueryable<TprazoPagtoVisanet> TprazoPagtoVisanet { get => contexto.TprazoPagtoVisanet.AsNoTracking(); }
        public IQueryable<TprodutoSubgrupo> TprodutoSubgrupo { get => contexto.TprodutoSubgrupo.AsNoTracking(); }
        public IQueryable<Tusuario> Tusuario { get => contexto.Tusuario.AsNoTracking(); }
        public IQueryable<Tperfil> Tperfil { get => contexto.Tperfil.AsNoTracking(); }
        public IQueryable<TperfilUsuario> TperfilUsuario { get => contexto.TperfilUsuario.AsNoTracking(); }
        public IQueryable<Testoque> Testoque { get => contexto.Testoque.AsNoTracking(); }
        public IQueryable<TestoqueMovimento> TestoqueMovimento { get => contexto.TestoqueMovimento.AsNoTracking(); }
        public IQueryable<TcfgModulo> TcfgModulo { get => contexto.TcfgModulo.AsNoTracking(); }
        public IQueryable<TcfgPagtoForma> TcfgPagtoForma { get => contexto.TcfgPagtoForma.AsNoTracking(); }
        public IQueryable<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get => contexto.TcfgPagtoFormaStatus.AsNoTracking(); }
        public IQueryable<TcfgPagtoMeio> TcfgPagtoMeio { get => contexto.TcfgPagtoMeio.AsNoTracking(); }
        public IQueryable<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get => contexto.TcfgPagtoMeioStatus.AsNoTracking(); }
        public IQueryable<TcfgTipoParcela> TcfgTipoParcela { get => contexto.TcfgTipoParcela.AsNoTracking(); }
        public IQueryable<TcfgTipoPessoa> TcfgTipoPessoa { get => contexto.TcfgTipoPessoa.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuario> TcfgTipoUsuario { get => contexto.TcfgTipoUsuario.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuarioPerfil> TcfgTipoUsuarioPerfil { get => contexto.TcfgTipoUsuarioPerfil.AsNoTracking(); }
        public IQueryable<TcfgTipoUsuarioContexto> TcfgTipoUsuarioContexto { get => contexto.TcfgTipoUsuarioContexto.AsNoTracking(); }
        public IQueryable<TcfgOrcamentoCotacaoStatus> TcfgOrcamentoCotacaoStatus { get => contexto.TcfgOrcamentoCotacaoStatus.AsNoTracking(); }
        public IQueryable<TcfgOrcamentoCotacaoEmailTemplate> TcfgOrcamentoCotacaoEmailTemplate { get => contexto.TcfgOrcamentoCotacaoEmailTemplate.AsNoTracking(); }
        public IQueryable<TusuarioXLoja> TusuarioXLoja { get => contexto.TusuarioXLoja.AsNoTracking(); }
        public IQueryable<Toperacao> Toperacao { get => contexto.Toperacao.AsNoTracking(); }
        public IQueryable<TperfilItem> TperfilItem { get => contexto.TperfilItem.AsNoTracking(); }
        public IQueryable<TtransportadoraCep> TtransportadoraCep { get => contexto.TtransportadoraCep.AsNoTracking(); }


        public IQueryable<Tdesconto> Tdesconto { get => contexto.Tdesconto.AsNoTracking(); }
        public IQueryable<Taviso> Tavisos { get => contexto.Taviso.AsNoTracking(); }
        public IQueryable<TavisoExibido> TavisoExibidos { get => contexto.TavisoExibidos.AsNoTracking(); }
        public IQueryable<TcfgUnidadeNegocio> TcfgUnidadeNegocio { get => contexto.TcfgUnidadeNegocio.AsNoTracking(); }
        public IQueryable<TavisoLido> TavisoLidos { get => contexto.TavisoLido.AsNoTracking(); }
        public IQueryable<Toperacao> Toperacaos { get => contexto.Toperacao.AsNoTracking(); }
        public IQueryable<TperfilItem> TperfilItens { get => contexto.TperfilIten.AsNoTracking(); }
        public IQueryable<TestoqueLog> TestoqueLogs { get => contexto.TestoqueLog.AsNoTracking(); }
        public IQueryable<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEndereco.AsNoTracking(); }
        public IQueryable<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseEnderecoConfrontacao { get => contexto.TpedidoAnaliseConfrontacao.AsNoTracking(); }
        public IQueryable<TcfgTipoPermissaoEdicaoCadastro> TcfgTipoPermissaoEdicaoCadastro { get => contexto.TcfgTipoPermissaoEdicaoCadastro.AsNoTracking().AsNoTracking(); }
        public IQueryable<TcfgTipoPropriedadeProdutoCatalogo> TcfgTipoPropriedadeProdutoCatalogo { get => contexto.TcfgTipoPropriedadeProdutoCatalogo.AsNoTracking(); }
    }
}
