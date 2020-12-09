using System;
using System.Collections.Generic;
using System.Text;
using Prepedido.Dados.DetalhesPrepedido;

//todo: religar nullable
#nullable disable

namespace Pedido.Dados.Criacao
{
    public class PedidoProdutoPedidoDados
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public short Qtde { get; set; }
        public decimal CustoFinancFornecPrecoListaBase { get; set; }
        public decimal Preco_NF { get; set; }
        public decimal Preco_Lista { get; set; }
        public float? Desc_Dado { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal TotalItem { get; set; }
        public decimal? TotalItemRA { get; set; }
        public float? Comissao { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }

        public static List<PrepedidoProdutoPrepedidoDados> PrepedidoProdutoPrepedidoDadosDePedidoProdutoPedidoDados(List<PedidoProdutoPedidoDados> lstProdutoPedido)
        {

            List<PrepedidoProdutoPrepedidoDados> lstPrepedidoProduto = new List<PrepedidoProdutoPrepedidoDados>();

            foreach (var x in lstProdutoPedido)
            {
                PrepedidoProdutoPrepedidoDados produtoPrepedido = new PrepedidoProdutoPrepedidoDados();
                produtoPrepedido.Fabricante = x.Fabricante;
                produtoPrepedido.Produto = x.Produto;
                produtoPrepedido.CustoFinancFornecPrecoListaBase = x.CustoFinancFornecPrecoListaBase;
                produtoPrepedido.CustoFinancFornecCoeficiente = x.CustoFinancFornecCoeficiente;
                produtoPrepedido.Preco_Lista = x.Preco_Lista;
                produtoPrepedido.Preco_Venda = x.Preco_Venda;
                produtoPrepedido.Preco_NF = x.Preco_NF;
                produtoPrepedido.Qtde = x.Qtde;
                produtoPrepedido.TotalItem = x.TotalItem;
                produtoPrepedido.TotalItemRA = x.TotalItemRA ?? 0;
                lstPrepedidoProduto.Add(produtoPrepedido);
            }


            return lstPrepedidoProduto;

        }
    }
}
