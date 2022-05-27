using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgOrcamentoCotacaoEmailTemplateFiltro : IFilter
    {
        public int? Id { get; set; }
        public int? IdCfgUnidadeNegocio { get; set; }
    }
}
