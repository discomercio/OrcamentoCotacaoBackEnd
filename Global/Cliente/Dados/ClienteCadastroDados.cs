using Cliente.Dados.Referencias;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cliente.Dados
{
    public class ClienteCadastroDados
    {
        public DadosClienteCadastroDados DadosCliente { get; set; }
        public List<RefBancariaClienteDados> RefBancaria { get; set; }
        public List<RefComercialClienteDados> RefComercial { get; set; }
    }
}
