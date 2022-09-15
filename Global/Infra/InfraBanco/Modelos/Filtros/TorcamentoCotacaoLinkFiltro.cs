using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoLinkFiltro : IFilter
    {
        public int? IdOrcamentoCotacao { get; set; }
        public Int16? Status { get; set; }
    }
}
