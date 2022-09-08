using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TcfgDataTypeMap : IEntityTypeConfiguration<TcfgDataType>
    {
        public void Configure(EntityTypeBuilder<TcfgDataType> builder)
        {
            builder.ToTable("t_CFG_DATA_TYPE");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Sigla)
                .HasColumnName("Sigla")
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(80)")
                .IsRequired();

            builder.Property(x => x.Ordenacao)
                .HasColumnName("Ordenacao")
                .HasColumnType("smallint")
                .IsRequired();
        }
    }
}
