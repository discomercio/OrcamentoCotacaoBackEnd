using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArclubePrepedidosWebapi.Dtos.ClienteCadastro
{
    public class EnderecoEntregaDtoClienteCadastro
    {
        public string EnderecoEntrega { get; set; }
        public int NumeroEntrega { get; set; }
        public string ComplementoEntrega { get; set; }
        public string BairroEntrega { get; set; }
        public string CidadeEntrega { get; set; }
        public string UfEntrega { get; set; }
        public string CepEntrega { get; set; }
        public string JustificaEntrega { get; set; }
    }
}