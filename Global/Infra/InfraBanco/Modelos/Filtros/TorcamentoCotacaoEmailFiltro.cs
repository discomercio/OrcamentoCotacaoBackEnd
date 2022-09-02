using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoEmailFiltro : IFilter
    {
        public int? IdOrcamentoCotacao { get; set; }
        public long? IdOrcamentoCotacaoEmailQueue { get; set; }
    }
}
