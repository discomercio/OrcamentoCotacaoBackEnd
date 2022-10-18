using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    public class TprodutoCatalogoItem : IModel
    {
        public int IdProdutoCatalogo { get; set; }
        public int IdProdutoCatalogoPropriedade { get; set; }
        public int? IdProdutoCatalogoPropriedadeOpcao { get; set; }
        public string Valor { get; set; }
        public bool Oculto { get; set; }

        public TProdutoCatalogoPropriedadeOpcao TprodutoCatalogoPropriedadeOpcao { get; set; }
    }
}
