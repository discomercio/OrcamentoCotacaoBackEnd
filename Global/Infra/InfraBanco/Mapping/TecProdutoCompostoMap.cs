using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TecProdutoCompostoMap : IEntityTypeConfiguration<TecProdutoComposto>
    {
        public void Configure(EntityTypeBuilder<TecProdutoComposto> builder)
        {
            builder.ToTable("t_EC_PRODUTO_COMPOSTO");
            builder.HasKey(t => new { t.Fabricante_Composto, t.Produto_Composto });

            builder.Property(x => x.Fabricante_Composto)
                .HasColumnName("fabricante_composto")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Produto_Composto)
                .HasColumnName("produto_composto")
                .HasColumnType("varchar")
                .HasMaxLength(8)
                .IsRequired();

            builder.Ignore(x => x.TecProdutoCompostoItems);

            //builder
            //    .HasMany(x => x.TecProdutoCompostoItems)
            //    .WithOne(o => o.TecProdutoComposto)
            //    .HasForeignKey(f => new { f.Fabricante_composto, f.Produto_composto });
        }
    }
}
