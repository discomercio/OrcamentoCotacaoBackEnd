﻿using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoMap : IEntityTypeConfiguration<TorcamentoCotacao>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacao> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.Loja)
                .HasColumnName("Loja")
                .HasColumnType("varchar(3)")
                .IsRequired(true);

            builder.Property(x => x.NomeCliente)
                .HasColumnName("NomeCliente")
                .HasColumnType("varchar(60)")
                .IsRequired(true);

            builder.Property(x => x.NomeObra)
                .HasColumnName("NomeObra")
                .HasColumnType("varchar(120)");

            builder.Property(x => x.IdVendedor)
                .HasColumnName("IdVendedor")
                .HasColumnType("int");

            builder.Property(x => x.IdIndicador)
                .HasColumnName("IdIndicador")
                .HasColumnType("int");

            builder.Property(x => x.IdIndicadorVendedor)
                .HasColumnName("IdIndicadorVendedor")
                .HasColumnType("int");

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Telefone)
                .HasColumnName("Telefone")
                .HasColumnType("varchar(15)");

            builder.Property(x => x.AceiteWhatsApp)
                .HasColumnName("AceiteWhatsApp")
                .HasColumnType("bit");

            builder.Property(x => x.UF)
                .HasColumnName("UF")
                .HasColumnType("varchar(2)");

            builder.Property(x => x.TipoCliente)
                .HasColumnName("TipoCliente")
                .HasColumnType("varchar(2)");

            builder.Property(x => x.Validade)
                .HasColumnName("Validade")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.ValidadeAnterior)
                .HasColumnName("ValidadeAnterior")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.QtdeRenovacao)
                .HasColumnName("QtdeRenovacao")
                .HasColumnType("tinyint");

            builder.Property(x => x.IdUsuarioUltRenovacao)
                .HasColumnName("IdUsuarioUltRenovacao")
                .HasColumnType("int");

            builder.Property(x => x.DataHoraUltRenovacao)
                .HasColumnName("DataHoraUltRenovacao")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.Observacao)
                .HasColumnName("Observacao")
                .HasColumnType("varchar(500)");

            builder.Property(x => x.InstaladorInstalaStatus)
                .HasColumnName("InstaladorInstalaStatus")
                .HasColumnType("smallint");

            builder.Property(x => x.GarantiaIndicadorStatus)
                .HasColumnName("GarantiaIndicadorStatus")
                .HasColumnType("tinyint");

            builder.Property(x => x.StEtgImediata)
                .HasColumnName("StEtgImediata")
                .HasColumnType("smallint");

            builder.Property(x => x.PrevisaoEntregaData)
                .HasColumnName("PrevisaoEntregaData")
                .HasColumnType("datetime");

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .HasColumnType("smallint");

            builder.Property(x => x.IdTipoUsuarioContextoUltStatus)
                .HasColumnName("IdTipoUsuarioContextoUltStatus")
                .HasColumnType("smallint");

            builder.Property(x => x.IdUsuarioUltStatus)
                .HasColumnName("IdUsuarioUltStatus")
                .HasColumnType("int");

            builder.Property(x => x.DataUltStatus)
                .HasColumnName("DataUltStatus")
                .HasColumnType("datetime");

            builder.Property(x => x.DataHoraUltStatus)
                .HasColumnName("DataHoraUltStatus")
                .HasColumnType("datetime");

            builder.Property(x => x.IdOrcamento)
                .HasColumnName("IdOrcamento")
                .HasColumnType("varchar(9)");

            builder.Property(x => x.IdPedido)
                .HasColumnName("IdPedido")
                .HasColumnType("varchar(9)");

            builder.Property(x => x.IdTipoUsuarioContextoCadastro)
                .HasColumnName("IdTipoUsuarioContextoCadastro")
                .HasColumnType("smallint");

            builder.Property(x => x.IdUsuarioCadastro)
                .HasColumnName("IdUsuarioCadastro")
                .HasColumnType("int");

            builder.Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .HasDefaultValueSql("CONVERT([datetime],CONVERT([varchar](10),getdate(),(121)),(121))");

            builder.Property(x => x.DataHoraCadastro)
                .HasColumnName("DataHoraCadastro")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getDate()");

            builder.Property(x => x.IdTipoUsuarioContextoUltAtualizacao)
                .HasColumnName("IdTipoUsuarioContextoUltAtualizacao")
                .HasColumnType("smallint");

            
            builder.Property(x => x.IdTipoUsuarioContextoUltRenovacao)
                .HasColumnName("IdTipoUsuarioContextoUltRenovacao")
                .HasColumnType("smallint");

            builder.Property(x => x.IdUsuarioUltAtualizacao)
                .HasColumnName("IdUsuarioUltAtualizacao")
                .HasColumnType("int");

            builder.Property(x => x.DataHoraUltAtualizacao)
                .HasColumnName("DataHoraUltAtualizacao")
                .HasColumnType("datetime");

            builder.Property(x => x.Perc_max_comissao_padrao)
                .HasColumnName("perc_max_comissao_padrao")
                .HasColumnType("real");

            builder.Property(x => x.Perc_max_comissao_e_desconto_padrao)
                .HasColumnName("perc_max_comissao_e_desconto_padrao")
                .HasColumnType("real");

            builder.Property(x => x.ContribuinteIcms)
                .HasColumnName("ContribuinteIcms")
                .HasColumnType("tinyint");

            builder.Property(x => x.VersaoPoliticaCredito)
                .HasColumnName("VersaoPoliticaCredito")
                .HasMaxLength(10)
                .HasColumnType("string");

            builder.Property(x => x.VersaoPoliticaPrivacidade)
                .HasColumnName("VersaoPoliticaPrivacidade")
                .HasMaxLength(10)
                .HasColumnType("string");

            builder.Property(x => x.InstaladorInstalaIdTipoUsuarioContexto)
                .HasColumnName("InstaladorInstalaIdTipoUsuarioContexto")
                .HasColumnType("smallint");

            builder.Property(x => x.InstaladorInstalaIdUsuarioUltAtualiz)
                .HasColumnName("InstaladorInstalaIdUsuarioUltAtualiz")
                .HasColumnType("int");

            builder.Property(x => x.InstaladorInstalaDtHrUltAtualiz)
                .HasColumnName("InstaladorInstalaDtHrUltAtualiz")
                .HasColumnType("datetime");

            builder.Property(x => x.GarantiaIndicadorIdTipoUsuarioContexto)
                .HasColumnName("GarantiaIndicadorIdTipoUsuarioContexto")
                .HasColumnType("smallint");

            builder.Property(x => x.GarantiaIndicadorIdUsuarioUltAtualiz)
                .HasColumnName("GarantiaIndicadorIdUsuarioUltAtualiz")
                .HasColumnType("int");

            builder.Property(x => x.GarantiaIndicadorDtHrUltAtualiz)
                .HasColumnName("GarantiaIndicadorDtHrUltAtualiz")
                .HasColumnType("datetime");

            builder.Property(x => x.EtgImediataIdTipoUsuarioContexto)
                .HasColumnName("EtgImediataIdTipoUsuarioContexto")
                .HasColumnType("smallint");

            builder.Property(x => x.EtgImediataIdUsuarioUltAtualiz)
                .HasColumnName("EtgImediataIdUsuarioUltAtualiz")
                .HasColumnType("int");

            builder.Property(x => x.EtgImediataDtHrUltAtualiz)
                .HasColumnName("EtgImediataDtHrUltAtualiz")
                .HasColumnType("datetime");

            builder.Property(x => x.PrevisaoEntregaIdTipoUsuarioContexto)
                .HasColumnName("PrevisaoEntregaIdTipoUsuarioContexto")
                .HasColumnType("smallint");

            builder.Property(x => x.PrevisaoEntregaIdUsuarioUltAtualiz)
                .HasColumnName("PrevisaoEntregaIdUsuarioUltAtualiz")
                .HasColumnType("int");

            builder.Property(x => x.PrevisaoEntregaDtHrUltAtualiz)
                .HasColumnName("PrevisaoEntregaDtHrUltAtualiz")
                .HasColumnType("datetime");

            builder.Ignore(x => x.TorcamentoCotacaoMensagems);
            builder.Ignore(x => x.TorcamentoCotacaoOpcaos);
            builder.Ignore(x => x.TorcamentoCotacaoLinks);
            builder.Ignore(x => x.TcfgOrcamentoCotacaoStatus);
            builder.Ignore(x => x.Tusuarios);

            //Relations
            builder
                .HasMany(x => x.TorcamentoCotacaoMensagems)
                .WithOne(o => o.TorcamentoCotacao)
                .HasForeignKey(f => f.IdOrcamentoCotacao);

            builder
                .HasMany(x => x.TorcamentoCotacaoOpcaos)
                .WithOne(o => o.TorcamentoCotacao)
                .HasForeignKey(f => f.IdOrcamentoCotacao);

            builder
                .HasMany(x => x.TorcamentoCotacaoLinks)
                .WithOne(o => o.TorcamentoCotacao)
                .HasForeignKey(f => f.IdOrcamentoCotacao);

            builder
                .HasOne(x => x.TcfgOrcamentoCotacaoStatus)
                .WithOne(o => o.TorcamentoCotacao)
                .HasForeignKey<TorcamentoCotacao>(f => f.Status);

        }
    }
}
