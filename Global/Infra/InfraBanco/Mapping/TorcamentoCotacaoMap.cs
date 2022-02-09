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

            builder.Property(x => x.NomeCliente)
                .HasColumnName("NomeCliente")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.NomeObra)
                .HasColumnName("NomeObra")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Parceiro)
                .HasColumnName("Parceiro")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Telefone)
                .HasColumnName("Telefone")
                .HasColumnType("varchar(15)");

            builder.Property(x => x.ConcordaWhatsapp)
                .HasColumnName("ConcordaWhatsapp")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.VendedorParceiro)
                .HasColumnName("VendedorParceiro")
                .HasColumnType("varchar(255)");

            builder.Property(x => x.UF)
                .HasColumnName("UF")
                .HasColumnType("char(2)");

            builder.Property(x => x.Tipo)
                .HasColumnName("Tipo")
                .HasColumnType("varchar(2)");

            builder.Property(x => x.Validade)
                .HasColumnName("Validade")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.IdLoja)
                .HasColumnName("IdLoja")
                .HasColumnType("varchar(3)");

            builder.Property(x => x.IdStatus)
                .HasColumnName("IdStatus")
                .HasColumnType("int");

            builder.Property(x => x.Observacao)
                .HasColumnName("Observacao")
                .HasColumnType("varchar(500)");

            builder.Property(x => x.UsuarioCadastro)
                .HasColumnName("UsuarioCadastro")
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("smalldatetime")
                .HasDefaultValueSql("(getdate())")
                .IsRequired();

            builder.Property(x => x.UsuarioUltimaAlteracao)
                .HasColumnName("UsuarioUltimaAlteracao")
                .HasColumnType("varchar(20)");

            builder.Property(x => x.DataUltimaAlteracao)
                .HasColumnName("DataUltimaAlteracao")
                .HasColumnType("smalldatetime");

            //Relations
            builder
                .HasOne(x => x.TorcamentoCotacaoStatus)
                .WithMany(o => o.TorcamentoCotacaos)
                .HasForeignKey(f => f.IdStatus);
        }
    }
}
