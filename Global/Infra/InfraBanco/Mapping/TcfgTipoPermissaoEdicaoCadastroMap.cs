using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TcfgTipoPermissaoEdicaoCadastroMap : IEntityTypeConfiguration<TcfgTipoPermissaoEdicaoCadastro>
    {
        public void Configure(EntityTypeBuilder<TcfgTipoPermissaoEdicaoCadastro> builder)
        {
            builder.ToTable("t_CFG_TIPO_PERMISSAO_EDICAO_CADASTRO");
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
                .WithOne(x => x.TcfgTipoPermissaoEdicaoCadastro)
                .HasForeignKey(x => x.IdCfgTipoPermissaoEdicaoCadastro);
        }
    }
}
