using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoPropriedadeMap : IEntityTypeConfiguration<TProdutoCatalogoPropriedade>
    {
        public void Configure(EntityTypeBuilder<TProdutoCatalogoPropriedade> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_PROPRIEDADE");
            builder.HasKey(o => o.id);

            builder.Property(x => x.id)
                .HasColumnName("id")
                .HasColumnType("int")
                .HasDefaultValueSql("NEXT VALUE FOR [dbo].[seq_PRODUTO_CATALOGO_PROPRIEDADE]"); ;

            builder.Property(x => x.IdCfgTipoPropriedade)
                .HasColumnName("IdCfgTipoPropriedade")
                .HasColumnType("smallint");

            builder.Property(x => x.IdCfgTipoPermissaoEdicaoCadastro)
                .HasColumnName("IdCfgTipoPermissaoEdicaoCadastro")
                .HasColumnType("smallint");

            builder.Property(x => x.IdCfgDataType)
                .HasColumnName("IdCfgDataType")
                .HasColumnType("smallint");

            builder.Property(x => x.descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(100)")
                .IsRequired(true);

            builder.Property(x => x.oculto)
                .HasColumnName("oculto")
                .HasColumnType("bit");

            builder.Property(x => x.ordem)
                .HasColumnName("ordem")
                .HasColumnType("int");

            builder.Property(x => x.usuario_cadastro)
                .HasColumnName("usuario_cadastro")
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder
                .HasOne(x => x.TcfgDataType)
                .WithMany(x => x.TprodutoCatalogoPropriedades)
                .HasForeignKey(x => x.IdCfgDataType);

            builder
                .HasOne(x => x.TcfgTipoPermissaoEdicaoCadastro)
                .WithMany(x => x.TprodutoCatalogoPropriedades)
                .HasForeignKey(x => x.IdCfgTipoPermissaoEdicaoCadastro);

            builder
                .HasOne(x => x.TcfgTipoPropriedadeProdutoCatalogo)
                .WithMany(x => x.TprodutoCatalogoPropriedades)
                .HasForeignKey(x => x.IdCfgTipoPropriedade);

            builder
                .HasMany(x => x.TprodutoCatalogoPropriedadeOpcaos)
                .WithOne(x => x.TprodutoCatalogoPropriedade)
                .HasForeignKey(x => x.id_produto_catalogo_propriedade);

            builder
                .HasMany(x => x.TprodutoCatalogoItems)
                .WithOne(x => x.TprodutoCatalogoPropriedade)
                .HasForeignKey(x => x.IdProdutoCatalogoPropriedade);
        }
    }
}
