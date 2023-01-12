using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TprodutoGrupoMap : IEntityTypeConfiguration<TprodutoGrupo>
    {
        public void Configure(EntityTypeBuilder<TprodutoGrupo> builder)
        {
            builder.ToTable("t_PRODUTO_GRUPO");
            builder.HasKey(o => o.Codigo);

            builder.Property(x => x.Codigo)
                .HasColumnName("codigo")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(x => x.Inativo)
                .HasColumnName("inativo")
                .HasColumnType("tinyint")
                .IsRequired();

            builder.Property(x => x.DtHrCadastro)
                .HasColumnName("dt_hr_cadastro")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.UsuarioCadastro)
                .HasColumnName("usuario_cadastro")
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.DtHrUltAtualizacao)
                .HasColumnName("dt_hr_ult_atualizacao")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.UsuarioUltAtualizacao)
                .HasColumnName("usuario_ult_atualizacao")
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}
