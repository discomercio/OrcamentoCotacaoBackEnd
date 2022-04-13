using InfraBanco.Modelos;
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
                .HasColumnType("varchar(3)");

            builder.Property(x => x.NomeCliente)
                .HasColumnName("NomeCliente")
                .HasColumnType("varchar(60)");

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
                .HasColumnType("int");

            builder.Property(x => x.UF)
                .HasColumnName("UF")
                .HasColumnType("varchar(2)");

            //builder.Property(x => x.Tipo)
            //    .HasColumnName("Tipo")
            //    .HasColumnType("varchar(2)");

            builder.Property(x => x.Validade)
                .HasColumnName("Validade")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.ValidadeAnterior)
                .HasColumnName("ValidadeAnterior")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.QtdeRenovacao)
                .HasColumnName("QtdeRenovacao")
                .HasColumnType("int");

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

            builder.Property(x => x.IdUsuarioUltAtualizacao)
                .HasColumnName("IdUsuarioUltAtualizacao")
                .HasColumnType("int");

            builder.Property(x => x.DataHoraUltAtualizacao)
                .HasColumnName("DataHoraUltAtualizacao")
                .HasColumnType("datetime");

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
