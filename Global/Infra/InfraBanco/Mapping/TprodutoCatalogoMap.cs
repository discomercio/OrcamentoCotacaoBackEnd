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

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int")
                .UseSqlServerIdentityColumn();

            builder.Property(x => x.Produto)
                .HasColumnName("produto")
                .HasColumnType("varchar(8)")
                .IsRequired();

            builder.Property(x => x.Fabricante)
                .HasColumnName("fabricante")
                .HasColumnType("varchar(4)")
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao_completa")
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

            builder
                .HasMany(x => x.campos)
                .WithOne(x => x.TprodutoCatalogo)
                .HasForeignKey(x => x.IdProdutoCatalogo);

            builder
                .HasOne(x => x.imagem)
                .WithOne(x => x.TprodutoCatalogo)
                .HasForeignKey<TprodutoCatalogoImagem>(x => x.IdProdutoCatalogo);
        }
    }
}
