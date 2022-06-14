using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TprodutoCatalogoItemFiltro: IFilter
    {
        public bool Oculto { get; set; } = false;
        public int IdProdutoCatalogoPropriedade { get; set; }
    }
}
