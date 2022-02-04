using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TorcamentoCotacaoStatusMap : IEntityTypeConfiguration<TorcamentoCotacaoStatus>
    {
        public void Configure(EntityTypeBuilder<TorcamentoCotacaoStatus> builder)
        {
            builder.ToTable("t_ORCAMENTO_COTACAO_STATUS");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(50)");
        }
    }
}
