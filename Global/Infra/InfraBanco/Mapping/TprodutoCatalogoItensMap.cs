using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    public class TprodutoCatalogoItensMap : IEntityTypeConfiguration<TprodutoCatalogoItens>
    {
        public void Configure(EntityTypeBuilder<TprodutoCatalogoItens> builder)
        {
            builder.ToTable("t_PRODUTO_CATALOGO_ITENS");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Codigo)
                .HasColumnName("codigo")
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Chave)
                .HasColumnName("chave")
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Valor)
                .HasColumnName("valor")
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .HasColumnType("int");

            builder.Ignore(x => x.Codigo);
            builder.Ignore(x => x.Chave);
        }
    }
}
