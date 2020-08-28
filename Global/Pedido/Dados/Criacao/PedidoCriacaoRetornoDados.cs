using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoRetornoDados
    {
        public string Id { get; set; }
        public List<string> ListaIdPedidosFilhotes { get; set; }
        public List<string> ListaErros { get; set; }
        public List<string> ListaErrosValidacao { get; set; }
    }
}
