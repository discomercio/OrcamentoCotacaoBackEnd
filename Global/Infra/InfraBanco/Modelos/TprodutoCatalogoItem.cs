using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_ITEM")]
    public class TprodutoCatalogoItem : IModel
    {
        public int IdProdutoCatalogo { get; set; }

        public int IdProdutoCatalogoItens { get; set; }

        public string Valor { get; set; }
    }
}
