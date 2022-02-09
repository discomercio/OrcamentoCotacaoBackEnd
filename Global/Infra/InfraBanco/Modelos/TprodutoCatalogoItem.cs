using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_ITEM")]
    public class TprodutoCatalogoItem : IModel
    {
        //[Key]
        //[Column(name: "id_produto_catalogo", Order = 1)]
        //[MaxLength(8)]
        //[Required]
        public string IdProdutoCatalogo { get; set; }

        //[Key]
        //[Column(name: "id_produto_catalogo_itens", Order = 2)]
        //[Required]
        public int IdProdutoCatalogoItens { get; set; }

        //[Required]
        //[Column("valor")]
        //[MaxLength(8000)]
        public string Valor { get; set; }
    }
}
