﻿using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoItemMap : IEntityTypeConfiguration<TprodutoCatalogoItem>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogoItem> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_ITEM");
            builder.HasKey(o => new { o.IdProdutoCatalogo, o.IdProdutoCatalogoPropriedade });

            builder.Property(x => x.IdProdutoCatalogo)
                .HasColumnName("id_produto_catalogo")
                .HasColumnType("int");

            builder.Property(x => x.IdProdutoCatalogoPropriedade)
                .HasColumnName("id_produto_catalogo_propriedade")
                .HasColumnType("int");

            builder.Property(x => x.IdProdutoCatalogoPropriedadeOpcao)
                .HasColumnName("id_produto_catalogo_propriedade_opcao")
                .HasColumnType("int");

            builder.Property(x => x.Valor)
                .HasColumnName("valor")
                .HasColumnType("varchar(max)");

            builder.Property(x => x.Oculto)
                .HasColumnName("oculto")
                .HasColumnType("bool");
        }
    }
}
