using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Cep
{
    public class CepDados
    {
        public string Cep { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string LogradouroComplemento { get; set; }
        public List<string> ListaCidadeIBGE { get; set; }
    }
}
