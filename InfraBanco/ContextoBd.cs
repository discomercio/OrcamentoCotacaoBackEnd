using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraBanco
{
    public class ContextoBd : DbContext
    {
        public ContextoBd(DbContextOptions<ContextoBd> opt) : base(opt)
        {

        }
        public DbSet<Tcliente> Tclientes { get; set; }
        public DbSet<Torcamento> Torcamentos { get; set; }
        public DbSet<TclienteRefBancaria> TclienteRefBancarias { get; set; }
        public DbSet<Tpedido> Tpedidos { get; set; }
        //public DbSet<TpedidoItem> TpedidoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TclienteRefBancaria>()
                .HasKey(o => new { o.Id_Cliente, o.Banco, o.Agencia, o.Conta });
        }
    }
}
