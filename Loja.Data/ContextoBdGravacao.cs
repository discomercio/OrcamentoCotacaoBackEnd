using Loja.Modelo;
using Loja.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Data
{
    public class ContextoBdGravacao : IDisposable, IContextoBd
    {
        private readonly ContextoBdBasico contexto;
        public readonly IDbContextTransaction transacao;
        internal ContextoBdGravacao(ContextoBdBasico contexto)
        {
            this.contexto = contexto;
            //contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
        public Task<int> SaveChangesAsync() => contexto.SaveChangesAsync();
        public int SaveChanges() => contexto.SaveChanges();

        //acesso às tabelas
        public DbSet<Tcliente> Tclientes { get => contexto.Tclientes; }
        public DbSet<Torcamento> Torcamentos { get => contexto.Torcamentos; }
        public DbSet<TsessaoHistorico> TsessaoHistoricos { get => contexto.TsessaoHistoricos; }
        public DbSet<Tcontrole> Tcontroles { get => contexto.Tcontroles; }
        public DbSet<TprodutoLoja> TprodutoLojas { get => contexto.TprodutoLojas; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }
        public DbSet<TclienteRefBancaria> tclienteRefBancarias { get => contexto.TclienteRefBancarias; }
        public DbSet<TclienteRefComercial> tclienteRefComercials { get => contexto.TclienteRefComercials; }
        public DbSet<Tlog> tlogs { get => contexto.Tlogs; }
        public DbSet<Testoque> Testoques { get => contexto.Testoques; }
        public DbSet<TestoqueItem> TestoqueItems { get => contexto.TestoqueItems; }
        public DbSet<TestoqueMovimento> TestoqueMovimentos { get => contexto.TestoqueMovimentos; }
        public DbSet<TestoqueLog> TestoqueLogs { get => contexto.TestoqueLogs; }
        public DbSet<Tpedido> Tpedidos { get => contexto.Tpedidos; }
        public DbSet<TpedidoItem> TpedidoItems { get => contexto.TpedidoItems; }
        public DbSet<Tdesconto> Tdescontos { get => contexto.Tdescontos; }
        public DbSet<TfinControle> TfinControles { get => contexto.TfinControles; }
        public DbSet<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos; }
        public DbSet<TpedidoAnaliseEnderecoConfrontacao> TpedidoAnaliseConfrontacaos { get => contexto.TpedidoAnaliseConfrontacaos; }
        public DbSet<Tusuario> Tusuarios { get => contexto.Tusuarios; }

        IQueryable<TalertaProduto> IContextoBd.TalertaProdutos { get => contexto.TalertaProdutos; }

        IQueryable<Tbanco> IContextoBd.Tbancos { get => contexto.Tbancos; }

        IQueryable<TclienteRefBancaria> IContextoBd.TclienteRefBancarias { get => contexto.TclienteRefBancarias; }

        IQueryable<TclienteRefComercial> IContextoBd.TclienteRefComercials { get => contexto.TclienteRefComercials; }

        IQueryable<TcodigoDescricao> IContextoBd.TcodigoDescricaos { get => contexto.TcodigoDescricaos; }

        IQueryable<Tcontrole> IContextoBd.Tcontroles { get => contexto.Tcontroles; }

        IQueryable<Tdesconto> IContextoBd.Tdescontos { get => contexto.Tdescontos; }

        IQueryable<TecProdutoCompostoItem> IContextoBd.TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems; }

        IQueryable<TecProdutoComposto> IContextoBd.TecProdutoCompostos { get => contexto.TecProdutoCompostos; }

        IQueryable<TestoqueItem> IContextoBd.TestoqueItems { get => contexto.TestoqueItems; }

        IQueryable<TestoqueLog> IContextoBd.TestoqueLogs { get => contexto.TestoqueLogs; }

        IQueryable<TestoqueMovimento> IContextoBd.TestoqueMovimentos { get => contexto.TestoqueMovimentos; }

        IQueryable<Testoque> IContextoBd.Testoques { get => contexto.Testoques; }

        IQueryable<Tfabricante> IContextoBd.Tfabricantes { get => contexto.Tfabricantes; }

        IQueryable<TfinControle> IContextoBd.TfinControles { get => contexto.TfinControles; }

        IQueryable<TformaPagto> IContextoBd.TformaPagtos { get => contexto.TformaPagtos; }

        IQueryable<Tlog> IContextoBd.Tlogs { get => contexto.Tlogs; }

        IQueryable<Tloja> IContextoBd.Tlojas { get => contexto.Tlojas; }

        IQueryable<TnfEmitente> IContextoBd.TnfEmitentes { get => contexto.TnfEmitentes; }

        IQueryable<Toperacao> IContextoBd.Toperacaos { get => contexto.Toperacaos; }

        IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> IContextoBd.torcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.torcamentistaEIndicadorRestricaoFormaPagtos; }

        IQueryable<TorcamentistaEindicador> IContextoBd.TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }

        IQueryable<TorcamentoItem> IContextoBd.TorcamentoItems { get => contexto.TorcamentoItems; }

        IQueryable<Torcamento> IContextoBd.Torcamentos { get => contexto.Torcamentos; }

        IQueryable<Tparametro> IContextoBd.Tparametros { get => contexto.Tparametros; }

        IQueryable<TpedidoAnaliseEndereco> IContextoBd.TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos; }

        IQueryable<TpedidoBlocosNotas> IContextoBd.TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas; }

        IQueryable<TpedidoItemDevolvidoBlocoNotas> IContextoBd.TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas; }

        IQueryable<TpedidoItemDevolvido> IContextoBd.TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos; }

        IQueryable<TpedidoItem> IContextoBd.TpedidoItems { get => contexto.TpedidoItems; }

        IQueryable<TpedidoOcorrenciaMensagem> IContextoBd.TpedidoOcorrenciaMensagems { get => contexto.TpedidoOcorrenciaMensagems; }

        IQueryable<TpedidoOcorrencia> IContextoBd.TpedidoOcorrencias { get => contexto.TpedidoOcorrencias; }

        IQueryable<TpedidoPagamento> IContextoBd.TpedidoPagamentos { get => contexto.TpedidoPagamentos; }

        IQueryable<TpedidoPerda> IContextoBd.TpedidoPerdas { get => contexto.TpedidoPerdas; }

        IQueryable<Tpedido> IContextoBd.Tpedidos { get => contexto.Tpedidos; }

        IQueryable<TpercentualCustoFinanceiroFornecedor> IContextoBd.TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors; }

        IQueryable<TperfilItem> IContextoBd.TperfilItems { get => contexto.TperfilItems; }

        IQueryable<Tperfil> IContextoBd.Tperfils { get => contexto.Tperfils; }

        IQueryable<TperfilUsuario> IContextoBd.TperfiUsuarios { get => contexto.TperfilUsuarios; }

        IQueryable<TprazoPagtoVisanet> IContextoBd.TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets; }

        IQueryable<TprodutoLoja> IContextoBd.TprodutoLojas { get => contexto.TprodutoLojas; }

        IQueryable<Tproduto> IContextoBd.Tprodutos { get => contexto.Tprodutos; }

        IQueryable<TprodutoXAlerta> IContextoBd.TprodutoXAlertas { get => contexto.TprodutoXAlertas; }

        IQueryable<TprodutoXwmsRegraCd> IContextoBd.TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds; }

        IQueryable<TsessaoHistorico> IContextoBd.TsessaoHistoricos { get => contexto.TsessaoHistoricos; }

        IQueryable<TtransportadoraCep> IContextoBd.TtransportadoraCeps { get => contexto.TtransportadoraCeps; }

        IQueryable<Ttransportadora> IContextoBd.Ttransportadoras { get => contexto.Ttransportadoras; }

        IQueryable<Tusuario> IContextoBd.Tusuarios { get => contexto.Tusuarios; }

        IQueryable<TusuarioXLoja> IContextoBd.TusuarioXLojas { get => contexto.TusuarioXLojas; }

        IQueryable<TwmsRegraCd> IContextoBd.TwmsRegraCds { get => contexto.TwmsRegraCds; }

        IQueryable<TwmsRegraCdXUfPessoa> IContextoBd.TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas; }

        IQueryable<TwmsRegraCdXUf> IContextoBd.TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs; }

        IQueryable<TwmsRegraCdXUfXPessoaXCd> IContextoBd.TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds; }

        IQueryable<Tcliente> IContextoBd.Tclientes { get => contexto.Tclientes; }

    }
}
