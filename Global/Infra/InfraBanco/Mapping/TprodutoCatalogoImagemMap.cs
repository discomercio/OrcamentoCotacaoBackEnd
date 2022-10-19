using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoImagemMap : IEntityTypeConfiguration<TprodutoCatalogoImagem>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogoImagem> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_IMAGEM");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int");

            builder.Property(x => x.IdProdutoCatalogo)
                .HasColumnName("id_produto_catalogo")
                .HasColumnType("int");

            builder.Property(x => x.IdTipoImagem)
                .HasColumnName("id_tipo_imagem")
                .HasColumnType("int");

            builder.Property(x => x.Caminho)
                .HasColumnName("caminho")
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .HasColumnType("int");

            builder
                .HasOne(x => x.TprodutoCatalogo)
                .WithOne(x => x.imagem)
                .HasForeignKey<TprodutoCatalogoImagem>(x => x.IdProdutoCatalogo);

        }
    }
}
