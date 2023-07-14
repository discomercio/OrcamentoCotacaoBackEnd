using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoItemAtomicoFiltro : IFilter
    {
        public int IdItemUnificado { get; set; }
        public List<int> LstIdItensUnifcados { get; set; } 
    }
}
