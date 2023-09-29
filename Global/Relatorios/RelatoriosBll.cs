using InfraBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relatorios
{
    public class RelatoriosBll
    {
        private RelatoriosData _data { get; set; }

        public RelatoriosBll(ContextoRelatorioProvider contextoProvider)
        {
            _data = new RelatoriosData(contextoProvider);
        }

        
    }
}
