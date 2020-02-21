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
    public class ContextoBdGravacao : IDisposable
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


    }
}
