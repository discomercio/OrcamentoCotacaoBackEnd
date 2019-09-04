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
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicadors { get; set; }
        public DbSet<TsessaoHistorico> TsessaoHistoricos { get; set; }
        public DbSet<Tusuario> Tusuarios { get; set; }
        public DbSet<Tproduto> Tprodutos { get; set; }
        public DbSet<TpedidoItem> TpedidoItems { get; set; }
        public DbSet<TpedidoItemDevolvido> TpedidoItemDevolvidos { get; set; }
        public DbSet<TpedidoPerda> TpedidoPerdas { get; set; }
        public DbSet<TpedidoPagamento> TpedidoPagamentos { get; set; }
        public DbSet<TestoqueMovimento> TestoqueMovimentos { get; set; }
        public DbSet<Ttransportadora> Ttransportadoras { get; set; }
        public DbSet<TpedidoBlocosNotas> TpedidoBlocosNotas { get; set; }
        public DbSet<TcodigoDescricao> TcodigoDescricaos { get; set; }
        public DbSet<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagems { get; set; }
        public DbSet<TpedidoOcorrencia> TpedidoOcorrencias { get; set; }
        public DbSet<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get; set; }
        public DbSet<TorcamentoItem> TorcamentoItems { get; set; }
        public DbSet<Tbanco> Tbancos { get; set; }
        public DbSet<TclienteRefComercial> TclienteRefComercials { get; set; }
        public DbSet<Tlog> Tlogs { get; set; }
        public DbSet<Tloja> Tlojas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TclienteRefBancaria>()
                .HasKey(o => new { o.Id_Cliente, o.Banco, o.Agencia, o.Conta });
        }
    }
}
