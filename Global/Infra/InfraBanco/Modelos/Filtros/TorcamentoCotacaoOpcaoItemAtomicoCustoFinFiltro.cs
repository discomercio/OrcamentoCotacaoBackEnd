using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro : IFilter
    {
        public List<int> LstIdItemAtomico { get; set; }
    }
}
