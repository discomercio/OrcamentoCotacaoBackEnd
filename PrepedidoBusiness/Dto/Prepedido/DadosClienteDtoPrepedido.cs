using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.Prepedido
{
    public class DadosClienteDtoPrepedido
    {
        public string Loja { get; set; }
        public string Orcamentista { get; set; }
        public string Vendedor { get; set; }
        public string Cnpj_Cpf { get; set; }
        public string Rg { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelComercial { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string EnderecoEntrega { get; set; }

    }
}