using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfraBanco
{
    public class ContextoBdGravacao : IDisposable
    {
        private readonly ContextoBdBasico contexto;
        public ContextoBdGravacaoOpcoes ContextoBdGravacaoOpcoes { get; private set; }
        public readonly IDbContextTransaction transacao;
        internal ContextoBdGravacao(ContextoBdBasico contexto, ContextoBdGravacaoOpcoes contextoBdGravacaoOpcoes, BloqueioTControle bloqueioTControle)
        {
            this.contexto = contexto;
            this.ContextoBdGravacaoOpcoes = contextoBdGravacaoOpcoes;

            /*
                * 
                usar transação como READ COMMITED,  e nao como SERIALIZABLE.
                Motivação: queremos evitar "falsos" deadlocks e também diminuir o número de bloqueios desnecessários.
                Exemplo: ao fazer um pedido ele consulta muitos outros pedidos para verificar se possuem o mesmo endereço de entrega.
                Não queremos bloquear todos esses outros pedidos durante a criação deste pedido.
                E também não queremos que dê um deadlock se alguém tentar editar um desses pedidos durante a criação do outro pedido
                (seja na criação do pedido novo, seja na edição do pedido anterior) porque isso não é relevante para o negócio. Quer dizer,
                uma edição dessas não interessa ter um bloqueio. 
                Onde o bloqueio é IMPORTANTE:
                - movimentação de estoque
                - geração de NSU

                Nessas tabelas, vamos ter um flag que sempre atualizamos para bloquear outras leituras. 
                Sempre que formos atualizar algum desses registros, primeiros atualizamos o flag para forçar o bloqueio no registro.
            */
            transacao = RelationalDatabaseFacadeExtensions.BeginTransaction(contexto.Database, System.Data.IsolationLevel.ReadCommitted);
            BloquearTControle(bloqueioTControle);
        }

        //os bloqueios possíveis na t_CONTROLE
        public enum BloqueioTControle
        {
            NENHUM = 0, XLOCK_SYNC_PEDIDO = 1, XLOCK_SYNC_ORCAMENTO = 2, XLOCK_SYNC_CLIENTE = 3, XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR = 4
        }

        private void BloquearTControle(BloqueioTControle bloqueioTControle)
        {
            if (!ContextoBdGravacaoOpcoes.TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO)
                return;
            string id_nsu = "";
            switch (bloqueioTControle)
            {
                case BloqueioTControle.NENHUM:
                    return;
                case BloqueioTControle.XLOCK_SYNC_PEDIDO:
                    id_nsu = Constantes.Constantes.ID_XLOCK_SYNC_PEDIDO;
                    break;
                case BloqueioTControle.XLOCK_SYNC_ORCAMENTO:
                    id_nsu = Constantes.Constantes.ID_XLOCK_SYNC_ORCAMENTO;
                    break;
                case BloqueioTControle.XLOCK_SYNC_CLIENTE:
                    id_nsu = Constantes.Constantes.ID_XLOCK_SYNC_CLIENTE;
                    break;
                case BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR:
                    id_nsu = Constantes.Constantes.ID_XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR;
                    break;
                default:
                    return;
            }
            if (string.IsNullOrEmpty(id_nsu))
                throw new ArgumentException("Não foi especificado o NSU para bloqueio!");

            var queryControle = from c in this.Tcontrole
                                where c.Id_Nsu == id_nsu
                                select c;

            var controle = queryControle.FirstOrDefault();
            if (controle == null)
                throw new ArgumentException($"Não existe registro na tabela de controle para poder bloquear este NSU! Não existe t_controle.id_nsu = {id_nsu}");

            //alteramos o flag
            controle.Dummy = !controle.Dummy;
            //não pode usar Update(controle) porque isso faz com que o Entity altere todos os campos
            //somente queremos alterar o campo Dummy
            this.SaveChanges();
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    transacao.Dispose();
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

        //acesso a métodos
        public EntityEntry Remove(object entity) => contexto.Remove(entity);
        public EntityEntry Add(object entity) => contexto.Add(entity);
        public EntityEntry Update(object entity) => contexto.Update(entity);
        public async Task<int> SaveChangesAsync() { return await contexto.SaveChangesAsync(); }
        public int SaveChanges() => contexto.SaveChanges();

        //acesso às tabelas
        public DbSet<Tcliente> Tcliente { get => contexto.Tcliente; }
        public DbSet<Torcamento> Torcamento { get => contexto.Torcamento; }
        public DbSet<TorcamentoCotacao> TorcamentoCotacao { get => contexto.TorcamentoCotacao; }
        public DbSet<TorcamentoCotacaoLink> TorcamentoCotacaoLink { get => contexto.TorcamentoCotacaoLink; }
        public DbSet<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagem { get => contexto.TorcamentoCotacaoMensagem; }
        public DbSet<TorcamentoCotacaoMensagemStatus> TorcamentoCotacaoMensagemStatus { get => contexto.TorcamentoCotacaoMensagemStatus; }
        public DbSet<TorcamentoItem> TorcamentoItem { get => contexto.TorcamentoItem; }
        public DbSet<TsessaoHistorico> TsessaoHistorico { get => contexto.TsessaoHistorico; }
        public DbSet<Tcontrole> Tcontrole { get => contexto.Tcontrole; }
        public DbSet<TprodutoLoja> TprodutoLoja { get => contexto.TprodutoLoja; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicador { get => contexto.TorcamentistaEindicador; }
        public DbSet<TsessaoAbandonada> TsessaoAbandonada { get => contexto.TsessaoAbandonada; }
        public DbSet<Tusuario> Tusuario { get => contexto.Tusuario; }
        public DbSet<TecProdutoComposto> TecProdutoComposto { get => contexto.TecProdutoComposto; }
        public DbSet<TecProdutoCompostoItem> TecProdutoCompostoItem { get => contexto.TecProdutoCompostoItem; }
        public DbSet<TcfgOrcamentoCotacaoStatus> TcfgOrcamentoCotacaoStatus { get => contexto.TcfgOrcamentoCotacaoStatus; }
        public DbSet<TcfgOrcamentoCotacaoEmailTemplate> TcfgOrcamentoCotacaoEmailTemplate { get => contexto.TcfgOrcamentoCotacaoEmailTemplate; }
        public DbSet<TcfgUnidadeNegocioParametro> TcfgUnidadeNegocioParametro { get => contexto.TcfgUnidadeNegocioParametro; }
        public DbSet<TcfgUnidadeNegocio> TcfgUnidadeNegocio { get => contexto.TcfgUnidadeNegocio; }
        public DbSet<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistorico { get => contexto.TpercentualCustoFinanceiroFornecedorHistorico; }
        public DbSet<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get => contexto.TcfgPagtoMeioStatus; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFin { get => contexto.TorcamentoCotacaoOpcaoItemAtomicoCustoFin; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomico { get => contexto.TorcamentoCotacaoOpcaoItemAtomico; }
        public DbSet<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificado { get => contexto.TorcamentoCotacaoItemUnificado; }
        public DbSet<Tproduto> Tproduto { get => contexto.Tproduto; }
        public DbSet<TprodutoCatalogo> TprodutoCatalogo { get => contexto.TprodutoCatalogo; }
        public DbSet<TprodutoCatalogoImagem> TprodutoCatalogoImagem { get => contexto.TprodutoCatalogoImagem; }
        public DbSet<TprodutoCatalogoImagemTipo> TprodutoCatalogoImagemTipo { get => contexto.TprodutoCatalogoImagemTipo; }
        public DbSet<TprodutoCatalogoItem> TprodutoCatalogoItem { get => contexto.TprodutoCatalogoItem; }
        public DbSet<TprodutoCatalogoItens> TprodutoCatalogoItens { get => contexto.TprodutoCatalogoItens; }
        public DbSet<TorcamentistaEIndicadorVendedor> TorcamentistaEIndicadorVendedor { get => contexto.TorcamentistaEindicadorVendedor; }
        public DbSet<TorcamentistaEIndicadorRestricaoFormaPagto> TorcamentistaEIndicadorRestricaoFormaPagto { get => contexto.TorcamentistaEIndicadorRestricaoFormaPagto; }
        public DbSet<Tfabricante> Tfabricante { get => contexto.Tfabricante; }
        public DbSet<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedade { get => contexto.TProdutoCatalogoPropriedade; }
        public DbSet<TProdutoCatalogoPropriedadeOpcao> TProdutoCatalogoPropriedadeOpcao { get => contexto.TProdutoCatalogoPropriedadeOpcao; }
        public DbSet<TprazoPagtoVisanet> TprazoPagtoVisanet { get => contexto.TprazoPagtoVisanet; }
        public DbSet<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get => contexto.TcfgPagtoFormaStatus; }
        public DbSet<TorcamentoCotacaoEmailQueue> TorcamentoCotacaoEmailQueue { get => contexto.TorcamentoCotacaoEmailQueue; }
        public DbSet<TorcamentoCotacaoEmail> TorcamentoCotacaoEmail { get => contexto.TorcamentoCotacaoEmail; }
        public DbSet<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseConfrontacao { get => contexto.TpedidoAnaliseConfrontacao; }
        public DbSet<Tpedido> Tpedido { get => contexto.Tpedido; }
        public DbSet<TfinControle> TfinControle { get => contexto.TfinControle; }
        public DbSet<TpedidoAnaliseEndereco> TpedidoAnaliseEndereco { get => contexto.TpedidoAnaliseEndereco; }
        public DbSet<Tdesconto> Tdesconto { get => contexto.Tdesconto; }
        public DbSet<TnfEmitente> TnfEmitente { get => contexto.TnfEmitente; }
        public DbSet<TpedidoItem> TpedidoItem { get => contexto.TpedidoItem; }
        public DbSet<TpedidoItemDevolvido> TpedidoItemDevolvido { get => contexto.TpedidoItemDevolvido; }
        public DbSet<TpedidoDevolucao> TpedidoDevolucao { get => contexto.TpedidoDevolucao; }
        public DbSet<TestoqueItem> TestoqueItem { get => contexto.TestoqueItem; }
        public DbSet<TestoqueLog> TestoqueLog { get => contexto.TestoqueLog; }
        public DbSet<Testoque> Testoque { get => contexto.Testoque; }
        public DbSet<TorcamentoCotacaoArquivos> TorcamentoCotacaoArquivos { get => contexto.TorcamentoCotacaoArquivos; }
        public DbSet<Tloja> Tloja { get => contexto.Tloja; }
        public DbSet<TclienteRefComercial> TclienteRefComercial { get => contexto.TclienteRefComercial; }
        public DbSet<TclienteRefBancaria> TclienteRefBancaria { get => contexto.TclienteRefBancaria; }
        public DbSet<TestoqueMovimento> TestoqueMovimento { get => contexto.TestoqueMovimento; }



        public DbSet<TusuarioXLoja> TusuarioXLoja { get => contexto.TusuarioXLoja; }
        public DbSet<TorcamentoCotacaoOpcao> TorcamentoCotacaoOpcao { get => contexto.TorcamentoCotacaoOpcao; }
        public DbSet<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPagto { get => contexto.TorcamentoCotacaoOpcaoPagto; }
        public DbSet<TcfgModulo> TcfgModulo { get => contexto.TcfgModulo; }
        public DbSet<TcfgPagtoForma> TcfgPagtoForma { get => contexto.TcfgPagtoForma; }
        public DbSet<TcfgPagtoMeio> TcfgPagtoMeio { get => contexto.TcfgPagtoMeio; }
        public DbSet<TcfgTipoParcela> TcfgTipoParcela { get => contexto.TcfgTipoParcela; }
        public DbSet<TcfgTipoPessoa> TcfgTipoPessoa { get => contexto.TcfgTipoPessoa; }
        public DbSet<TcfgTipoUsuario> TcfgTipoUsuario { get => contexto.TcfgTipoUsuario; }
        public DbSet<TcfgTipoUsuarioPerfil> TcfgTipoUsuarioPerfil { get => contexto.TcfgTipoUsuarioPerfil; }
        public DbSet<TcfgTipoUsuarioContexto> TcfgTipoUsuarioContexto { get => contexto.TcfgTipoUsuarioContexto; }
        public DbSet<TcfgParametro> TcfgParametro { get => contexto.TcfgParametro; }
        public DbSet<TcfgDataType> TcfgDataType { get => contexto.TcfgDataType; }
        public DbSet<TcfgTipoPermissaoEdicaoCadastro> TcfgTipoPermissaoEdicaoCadastro { get => contexto.TcfgTipoPermissaoEdicaoCadastro; }
        public DbSet<TcfgTipoPropriedadeProdutoCatalogo> TcfgTipoPropriedadeProdutoCatalogo { get => contexto.TcfgTipoPropriedadeProdutoCatalogo; }

        public DbSet<Tbanco> Tbancos { get => contexto.Tbanco; }
        public DbSet<Tfabricante> Tfabricantes { get => contexto.Tfabricante; }
        public DbSet<Tproduto> Tprodutos { get => contexto.Tproduto; }
        public DbSet<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedor; }
        public DbSet<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistoricos { get => contexto.TpercentualCustoFinanceiroFornecedorHistorico; }
        public DbSet<Tparametro> Tparametros { get => contexto.Tparametro; }
        public DbSet<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCd; }
        public DbSet<TwmsRegraCd> TwmsRegraCd { get => contexto.TwmsRegraCd; }
        public DbSet<TwmsRegraCdXUf> TwmsRegraCdXUf { get => contexto.TwmsRegraCdXUf; }
        public DbSet<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoa { get => contexto.TwmsRegraCdXUfPessoa; }
        public DbSet<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCd { get => contexto.TwmsRegraCdXUfXPessoaXCd; }
        public DbSet<TformaPagto> TformaPagto { get => contexto.TformaPagto; }
        public DbSet<Tperfil> Tperfil { get => contexto.Tperfil; }
        public DbSet<TperfilItem> TperfilIten { get => contexto.TperfilIten; }
        public DbSet<TperfilUsuario> TperfilUsuario { get => contexto.TperfilUsuario; }
        public DbSet<TcodigoDescricao> TcodigoDescricao { get => contexto.TcodigoDescricao; }
        public DbSet<Toperacao> Toperacao { get => contexto.Toperacao; }
        public DbSet<Tlog> Tlog { get => contexto.Tlog; }
        public DbSet<TLogV2> TLogV2 { get => contexto.TlogV2; }
        public DbSet<TcfgOperacao> TcfgOperacao { get => contexto.TcfgOperacao; }
        public DbSet<TprodutoGrupo> TprodutoGrupo { get => contexto.TprodutoGrupo; }
    }
}
