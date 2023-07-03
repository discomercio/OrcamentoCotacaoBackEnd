using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Mapping
{
    public class TcfgOrcamentoCotacaoEndpointFilterMap : IEntityTypeConfiguration<TcfgOrcamentoCotacaoEndpointFilter>
    {
        public void Configure(EntityTypeBuilder<TcfgOrcamentoCotacaoEndpointFilter> builder)
        {
            builder.ToTable("t_CFG_ORCAMENTO_COTACAO_ENDPOINT_FILTER");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("int")
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(x => x.Endpoint)
                .HasColumnName("Endpoint")
                .HasColumnType("varchar")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(x => x.Delay)
                .HasColumnName("Delay")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Observacoes)
                .HasColumnName("Observacoes")
                .HasColumnType("varchar");
        }
    }
}
