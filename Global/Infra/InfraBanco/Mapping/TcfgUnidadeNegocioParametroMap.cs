using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    internal class TcfgUnidadeNegocioParametroMap : IEntityTypeConfiguration<TcfgUnidadeNegocioParametro>
    {
        public void Configure(EntityTypeBuilder<TcfgUnidadeNegocioParametro> builder)
        {
            builder.ToTable("T_CFG_UNIDADE_NEGOCIO_PARAMETRO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.IdCfgUnidadeNegocio)
                .HasColumnName("IdCfgUnidadeNegocio")
                .HasColumnType("int");

            builder.Property(x => x.IdCfgParametro)
                .HasColumnName("IdCfgParametro")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Valor)
                .HasColumnName("Valor")
                .HasColumnType("varchar")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.DataHoraCadastro)
                .HasColumnName("DataHoraCadastro")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.DataHoraUltAtualizacao)
                .HasColumnName("DataHoraUltAtualizacao")
                .HasColumnType("DateTime");
        }
    }
}