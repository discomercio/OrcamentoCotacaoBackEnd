using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class ProdutoValidadoComEstoqueDados
    {
        public ProdutoValidadoComEstoqueDados(ProdutoPedidoDados produto, List<string> listaErros)
        {
            Produto = produto;
            ListaErros = listaErros;
        }

        public ProdutoPedidoDados Produto{ get; set; }

        public List<string> ListaErros { get; set; }
    }
}
