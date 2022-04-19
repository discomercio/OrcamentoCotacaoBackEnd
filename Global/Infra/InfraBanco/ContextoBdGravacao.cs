using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var queryControle = from c in this.Tcontroles
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

        #region IDisposable Support
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
        #endregion

        //acesso a métodos
        public EntityEntry Remove(object entity) => contexto.Remove(entity);
        public EntityEntry Add(object entity) => contexto.Add(entity);
        public EntityEntry Update(object entity) => contexto.Update(entity);
        public async Task<int> SaveChangesAsync() { return await contexto.SaveChangesAsync(); }
        public int SaveChanges() => contexto.SaveChanges();

        //acesso às tabelas
        public DbSet<Tcliente> Tclientes { get => contexto.Tclientes; }
        public DbSet<Torcamento> Torcamentos { get => contexto.Torcamentos; }
        public DbSet<TorcamentoCotacaoLink> TorcamentoCotacaoLinks { get => contexto.TorcamentoCotacaoLinks; }
        public DbSet<TorcamentoCotacaoMensagem> TorcamentoCotacaoMensagens { get => contexto.TorcamentoCotacaoMensagens; }
        public DbSet<TorcamentoItem> TorcamentoItems { get => contexto.TorcamentoItems; }
        public DbSet<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos; }
        public DbSet<Tcontrole> Tcontroles { get => contexto.Tcontroles; }
        public DbSet<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }
        public DbSet<TsessaoAbandonada> TsessaoAbandonadas { get => contexto.TsessaoAbandonadas; }
        public DbSet<Tusuario> Tusuarios { get => contexto.Tusuarios; }
        public DbSet<TecProdutoComposto> TecProdutoCompostos { get => contexto.TecProdutoCompostos; }
        public DbSet<TecProdutoCompostoItem> TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        public DbSet<TclienteRefBancaria> TclienteRefBancarias { get => contexto.TclienteRefBancarias; }
        public DbSet<TclienteRefComercial> TclienteRefComercials { get => contexto.TclienteRefComercials; }
        public DbSet<Tdesconto> Tdescontos { get => contexto.Tdescontos; }
        public DbSet<Testoque> Testoques { get => contexto.Testoques; }
        public DbSet<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems; }
        public DbSet<TestoqueLog> TestoqueLogs { get => contexto.TestoqueLogs; }
        public DbSet<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos; }
        public DbSet<TfinControle> TfinControles { get => contexto.TfinControles; }
        public DbSet<TnfEmitente> TnfEmitentes { get => contexto.TnfEmitentes; }
        public DbSet<Tpedido> Tpedidos { get => contexto.Tpedidos; }
        public DbSet<TpedidoDevolucao> TpedidoDevolucaos { get => contexto.TpedidoDevolucaos; }
        public DbSet<TpedidoItemDevolvido> TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos; }
        public DbSet<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos; }
        public DbSet<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseConfrontacaos { get => contexto.TpedidoAnaliseConfrontacaos; }
        public DbSet<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems; }
        public DbSet<TusuarioXLoja> TusuarioXLojas { get => contexto.TusuarioXLojas; }
        public DbSet<TorcamentistaEIndicadorVendedor> TorcamentistaEIndicadorVendedor { get => contexto.TorcamentistaEIndicadorVendedor; }
        public DbSet<TorcamentoCotacaoArquivos> TorcamentoCotacaoArquivos { get => contexto.TorcamentoCotacaoArquivos; }
        public DbSet<TprodutoCatalogo> TprodutoCatalogo { get => contexto.TprodutoCatalogo; }
        public DbSet<TprodutoCatalogoImagem> TprodutoCatalogoImagem { get => contexto.TprodutoCatalogoImagem; }
        public DbSet<TprodutoCatalogoImagemTipo> TprodutoCatalogoImagemTipo { get => contexto.TprodutoCatalogoImagemTipo; }
        public DbSet<TprodutoCatalogoItem> TprodutoCatalogoItem { get => contexto.TprodutoCatalogoItem; }
        public DbSet<TprodutoCatalogoItens> TprodutoCatalogoItens { get => contexto.TprodutoCatalogoItens; }
        public DbSet<TorcamentoCotacao> TorcamentoCotacao { get => contexto.TorcamentoCotacao; }
        public DbSet<TorcamentoCotacaoOpcao> TorcamentoCotacaoOpcao { get => contexto.TorcamentoCotacaoOpcao; }
        public DbSet<TorcamentoCotacaoOpcaoPagto> TorcamentoCotacaoOpcaoPagtos { get => contexto.TorcamentoCotacaoOpcaoPagtos; }
        public DbSet<TorcamentoCotacaoOpcaoItem> TorcamentoCotacaoOpcaoItem { get => contexto.TorcamentoCotacaoOpcaoItem; }
        public DbSet<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificados { get => contexto.TorcamentoCotacaoItemUnificados; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFins { get => contexto.TorcamentoCotacaoOpcaoItemAtomicoCustoFins; }
        public DbSet<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomico { get => contexto.TorcamentoCotacaoOpcaoItemAtomicos; }
        public DbSet<TorcamentoCotacaoOpcaoItemSku> TorcamentoCotacaoOpcaoItemSku { get => contexto.TorcamentoCotacaoOpcaoItemSku; }
        public DbSet<TcfgModulo> TcfgModulos { get => contexto.TcfgModulos; }
        public DbSet<TcfgPagtoForma> TcfgPagtoFormas { get => contexto.TcfgPagtoFormas; }
        public DbSet<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get => contexto.TcfgPagtoFormaStatus; }
        public DbSet<TcfgPagtoMeio> TcfgPagtoMeios { get => contexto.TcfgPagtoMeios; }
        public DbSet<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get => contexto.TcfgPagtoMeioStatus; }
        public DbSet<TcfgTipoParcela> TcfgTipoParcelas { get => contexto.TcfgTipoParcelas; }
        public DbSet<TcfgTipoPessoa> TcfgTipoPessoas { get => contexto.TcfgTipoPessoas; }
        public DbSet<TcfgTipoUsuario> TcfgTipoUsuarios { get => contexto.TcfgTipoUsuarios; }
        public DbSet<TcfgTipoUsuarioPerfil> TcfgTipoUsuarioPerfis { get => contexto.TcfgTipoUsuarioPerfis; }
        public DbSet<TcfgTipoUsuarioContexto> TcfgTipoUsuarioContextos { get => contexto.TcfgTipoUsuarioContextos; }
        public DbSet<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedades { get => contexto.TProdutoCatalogoPropriedades; }
        public DbSet<TcfgOrcamentoCotacaoStatus> TcfgOrcamentoCotacaoStatus { get => contexto.TcfgOrcamentoCotacaoStatus; }
#endif

        //daqui para a frente só é necessário para os testes automatizados
#if DEBUG_BANCO_DEBUG
        public DbSet<TorcamentoItem> TorcamentoItem { get => contexto.TorcamentoItem; }
        public DbSet<Tbanco> Tbancos { get => contexto.Tbancos; }
        public DbSet<Tfabricante> Tfabricantes { get => contexto.Tfabricantes; }
        public DbSet<Tproduto> Tprodutos { get => contexto.Tprodutos; }
        public DbSet<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors; }
        public DbSet<TpercentualCustoFinanceiroFornecedorHistorico> TpercentualCustoFinanceiroFornecedorHistoricos { get => contexto.TpercentualCustoFinanceiroFornecedorHistoricos; }
        public DbSet<Tparametro> Tparametros { get => contexto.Tparametros; }
        public DbSet<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds; }
        public DbSet<TwmsRegraCd> TwmsRegraCds { get => contexto.TwmsRegraCds; }
        public DbSet<TwmsRegraCdXUf> TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs; }
        public DbSet<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas; }
        public DbSet<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds; }
        public DbSet<TformaPagto> TformaPagtos { get => contexto.TformaPagtos; }
        public DbSet<TorcamentistaEIndicadorRestricaoFormaPagto> TorcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.TorcamentistaEIndicadorRestricaoFormaPagtos; }
        public DbSet<TprazoPagtoVisanet> TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets; }
        public DbSet<Tperfil> Tperfils { get => contexto.Tperfils; }
        public DbSet<TperfilItem> TperfilItens { get => contexto.TperfilItens; }
        public DbSet<TperfilUsuario> TperfilUsuarios { get => contexto.TperfilUsuarios; }
        public DbSet<TcodigoDescricao> TcodigoDescricaos { get => contexto.TcodigoDescricaos; }
        public DbSet<Tloja> Tlojas { get => contexto.Tlojas; }
        public DbSet<Toperacao> Toperacaos { get => contexto.Toperacaos; }
        public DbSet<Tlog> Tlogs { get => contexto.Tlogs; }

#endif

    }
}
