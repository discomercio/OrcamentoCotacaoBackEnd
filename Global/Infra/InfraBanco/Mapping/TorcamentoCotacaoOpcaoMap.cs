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

            builder.Property(o => o.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

            builder.Property(x => x.IdOrcamentoCotacao)
                .HasColumnName("IdOrcamentoCotacao")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.PercRT)
                .HasColumnName("PercRT")
                .HasColumnType("real")
                .IsRequired();

            builder.Property(x => x.Sequencia)
                .HasColumnName("Sequencia")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.IdTipoUsuarioContextoCadastro)
                .HasColumnName("IdTipoUsuarioContextoCadastro")
                .HasColumnType("smallint");

            builder.Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("smalldatetime")
                .HasDefaultValueSql("(getdate())")
                .IsRequired();

            builder.Property(x => x.DataHoraCadastro)
                .HasColumnName("DataHoraCadastro")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(x => x.IdTipoUsuarioContextoUltAtualizacao)
                .HasColumnName("IdTipoUsuarioContextoUltAtualizacao")
                .HasColumnType("smallint");

            builder.Property(x => x.UsuarioUltimaAlteracao)
                .HasColumnName("UsuarioUltimaAlteracao")
                .HasColumnType("int");

            builder.Property(x => x.DataUltimaAlteracao)
                .HasColumnName("DataUltimaAlteracao")
                .HasColumnType("datetime");


            //Relations
            builder
                .HasOne(x => x.TorcamentoCotacao)
                .WithMany(o => o.TorcamentoCotacaoOpcaos)
                .HasForeignKey(f => f.IdOrcamentoCotacao);
        }
    }
}
