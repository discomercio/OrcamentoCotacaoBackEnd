﻿using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoItemAtomicoCustoFinMap: IEntityTypeConfiguration<TorcamentoCotacaoItemAtomicoCustoFin>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoItemAtomicoCustoFin> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdItemAtomico)
                .HasColumnName("IdItemAtomico")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.IdOpcaoPagto)
                .HasColumnName("IdOpcaoPagto")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.DescDado)
                .HasColumnName("DescDado")
                .HasColumnType("real")
                .IsRequired();

            builder.Property(x => x.PrecoLista)
                .HasColumnName("PrecoLista")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.PrecoVenda)
                .HasColumnName("PrecoVenda")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.PrecoNF)
                .HasColumnName("PrecoNF")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.CustoFinancFornecCoeficiente)
                .HasColumnName("CustoFinancFornecCoeficiente")
                .HasColumnType("real")
                .IsRequired();

            builder.Property(x => x.CustoFinancFornecPrecoListaBase)
                .HasColumnName("CustoFinancFornecPrecoListaBase")
                .HasColumnType("money")
                .IsRequired();

            builder.Ignore(x => x.TorcamentoCotacaoOpcaoItemAtomico);
            builder.Ignore(x => x.TorcamentoCotacaoOpcaoPagto);

            builder
                .HasOne(x => x.TorcamentoCotacaoOpcaoItemAtomico)
                .WithOne(o => o.TorcamentoCotacaoItemAtomicoCustoFin)
                .HasForeignKey<TorcamentoCotacaoItemAtomicoCustoFin>(f => f.IdItemAtomico);

        }
    }
}