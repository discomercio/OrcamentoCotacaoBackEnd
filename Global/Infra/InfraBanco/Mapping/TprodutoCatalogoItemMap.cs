using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoItemMap : IEntityTypeConfiguration<TprodutoCatalogoItem>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogoItem> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_ITEM");
            builder.HasKey(o => new { o.IdProdutoCatalogo, o.IdProdutoCatalogoItens });

            builder.Property(x => x.IdProdutoCatalogo)
                .HasColumnName("id_produto_catalogo")
                .HasColumnType("int");

            builder.Property(x => x.IdProdutoCatalogoItens)
                .HasColumnName("id_produto_catalogo_itens")
                .HasColumnType("int");

            builder.Property(x => x.Valor)
                .HasColumnName("valor")
                .HasColumnType("varchar(max)");
        }
    }
}
