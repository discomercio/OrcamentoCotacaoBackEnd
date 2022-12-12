using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TLogV2Map : IEntityTypeConfiguration<TLogV2>
    {
        public void Configure(EntityTypeBuilder<TLogV2> builder)
        {
            builder.ToTable("t_LOG_V2");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(x => x.Data)
                .HasColumnName("data")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.DataHora)
                .HasColumnName("data_hora")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.IdTipoUsuarioContexto)
                .HasColumnName("IdTipoUsuarioContexto")
                .HasColumnType("smallint");

            builder.Property(x => x.IdUsuario)
                .HasColumnName("IdUsuario")
                .HasColumnType("int");

            builder.Property(x => x.Loja)
                .HasColumnName("loja")
                .HasColumnType("varchar")
                .HasMaxLength(3);

            builder.Property(x => x.Pedido)
                .HasColumnName("pedido")
                .HasColumnType("varchar")
                .HasMaxLength(9);

            builder.Property(x => x.IdOrcamentoCotacao)
                .HasColumnName("IdOrcamentoCotacao")
                .HasColumnType("int");

            builder.Property(x => x.IdCliente)
                .HasColumnName("id_cliente")
                .HasColumnType("varchar")
                .HasMaxLength(12);

            builder.Property(x => x.SistemaResponsavel)
                .HasColumnName("sistema_responsavel")
                .HasColumnType("int");

            builder.Property(x => x.IdOperacao)
                .HasColumnName("IdOperacao")
                .HasColumnType("int");

            builder.Property(x => x.Complemento)
                .HasColumnName("complemento")
                .HasColumnType("varchar(max)");

            builder.Property(x => x.IP)
                .HasColumnName("IP")
                .HasColumnType("varchar")
                .HasMaxLength(64);
        }
    }
}
