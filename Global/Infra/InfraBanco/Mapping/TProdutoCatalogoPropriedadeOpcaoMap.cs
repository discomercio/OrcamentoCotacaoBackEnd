using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TProdutoCatalogoPropriedadeOpcaoMap : IEntityTypeConfiguration<TProdutoCatalogoPropriedadeOpcao>
    {
        public void Configure(EntityTypeBuilder<TProdutoCatalogoPropriedadeOpcao> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_PROPRIEDADE_OPCAO");
            builder.HasKey(o => o.id);

            builder.Property(x => x.id)
                .HasColumnName("id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.id_produto_catalogo_propriedade)
                .HasColumnName("id_produto_catalogo_propriedade")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.valor)
                .HasColumnName("valor")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.oculto)
                .HasColumnName("oculto")
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(x => x.ordem)
                .HasColumnName("ordem")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.dt_cadastro)
                .HasColumnName("dt_cadastro")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.usuario_cadastro)
                .HasColumnName("usuario_cadastro")
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder
                .HasOne(x => x.TprodutoCatalogoPropriedade)
                .WithMany(x => x.TprodutoCatalogoPropriedadeOpcaos)
                .HasForeignKey(x => x.id_produto_catalogo_propriedade);
        }
    }
}
