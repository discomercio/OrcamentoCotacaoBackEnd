using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TecProdutoCompostoItemMap: IEntityTypeConfiguration<TecProdutoCompostoItem>
    {
        public void Configure(EntityTypeBuilder<TecProdutoCompostoItem> builder)
        {
            builder.ToTable("t_EC_PRODUTO_COMPOSTO_ITEM");
            builder.HasKey(t => new {t.Fabricante_composto, t.Produto_composto, t.Produto_item});

            builder.Property(x => x.Fabricante_composto)
                .HasColumnName("fabricante_composto")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Produto_composto)
                .HasColumnName("produto_composto")
                .HasColumnType("varchar")
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(x => x.Fabricante_item)
                .HasColumnName("fabricante_item")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Produto_item)
                .HasColumnName("produto_item")
                .HasColumnType("varchar")
                .HasMaxLength(8);

            builder.Property(x => x.Qtde)
                .HasColumnName("qtde")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Sequencia)
                .HasColumnName("sequencia")
                .HasColumnType("smallint");

            builder.Property(x => x.Excluido_status)
                .HasColumnName("excluido_status")
                .HasColumnType("smallint");

            builder.Ignore(x => x.TecProdutoComposto);

            builder
                .HasOne(x => x.TecProdutoComposto)
                .WithMany(o => o.TecProdutoCompostoItems)
                .HasForeignKey(f => new { f.Fabricante_composto, f.Produto_composto });
        }
    }
}
