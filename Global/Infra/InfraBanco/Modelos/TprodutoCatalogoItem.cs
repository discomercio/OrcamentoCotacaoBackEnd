using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_ITEM")]
    public class TprodutoCatalogoItem : IModel
    {
        public int IdProdutoCatalogo { get; set; }
        public int IdProdutoCatalogoPropriedade { get; set; }
        public int IdProdutoCatalogoPropriedadeOpcao { get; set; }
        public string Valor { get; set; }
        public bool Oculto { get; set; }

    }
}
