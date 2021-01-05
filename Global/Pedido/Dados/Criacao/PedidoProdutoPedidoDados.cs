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
        public short Qtde { get; set; }
        public float? Desc_Dado { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal Preco_Lista { get; set; }
        public decimal Preco_NF { get; set; }

        //estes campos são usados somente para conferência
        public decimal CustoFinancFornecPrecoListaBase_Conferencia { get; set; }
        public float CustoFinancFornecCoeficiente_Conferencia { get; set; }

        //usado para avisar se mudou o número de produtos não disponíveis em estoque
        //o usuário concordou em fazer o pedido com X unidades em estoque; 
        //se durante o processo outro pedido consumir esse estoque, devemos avisar o cliente que mudou a quantidade de produtos disponíveis para entrega
        public short? Qtde_estoque_total_disponivel { get; set; }

        //informações
        public decimal TotalItem()
        {
            return Math.Round((Preco_Venda * Qtde), 2);
        }
        public decimal? TotalItemRA()
        {
            return Math.Round((Preco_NF * Qtde), 2);
        }


        public static List<PrepedidoProdutoPrepedidoDados> PrepedidoProdutoPrepedidoDadosDePedidoProdutoPedidoDados(List<PedidoProdutoPedidoDados> lstProdutoPedido)
        {

            List<PrepedidoProdutoPrepedidoDados> lstPrepedidoProduto = new List<PrepedidoProdutoPrepedidoDados>();

            foreach (var x in lstProdutoPedido)
            {
                PrepedidoProdutoPrepedidoDados produtoPrepedido = new PrepedidoProdutoPrepedidoDados();
                produtoPrepedido.Fabricante = x.Fabricante;
                produtoPrepedido.Produto = x.Produto;
                produtoPrepedido.CustoFinancFornecPrecoListaBase = x.CustoFinancFornecPrecoListaBase_Conferencia;
                produtoPrepedido.CustoFinancFornecCoeficiente = x.CustoFinancFornecCoeficiente_Conferencia;
                produtoPrepedido.Preco_Lista = x.Preco_Lista;
                produtoPrepedido.Preco_Venda = x.Preco_Venda;
                produtoPrepedido.Preco_NF = x.Preco_NF;
                produtoPrepedido.Qtde = x.Qtde;
                produtoPrepedido.TotalItem = x.TotalItem();
                produtoPrepedido.TotalItemRA = x.TotalItemRA() ?? 0;
                lstPrepedidoProduto.Add(produtoPrepedido);
            }


            return lstPrepedidoProduto;

        }
    }
}
