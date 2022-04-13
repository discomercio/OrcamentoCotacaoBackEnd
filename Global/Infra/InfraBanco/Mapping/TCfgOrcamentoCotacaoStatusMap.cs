using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TcfgOrcamentoCotacaoStatusMap : IEntityTypeConfiguration<TcfgOrcamentoCotacaoStatus>
    {
        public void Configure(EntityTypeBuilder<TcfgOrcamentoCotacaoStatus> builder)
        {
            builder.ToTable("t_CFG_ORCAMENTO_COTACAO_STATUS");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(30)");
        }
    }
}
