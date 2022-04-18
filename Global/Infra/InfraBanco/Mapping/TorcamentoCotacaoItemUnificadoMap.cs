    using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoItemUnificadoMap : IEntityTypeConfiguration<TorcamentoCotacaoItemUnificado>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoItemUnificado> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdOrcamentoCotacaoOpcao)
                .HasColumnName("IdOrcamentoCotacaoOpcao")
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

            builder.Property(x => x.Sequencia)
                .HasColumnName("Sequencia")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Ignore(x => x.TorcamentoCotacaoOpcao);
            builder.Ignore(x => x.TorcamentoCotacaoOpcaoItemAtomicos);

            builder
                .HasOne(x => x.TorcamentoCotacaoOpcao)
                .WithMany(o => o.TorcamentoCotacaoItemUnificados)
                .HasForeignKey(x => x.IdOrcamentoCotacaoOpcao);

            builder
                .HasMany(x => x.TorcamentoCotacaoOpcaoItemAtomicos)
                .WithOne(o => o.TorcamentoCotacaoItemUnificado)
                .HasForeignKey(f => f.IdItemUnificado);
        }
    }
}
