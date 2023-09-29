using InfraBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relatorios
{
    public class RelatoriosData
    {
        private readonly ContextoRelatorioProvider contexto;

        public RelatoriosData(ContextoRelatorioProvider _contexto)
        {
            contexto = _contexto;
        }


    }
}
