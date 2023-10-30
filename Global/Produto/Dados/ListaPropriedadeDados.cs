using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsGlobais.RequestResponse;

namespace Produto.Dados
{
    public class ListaPropriedadeDados: RequestBase
    {
        public List<ProdutoCatalogoPropriedadeDados> ListaPropriedade { get; set; }
        public int QtdeRegistros { get; set; }
    }
}
