using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class ProdutoValidadoComEstoqueDados
    {
        public ProdutoPedidoDados Produto{ get; set; }

        public List<string> ListaErros { get; set; }
    }
}
