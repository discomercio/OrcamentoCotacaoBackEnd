using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class BlocoNotasDevolucaoMercadoriasDtoPedido
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Mensagem { get; set; }
    }
}
