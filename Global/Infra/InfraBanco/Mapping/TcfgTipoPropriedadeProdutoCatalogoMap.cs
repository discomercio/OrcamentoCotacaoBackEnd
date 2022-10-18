using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TcfgTipoPropriedadeProdutoCatalogoMap : IEntityTypeConfiguration<TcfgTipoPropriedadeProdutoCatalogo>
    {
        public void Configure(EntityTypeBuilder<TcfgTipoPropriedadeProdutoCatalogo> builder)
        {
            builder.ToTable("t_CFG_TIPO_PROPRIEDADE_PRODUTO_CATALOGO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Sigla)
                .HasColumnName("Sigla")
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder
                .HasMany(x => x.TprodutoCatalogoPropriedades)
                .WithOne(x => x.TcfgTipoPropriedadeProdutoCatalogo)
                .HasForeignKey(x => x.IdCfgTipoPropriedade);
        }
    }
}
