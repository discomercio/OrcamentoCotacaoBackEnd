using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produto.Dados
{
    public class PropriedadesDados
    {
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }
        public int Pagina { get; set; }
        public int QtdeItensPorPagina { get; set; }
        public bool OrdenacaoAscendente { get; set; }
        public string NomeColunaOrdenacao { get; set; }
    }
}
