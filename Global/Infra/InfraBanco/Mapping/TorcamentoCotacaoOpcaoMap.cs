using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoOpcaoMap : IEntityTypeConfiguration<TorcamentoCotacaoOpcao>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoOpcao> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_OPCAO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.IdOrcamentoCotacao)
                .HasColumnName("IdOrcamentoCotacao")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.VlTotal)
                .HasColumnName("VlTotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.ValorTotalComRA)
                .HasColumnName("ValorTotalComRA")
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Observacoes)
                .HasColumnName("Observacoes")
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
                .HasOne(x => x.TorcamentoCotacao)
                .WithMany(o => o.TorcamentoCotacaoOpcaos)
                .HasForeignKey(f => f.IdOrcamentoCotacao);
        }
    }
}
