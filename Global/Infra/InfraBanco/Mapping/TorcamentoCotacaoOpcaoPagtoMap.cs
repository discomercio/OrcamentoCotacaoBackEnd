using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoOpcaoPagtoMap : IEntityTypeConfiguration<TorcamentoCotacaoOpcaoPagto>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoOpcaoPagto> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_OPCAO_PAGTO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdOrcamentoCotacaoOpcao)
                .HasColumnName("IdOrcamentoCotacaoOpcao")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Aprovado)
                .HasColumnName("Aprovado")
                .HasColumnType("bit")
                .IsRequired();

            builder.Property(x => x.IdTipoUsuarioContextoAprovado)
                .HasColumnName("IdTipoUsuarioContextoAprovado")
                .HasColumnType("smallint");

            builder.Property(x => x.IdUsuarioAprovado)
                .HasColumnName("IdUsuarioAprovado")
                .HasColumnType("int");

            builder.Property(x => x.DataAprovado)
                .HasColumnName("DataAprovado")
                .HasColumnType("datetime");

            builder.Property(x => x.DataHoraAprovado)
                .HasColumnName("DataHoraAprovado")
                .HasColumnType("datetime");

            builder.Property(x => x.Observacao)
                .HasColumnName("Observacao")
                .HasColumnType("varchar")
                .HasMaxLength(250);

            builder.Property(x => x.Tipo_parcelamento)
                .HasColumnName("tipo_parcelamento")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Av_forma_pagto)
                .HasColumnName("av_forma_pagto")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pc_qtde_parcelas)
                .HasColumnName("pc_qtde_parcelas")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pc_valor_parcela)
                .HasColumnName("pc_valor_parcela")
                .HasColumnType("money");

            builder.Property(x => x.Pc_maquineta_qtde_parcelas)
                .HasColumnName("pc_maquineta_qtde_parcelas")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pc_maquineta_valor_parcela)
                .HasColumnName("pc_maquineta_valor_parcela")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pce_forma_pagto_entrada)
                .HasColumnName("pce_forma_pagto_entrada")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pce_forma_pagto_prestacao)
                .HasColumnName("pce_forma_pagto_prestacao")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pce_entrada_valor)
                .HasColumnName("pce_entrada_valor")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pce_prestacao_qtde)
                .HasColumnName("pce_prestacao_qtde")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pce_prestacao_valor)
                .HasColumnName("pce_prestacao_valor")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pce_prestacao_periodo)
                .HasColumnName("pce_prestacao_periodo")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pse_forma_pagto_prim_prest)
                .HasColumnName("pse_forma_pagto_prim_prest")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pse_forma_pagto_demais_prest)
                .HasColumnName("pse_forma_pagto_demais_prest")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pse_prim_prest_valor)
                .HasColumnName("pse_prim_prest_valor")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pse_prim_prest_apos)
                .HasColumnName("pse_prim_prest_apos")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pse_demais_prest_qtde)
                .HasColumnName("pse_demais_prest_qtde")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pse_demais_prest_valor)
                .HasColumnName("pse_demais_prest_valor")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pse_demais_prest_periodo)
                .HasColumnName("pse_demais_prest_periodo")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pu_forma_pagto)
                .HasColumnName("pu_forma_pagto")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Pu_valor)
                .HasColumnName("pu_valor")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(x => x.Pu_vencto_apos)
                .HasColumnName("pu_vencto_apos")
                .HasColumnType("smallint")
                .IsRequired();
        }
    }
}
