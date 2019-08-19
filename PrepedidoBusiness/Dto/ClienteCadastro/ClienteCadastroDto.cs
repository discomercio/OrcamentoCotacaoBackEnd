using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.ClienteCadastro
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoCliente { get; set; }
    }
}