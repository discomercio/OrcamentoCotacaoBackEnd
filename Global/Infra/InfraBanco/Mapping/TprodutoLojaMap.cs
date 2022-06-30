using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraBanco.Mapping
{
    //COMENTADOS: NAO ESTAO MAPEADOS EN ENTIDADE C#

    public class TprodutoLojaMap : IEntityTypeConfiguration<TprodutoLoja>
    {
        public void Configure(EntityTypeBuilder<TprodutoLoja> builder)
        {
            builder.ToTable("t_PRODUTO_LOJA");
            builder.HasKey(o => new { o.Fabricante, o.Produto, o.Loja });

            //builder.Property(x => x.Preco_Lista)
            //    .HasColumnName("preco_lista")
            //    .HasColumnType("money");

            //builder.Property(x => x.Margem)
            //    .HasColumnName("margem")
            //    .HasColumnType("real");
        }
    }
}


