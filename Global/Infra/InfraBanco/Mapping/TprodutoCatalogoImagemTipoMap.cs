using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoImagemTipoMap : IEntityTypeConfiguration<TprodutoCatalogoImagemTipo>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogoImagemTipo> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_IMAGEM_TIPO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int");

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder
                .HasMany(x => x.TprodutoCatalogoImagems)
                .WithOne(x => x.TprodutoCatalogoImagemTipo)
                .HasForeignKey(x => x.IdTipoImagem);
        }
    }
}
