using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArclubePrepedidosWebapi.Dtos.ClienteCadastro
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoCliente { get; set; }
    }
}