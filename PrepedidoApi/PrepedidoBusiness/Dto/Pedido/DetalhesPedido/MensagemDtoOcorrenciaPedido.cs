using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class MensagemDtoOcorrenciaPedido
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Texto_Mensagem { get; set; }
    }
}
