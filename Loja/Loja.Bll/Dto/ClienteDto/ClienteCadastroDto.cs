using Loja.Bll.Dto.ClienteDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ClienteDto
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public List<RefBancariaDtoCliente> RefBancaria { get; set; }
        public List<RefComercialDtoCliente> RefComercial { get; set; }

    }
}
