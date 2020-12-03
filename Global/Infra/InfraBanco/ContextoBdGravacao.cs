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
        public readonly IDbContextTransaction transacao;
        internal ContextoBdGravacao(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            transacao = contexto.Database.BeginTransaction();
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
        public DbSet<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos; }
        public DbSet<Tcontrole> Tcontroles { get => contexto.Tcontroles; }
        public DbSet<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }
        public DbSet<TsessaoAbandonada> TsessaoAbandonadas { get => contexto.TsessaoAbandonadas; }
        public DbSet<Tusuario> Tusuarios { get => contexto.Tusuarios; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        public DbSet<TnfEmitente> TnfEmitentes { get => contexto.TnfEmitentes; }
        public DbSet<Tpedido> Tpedidos { get => contexto.Tpedidos; }
        public DbSet<TestoqueLog> TestoqueLogs { get => contexto.TestoqueLogs; }
        public DbSet<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos; }
        public DbSet<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems; }
        public DbSet<Tdesconto> Tdescontos { get => contexto.Tdescontos; }
        public DbSet<TfinControle> TfinControles { get => contexto.TfinControles; }
        public DbSet<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos; }
        public DbSet<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseConfrontacaos { get => contexto.TpedidoAnaliseConfrontacaos; }
        public DbSet<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems; }
        public DbSet<Testoque> Testoques { get => contexto.Testoques; }
        public DbSet<TusuarioXLoja> TusuarioXLojas { get => contexto.TusuarioXLojas; }
        public DbSet<TclienteRefComercial> TclienteRefComercials { get => contexto.TclienteRefComercials; }
        public DbSet<TclienteRefBancaria> TclienteRefBancarias { get => contexto.TclienteRefBancarias; }
#endif

        //daqui para a frente só é necessário para os testes automatizados
#if DEBUG_BANCO_DEBUG
        public DbSet<Tbanco> Tbancos { get => contexto.Tbancos; }
        public DbSet<Tfabricante> Tfabricantes { get => contexto.Tfabricantes; }
        public DbSet<Tproduto> Tprodutos { get => contexto.Tprodutos; }
        public DbSet<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors; }
        public DbSet<Tparametro> Tparametros { get => contexto.Tparametros; }
        public DbSet<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds; }
        public DbSet<TwmsRegraCd> TwmsRegraCds { get => contexto.TwmsRegraCds; }
        public DbSet<TwmsRegraCdXUf> TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs; }
        public DbSet<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas; }
        public DbSet<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds; }
        public DbSet<TformaPagto> TformaPagtos { get => contexto.TformaPagtos; }
        public DbSet<TorcamentistaEIndicadorRestricaoFormaPagto> torcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.torcamentistaEIndicadorRestricaoFormaPagtos; }
        public DbSet<TprazoPagtoVisanet> TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets; }
        public DbSet<Tperfil> Tperfils { get => contexto.Tperfils; }
        public DbSet<TperfilUsuario> TperfilUsuarios { get => contexto.TperfilUsuarios; }
        public DbSet<TcodigoDescricao> TcodigoDescricaos { get => contexto.TcodigoDescricaos; }
        public DbSet<Tloja> Tlojas { get => contexto.Tlojas; }
#endif

    }
}
