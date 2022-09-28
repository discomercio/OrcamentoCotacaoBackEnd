using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro : IFilter
    {
        public List<int> LstIdItemAtomico { get; set; }
        public int IdItemAtomico { get; set; }
        public int IdOpcaoPagto { get; set; }
        public bool IncluirTorcamentoCotacaoOpcaoItemAtomico { get; set; } = false;
        public bool IncluirTorcamentoCotacaoOpcaoPagto { get; set; } = false;
    }
}
