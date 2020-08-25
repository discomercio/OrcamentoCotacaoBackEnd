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
    public class LojaContextoBdGravacao : IDisposable, ILojaContextoBd
    {
        private readonly LojaContextoBdBasico contexto;
        public readonly IDbContextTransaction transacao;
        internal LojaContextoBdGravacao(LojaContextoBdBasico contexto)
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

        IQueryable<TalertaProduto> ILojaContextoBd.TalertaProdutos { get => contexto.TalertaProdutos; }

        IQueryable<Tbanco> ILojaContextoBd.Tbancos { get => contexto.Tbancos; }

        IQueryable<TclienteRefBancaria> ILojaContextoBd.TclienteRefBancarias { get => contexto.TclienteRefBancarias; }

        IQueryable<TclienteRefComercial> ILojaContextoBd.TclienteRefComercials { get => contexto.TclienteRefComercials; }

        IQueryable<TcodigoDescricao> ILojaContextoBd.TcodigoDescricaos { get => contexto.TcodigoDescricaos; }

        IQueryable<Tcontrole> ILojaContextoBd.Tcontroles { get => contexto.Tcontroles; }

        IQueryable<Tdesconto> ILojaContextoBd.Tdescontos { get => contexto.Tdescontos; }

        IQueryable<TecProdutoCompostoItem> ILojaContextoBd.TecProdutoCompostoItems { get => contexto.TecProdutoCompostoItems; }

        IQueryable<TecProdutoComposto> ILojaContextoBd.TecProdutoCompostos { get => contexto.TecProdutoCompostos; }

        IQueryable<TestoqueItem> ILojaContextoBd.TestoqueItems { get => contexto.TestoqueItems; }

        IQueryable<TestoqueLog> ILojaContextoBd.TestoqueLogs { get => contexto.TestoqueLogs; }

        IQueryable<TestoqueMovimento> ILojaContextoBd.TestoqueMovimentos { get => contexto.TestoqueMovimentos; }

        IQueryable<Testoque> ILojaContextoBd.Testoques { get => contexto.Testoques; }

        IQueryable<Tfabricante> ILojaContextoBd.Tfabricantes { get => contexto.Tfabricantes; }

        IQueryable<TfinControle> ILojaContextoBd.TfinControles { get => contexto.TfinControles; }

        IQueryable<TformaPagto> ILojaContextoBd.TformaPagtos { get => contexto.TformaPagtos; }

        IQueryable<Tlog> ILojaContextoBd.Tlogs { get => contexto.Tlogs; }

        IQueryable<Tloja> ILojaContextoBd.Tlojas { get => contexto.Tlojas; }

        IQueryable<TnfEmitente> ILojaContextoBd.TnfEmitentes { get => contexto.TnfEmitentes; }

        IQueryable<Toperacao> ILojaContextoBd.Toperacaos { get => contexto.Toperacaos; }

        IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> ILojaContextoBd.torcamentistaEIndicadorRestricaoFormaPagtos { get => contexto.torcamentistaEIndicadorRestricaoFormaPagtos; }

        IQueryable<TorcamentistaEindicador> ILojaContextoBd.TorcamentistaEindicadors { get => contexto.TorcamentistaEindicadors; }

        IQueryable<TorcamentoItem> ILojaContextoBd.TorcamentoItems { get => contexto.TorcamentoItems; }

        IQueryable<Torcamento> ILojaContextoBd.Torcamentos { get => contexto.Torcamentos; }

        IQueryable<Tparametro> ILojaContextoBd.Tparametros { get => contexto.Tparametros; }

        IQueryable<TpedidoAnaliseEndereco> ILojaContextoBd.TpedidoAnaliseEnderecos { get => contexto.TpedidoAnaliseEnderecos; }

        IQueryable<TpedidoBlocosNotas> ILojaContextoBd.TpedidoBlocosNotas { get => contexto.TpedidoBlocosNotas; }

        IQueryable<TpedidoItemDevolvidoBlocoNotas> ILojaContextoBd.TpedidoItemDevolvidoBlocoNotas { get => contexto.TpedidoItemDevolvidoBlocoNotas; }

        IQueryable<TpedidoItemDevolvido> ILojaContextoBd.TpedidoItemDevolvidos { get => contexto.TpedidoItemDevolvidos; }

        IQueryable<TpedidoItem> ILojaContextoBd.TpedidoItems { get => contexto.TpedidoItems; }

        IQueryable<TpedidoOcorrenciaMensagem> ILojaContextoBd.TpedidoOcorrenciaMensagems { get => contexto.TpedidoOcorrenciaMensagems; }

        IQueryable<TpedidoOcorrencia> ILojaContextoBd.TpedidoOcorrencias { get => contexto.TpedidoOcorrencias; }

        IQueryable<TpedidoPagamento> ILojaContextoBd.TpedidoPagamentos { get => contexto.TpedidoPagamentos; }

        IQueryable<TpedidoPerda> ILojaContextoBd.TpedidoPerdas { get => contexto.TpedidoPerdas; }

        IQueryable<Tpedido> ILojaContextoBd.Tpedidos { get => contexto.Tpedidos; }

        IQueryable<TpercentualCustoFinanceiroFornecedor> ILojaContextoBd.TpercentualCustoFinanceiroFornecedors { get => contexto.TpercentualCustoFinanceiroFornecedors; }

        IQueryable<TperfilItem> ILojaContextoBd.TperfilItems { get => contexto.TperfilItems; }

        IQueryable<Tperfil> ILojaContextoBd.Tperfils { get => contexto.Tperfils; }

        IQueryable<TperfilUsuario> ILojaContextoBd.TperfiUsuarios { get => contexto.TperfilUsuarios; }

        IQueryable<TprazoPagtoVisanet> ILojaContextoBd.TprazoPagtoVisanets { get => contexto.TprazoPagtoVisanets; }

        IQueryable<TprodutoLoja> ILojaContextoBd.TprodutoLojas { get => contexto.TprodutoLojas; }

        IQueryable<Tproduto> ILojaContextoBd.Tprodutos { get => contexto.Tprodutos; }

        IQueryable<TprodutoXAlerta> ILojaContextoBd.TprodutoXAlertas { get => contexto.TprodutoXAlertas; }

        IQueryable<TprodutoXwmsRegraCd> ILojaContextoBd.TprodutoXwmsRegraCds { get => contexto.TprodutoXwmsRegraCds; }

        IQueryable<TsessaoHistorico> ILojaContextoBd.TsessaoHistoricos { get => contexto.TsessaoHistoricos; }

        IQueryable<TtransportadoraCep> ILojaContextoBd.TtransportadoraCeps { get => contexto.TtransportadoraCeps; }

        IQueryable<Ttransportadora> ILojaContextoBd.Ttransportadoras { get => contexto.Ttransportadoras; }

        IQueryable<Tusuario> ILojaContextoBd.Tusuarios { get => contexto.Tusuarios; }

        IQueryable<TusuarioXLoja> ILojaContextoBd.TusuarioXLojas { get => contexto.TusuarioXLojas; }

        IQueryable<TwmsRegraCd> ILojaContextoBd.TwmsRegraCds { get => contexto.TwmsRegraCds; }

        IQueryable<TwmsRegraCdXUfPessoa> ILojaContextoBd.TwmsRegraCdXUfPessoas { get => contexto.TwmsRegraCdXUfPessoas; }

        IQueryable<TwmsRegraCdXUf> ILojaContextoBd.TwmsRegraCdXUfs { get => contexto.TwmsRegraCdXUfs; }

        IQueryable<TwmsRegraCdXUfXPessoaXCd> ILojaContextoBd.TwmsRegraCdXUfXPessoaXCds { get => contexto.TwmsRegraCdXUfXPessoaXCds; }

        IQueryable<Tcliente> ILojaContextoBd.Tclientes { get => contexto.Tclientes; }

    }
}
