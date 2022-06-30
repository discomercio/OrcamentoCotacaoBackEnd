using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TcfgTipoUsuarioContextoMap : IEntityTypeConfiguration<TcfgTipoUsuarioContexto>
    {
        public void Configure(EntityTypeBuilder<TcfgTipoUsuarioContexto> builder)
        {
            builder.ToTable("t_CFG_TIPO_USUARIO_CONTEXTO");
            builder.HasKey(o => o.Id);

            //builder.Property(x => x.Descricao)
            //    .HasColumnName("Descricao")
            //    .HasColumnType("varchar(30)");

            builder
                .HasOne(x => x.TorcamentoCotacaoOpcaoPagto)
                .WithOne(o => o.TcfgTipoUsuarioContexto)
                .HasForeignKey<TorcamentoCotacaoOpcaoPagto>(f => f.IdTipoUsuarioContextoAprovado);
        }
    }
}
