using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoRetornoDados
    {
        public string? Id { get; set; }
        public List<string> ListaIdPedidosFilhotes { get; private set; } = new List<string>();
        public List<string> ListaErros { get; } = new List<string>();
        public List<string> ListaErrosValidacao { get; } = new List<string>();

        public void RemoverPedidos()
        {
            Id = null;
            ListaIdPedidosFilhotes = new List<string>();
        }

        public bool AlgumErro()
        {
            if (ListaErros.Count > 0)
                return true;
            if (ListaErrosValidacao.Count > 0)
                return true;
            return false;
        }
    }
}
