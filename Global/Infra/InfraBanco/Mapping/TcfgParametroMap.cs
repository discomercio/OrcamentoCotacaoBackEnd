using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    internal class TcfgParametroMap : IEntityTypeConfiguration<TcfgParametro>
    {
        public void Configure(EntityTypeBuilder<TcfgParametro> builder)
        {
            builder.ToTable("t_CFG_PARAMETRO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.IdCfgDataType)
                .HasColumnName("IdCfgDataType")
                .HasColumnType("int");

            builder.Property(x => x.Sigla)
                .HasColumnName("Sigla")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(4000)")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.DataHoraCadastro)
                .HasColumnName("DataHoraCadastro")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(x => x.DataHoraUltAtualizacao)
                .HasColumnName("DataHoraUltAtualizacao")
                .HasColumnType("DateTime");
        }
    }
}