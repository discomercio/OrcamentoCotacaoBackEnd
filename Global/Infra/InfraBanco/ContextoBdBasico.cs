﻿using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraBanco
{
    public class ContextoBdBasico : DbContext
    {
        internal ContextoBdBasico(DbContextOptions<ContextoBdBasico> opt) : base(opt)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TclienteRefBancaria>()
                .HasKey(o => new { o.Id_Cliente, o.Banco, o.Agencia, o.Conta });
            modelBuilder.Entity<TclienteRefComercial>()
                .HasKey(o => new { o.Id_Cliente, o.Nome_Empresa });
            modelBuilder.Entity<TpedidoItem>()
                .HasKey(o => new { o.Pedido, o.Fabricante, o.Produto });
            modelBuilder.Entity<TorcamentoItem>(o =>
            {
                o.HasKey(x => new { x.Orcamento, x.Fabricante, x.Produto });
            });

            modelBuilder.Entity<Tproduto>()
                .HasKey(x => new { x.Fabricante, x.Produto });

            modelBuilder.Entity<TecProdutoComposto>()
                .HasKey(x => new { x.Fabricante_Composto, x.Produto_Composto });

            modelBuilder.Entity<TpedidoItem>()
                .HasKey(x => new { x.Pedido, x.Fabricante, x.Produto });

            modelBuilder.Entity<TprodutoXAlerta>()
                .HasKey(x => new { x.Fabricante, x.Produto, x.Id_Alerta });

            modelBuilder.Entity<TprodutoLoja>()
                .HasKey(x => new { x.Fabricante, x.Produto, x.Loja });

            modelBuilder.Entity<TalertaProduto>()
                .HasKey(x => x.Apelido);

            modelBuilder.Entity<TsessaoHistorico>()
                .HasKey(x => new { x.Usuario, x.DtHrInicio });
            //vira e mexe dá  Database operation expected to affect 1 row(s) but actually affected 0 row(s).
            //porque o entity lê a data e quando faz o update com essa mesma data l SQL não afeta nenhum registro
            modelBuilder.Entity<TsessaoHistorico>()
                .Property(f => f.DtHrInicio).HasColumnType("datetime");

            //não tem chave no banco, mas é obrigatória para o entity. campos NULL não podem fazer parte.
            modelBuilder.Entity<TsessaoAbandonada>()
                .HasKey(o => new
                {
                    o.Usuario,
                    o.SessaoAbandonadaDtHrInicio,
                    //o.SessaoAbandonadaLoja, 
                    o.SessaoAbandonadaModulo,
                    o.SessaoSeguinteDtHrInicio,
                    //o.SessaoSeguinteLoja ,
                    o.SessaoSeguinteModulo
                });


            modelBuilder.Entity<TperfilUsuario>()
                .HasKey(x => new { x.Usuario, x.Id_perfil });
            modelBuilder.Entity<Tperfil>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Toperacao>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<TperfilItem>()
                .HasKey(x => new { x.Id });
            modelBuilder.Entity<TcodigoDescricao>()
                .HasKey(x => new { x.Grupo, x.Codigo });
            modelBuilder.Entity<TpercentualCustoFinanceiroFornecedor>()
                .HasKey(x => new { x.Fabricante, x.Tipo_Parcelamento, x.Qtde_Parcelas });
        }

        public DbSet<Tcliente> Tclientes { get; set; }
        public DbSet<Torcamento> Torcamentos { get; set; }
        public DbSet<TclienteRefBancaria> TclienteRefBancarias { get; set; }
        public DbSet<Tpedido> Tpedidos { get; set; }
        public DbSet<TorcamentistaEindicador> TorcamentistaEindicadors { get; set; }
        public DbSet<TsessaoHistorico> TsessaoHistoricos { get; set; }
        public DbSet<Tproduto> Tprodutos { get; set; }
        public DbSet<TprodutoLoja> TprodutoLojas { get; set; }
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
        public DbSet<Tcontrole> Tcontroles { get; set; }
        public DbSet<TnfEmitente> TnfEmitentes { get; set; }
        public DbSet<TecProdutoComposto> TecProdutoCompostos { get; set; }
        public DbSet<Tfabricante> Tfabricantes { get; set; }
        public DbSet<Tparametro> Tparametros { get; set; }
        public DbSet<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get; set; }
        public DbSet<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get; set; }
        public DbSet<TwmsRegraCd> TwmsRegraCds { get; set; }
        public DbSet<TwmsRegraCdXUf> TwmsRegraCdXUfs { get; set; }
        public DbSet<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get; set; }
        public DbSet<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get; set; }
        public DbSet<TecProdutoCompostoItem> TecProdutoCompostoItems { get; set; }
        public DbSet<Testoque> Testoques { get; set; }
        public DbSet<TestoqueItem> TestoqueItems { get; set; }
        public DbSet<TprodutoXAlerta> TprodutoXAlertas { get; set; }
        public DbSet<TalertaProduto> TalertaProdutos { get; set; }
        public DbSet<TformaPagto> TformaPagtos { get; set; }
        public DbSet<TorcamentistaEIndicadorRestricaoFormaPagto> torcamentistaEIndicadorRestricaoFormaPagtos { get; set; }
        public DbSet<TprazoPagtoVisanet> TprazoPagtoVisanets { get; set; }
        public DbSet<TprodutoSubgrupo> TprodutoSubgrupos { get; set; }
        public DbSet<Tusuario> Tusuarios { get; set; }
        public DbSet<TsessaoAbandonada> TsessaoAbandonadas { get; set; }
        public DbSet<Tperfil> Tperfils { get; set; }
        public DbSet<TperfilUsuario> TperfilUsuarios { get; set; }
    }
}