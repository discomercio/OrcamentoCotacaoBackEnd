using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dto.ClienteCadastro
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public List<RefBancariaDtoCliente> RefBancaria { get; set; }
        public List<RefComercialDtoCliente> RefComercial { get; set; }
    }
}