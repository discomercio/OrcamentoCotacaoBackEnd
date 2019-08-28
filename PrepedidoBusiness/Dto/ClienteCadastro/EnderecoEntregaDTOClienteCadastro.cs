using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.ClienteCadastro
{
    public class EnderecoEntregaDtoClienteCadastro
    {
        public string EndEtg_endereco { get; set; }
        public string EndEtg_endereco_numero { get; set; }
        public string EndEtg_endereco_complemento { get; set; }
        public string EndEtg_bairro { get; set; }
        public string EndEtg_cidade { get; set; }
        public string EndEtg_uf { get; set; }
        public string EndEtg_cep { get; set; }
        public string EndEtg_cod_justificativa { get; set; }
    }
}