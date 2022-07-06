using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoOpcaoItemAtomicoMap: IEntityTypeConfiguration<TorcamentoCotacaoOpcaoItemAtomico>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoOpcaoItemAtomico> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdItemUnificado)
                .HasColumnName("IdItemUnificado")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Fabricante)
                .HasColumnName("Fabricante")
                .HasColumnType("varchar")
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Produto)
                .HasColumnName("Produto")
                .HasColumnType("varchar")
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(x => x.Qtde)
                .HasColumnName("Qtde")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar")
                .HasMaxLength(120);

            builder.Property(x => x.DescricaoHtml)
                .HasColumnName("DescricaoHtml")
                .HasColumnType("varchar")
                .HasMaxLength(4000);

            builder.Ignore(x => x.TorcamentoCotacaoItemUnificado);
            builder.Ignore(x => x.TorcamentoCotacaoItemAtomicoCustoFin);

            builder
                .HasOne(x => x.TorcamentoCotacaoItemUnificado)
                .WithMany(o => o.TorcamentoCotacaoOpcaoItemAtomicos)
                .HasForeignKey(f => f.IdItemUnificado);

            builder
                .HasMany(x => x.TorcamentoCotacaoItemAtomicoCustoFin)
                .WithOne(o => o.TorcamentoCotacaoOpcaoItemAtomico)
                .HasForeignKey(f => f.IdItemAtomico);
        }
    }
}
