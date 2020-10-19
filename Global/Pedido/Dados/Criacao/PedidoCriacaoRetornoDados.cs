using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoRetornoDados
    {
        public string? Id { get; set; }
        public List<string> ListaIdPedidosFilhotes { get; set; } = new List<string>();
        public List<string> ListaErros { get; set; } = new List<string>();
        public List<string> ListaErrosValidacao { get; set; } = new List<string>();
    }
}
