using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoEmailQueueFiltro : IFilter
    {
        public bool? Sent { get; set; }

        public long? Id { get; set; }
    }
}
