using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Modelos.Filtros
{
    public class TcodigoDescricaoFiltro : IFilter
    {
        public string Grupo { get; set; }

        public string Codigo { get; set; }
    }
}
