using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TcfgOperacaoMap : IEntityTypeConfiguration<TcfgOperacao>
    {
        public void Configure(EntityTypeBuilder<TcfgOperacao> builder)
        {
            builder.ToTable("t_CFG_LOG_OPERACAO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.Sigla)
                .HasColumnName("Sigla")
                .HasColumnType("varchar")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar")
                .HasMaxLength(800)
                .IsRequired();

            builder.Property(x => x.DataHoraCadastro)
                .HasColumnName("DataHoraCadastro")
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
