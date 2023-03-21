using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Mapping
{
    public class TloginHistoricoMap : IEntityTypeConfiguration<TloginHistorico>
    {
        public void Configure(EntityTypeBuilder<TloginHistorico> builder)
        {
            builder.ToTable("t_LOGIN_HISTORICO");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.DataHora)
                .HasColumnName("DataHora")
                .HasColumnType("datetime");

            builder.Property(x => x.IdTipoUsuarioContexto)
                .HasColumnName("IdTipoUsuarioContexto")
                .HasColumnType("smallint")
                .IsRequired(false);

            builder.Property(x => x.IdUsuario)
                .HasColumnName("IdUsuario")
                .HasColumnType("int")
                .IsRequired(false);

            builder.Property(x => x.StSucesso)
                .HasColumnName("StSucesso")
                .HasColumnType("bit");

            builder.Property(x => x.Ip)
                .HasColumnName("IP")
                .HasColumnType("varchar")
                .HasMaxLength(64);

            builder.Property(x => x.sistema_responsavel)
                .HasColumnName("sistema_responsavel")
                .HasColumnType("int")
                .IsRequired(false);

            builder.Property(x => x.Login)
                .HasColumnName("Login")
                .HasColumnType("varchar")
                .HasMaxLength(255);

            builder.Property(x => x.Motivo)
                .HasColumnName("Motivo")
                .HasColumnType("varchar")
                .HasMaxLength(3);

            builder.Property(x => x.IdCfgModulo)
                .HasColumnName("IdCfgModulo")
                .HasColumnType("smallint")
                .IsRequired(false);

            builder.Property(x => x.Loja)
                .HasColumnName("loja")
                .HasColumnType("varchar")
                .HasMaxLength(3);
        }
    }
}
