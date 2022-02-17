using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoMap : IEntityTypeConfiguration<TprodutoCatalogo>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogo> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Codigo)
                .HasColumnName("codigo")
                .HasColumnType("varchar(8)")
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(500)");

            builder.Property(x => x.UsuarioCadastro)
                .HasColumnName("usuario_cadastro")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(x => x.UsuarioEdicao)
                .HasColumnName("usuario_edicao")
                .HasColumnType("varchar(100)");

            builder.Property(x => x.DtCadastro)
                .HasColumnName("dt_cadastro")
                .HasColumnType("smalldatetime")
                .HasDefaultValueSql("(getdate())")
                .IsRequired();

            builder.Property(x => x.DtEdicao)
                .HasColumnName("dt_edicao")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("bit")
                .HasDefaultValueSql("1");

            builder.Ignore(x => x.campos);
            builder.Ignore(x => x.imagens);

            //public List<TorcamentoProdutoCatalogoItens> campos { get; set; }
            //public List<TorcamentoProdutoCatalogoImagem> imagens { get; set; }
        }
    }
}
