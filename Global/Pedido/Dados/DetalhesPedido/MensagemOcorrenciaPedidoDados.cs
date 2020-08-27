using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.DetalhesPedido
{
    public class MensagemOcorrenciaPedidoDados
    {
        public DateTime Dt_Hr_Cadastro { get; set; }
        public string Usuario { get; set; }
        public string Loja { get; set; }
        public string Texto_Mensagem { get; set; }
    }
}
