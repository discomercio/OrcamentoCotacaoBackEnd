using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Execucao
{
    public class ProdutoGravacao
    {
        public PedidoCriacaoProdutoDados Pedido { get; }
        public int Qtde_estoque_vendido = 0;
        public int Qtde_estoque_sem_presenca = 0;
        public bool Abaixo_min_status = false;
        public string? Abaixo_min_superv_autorizador = null;
        public string? Abaixo_min_autorizador = null;
        public string? Abaixo_min_autorizacao = null;

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
