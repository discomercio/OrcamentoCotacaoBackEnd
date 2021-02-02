using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo60.Gravacao.Grava60
{
    class ProdutoGravacao
    {
        public PedidoCriacaoProdutoDados Pedido { get; }
        public int Qtde_estoque_vendido = 0;
        public int Qtde_estoque_sem_presenca = 0;

        private ProdutoGravacao(PedidoCriacaoProdutoDados produto)
        {
            Pedido = produto;
        }

        public static List<ProdutoGravacao> ListaProdutoGravacao(List<PedidoCriacaoProdutoDados> pedidoCriacaoProdutos)
        {
            var ret = new List<ProdutoGravacao>();
            foreach (var p in pedidoCriacaoProdutos)
                ret.Add(new ProdutoGravacao(p));
            return ret;
        }
    }
}
