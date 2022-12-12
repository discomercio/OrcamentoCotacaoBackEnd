using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogoItem : IModel
    {
        [Column("id_produto_catalogo")]
        public int IdProdutoCatalogo { get; set; }

        [Column("id_produto_catalogo_propriedade")]
        public int IdProdutoCatalogoPropriedade { get; set; }

        [Column("id_produto_catalogo_propriedade_opcao")]
        public int? IdProdutoCatalogoPropriedadeOpcao { get; set; }

        [Column("valor")]
        public string Valor { get; set; }

        [Column("oculto")]
        public bool Oculto { get; set; }

        public TProdutoCatalogoPropriedadeOpcao TprodutoCatalogoPropriedadeOpcao { get; set; }
        public TProdutoCatalogoPropriedade TprodutoCatalogoPropriedade { get; set; }

        public TprodutoCatalogo TprodutoCatalogo { get; set; }
    }
}
