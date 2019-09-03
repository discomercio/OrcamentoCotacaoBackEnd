using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.ClienteCadastro
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public RefBancariaDtoCliente RefBancaria { get; set; }
        public RefComercialDtoCliente RefComercial { get; set; }
    }
}