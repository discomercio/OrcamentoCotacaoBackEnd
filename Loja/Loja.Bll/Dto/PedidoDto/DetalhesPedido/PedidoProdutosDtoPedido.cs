using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class PedidoProdutosDtoPedido
    {
        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? Preco { get; set; }
        public decimal? Preco_Lista { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public decimal? VlVenda { get; set; }
        public decimal? VlTotal { get; set; }
        public float? Comissao { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public string Alertas { get; set; }


        ////acertar os nomes de variáveis para usar estes
        //public decimal CustoFinancFornecPrecoListaBase { get; set; }
        //public decimal Preco_NF { get; set; }
        //public float? Desc_Dado { get; set; }
        //public decimal Preco_Venda { get; set; }
        //public decimal TotalItem { get; set; }
        //public decimal? TotalItemRA { get; set; }
        //public float CustoFinancFornecCoeficiente { get; set; }


        public static List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> List_PedidoProdutoPedidoDados_De_PedidoProdutosDtoPedido(List<PedidoProdutosDtoPedido> origemLista)
        {
            if (origemLista == null) return null;
            var ret = new List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados>(0);
            foreach (var origem in origemLista)
            {
                ret.Add(new Pedido.Dados.Criacao.PedidoProdutoPedidoDados()
                {
                    Fabricante = origem.Fabricante,
                    Produto = origem.NumProduto,
                    Descricao = origem.Descricao,
                    Qtde = origem.Qtde ?? 0,
                    Qtde_estoque_total_disponivel = origem.Qtde_estoque_total_disponivel,
                    Preco_Lista = origem.Preco_Lista ?? 0,
                    Comissao = origem.Comissao,

                    CustoFinancFornecPrecoListaBase = origem.Preco_Lista ?? 0,
                    Preco_NF = origem.Preco ?? 0,
                    Desc_Dado = origem.Desconto,
                    Preco_Venda = origem.Preco ?? 0,
                    TotalItem = origem.VlTotalItem ?? 0,
                    TotalItemRA = origem.VlTotalItemComRA,
                    CustoFinancFornecCoeficiente = 0

                    corrigir:
                    Fabricante = origem.Fabricante,
                    NumProduto = origem.Produto,
                    Descricao = origem.Descricao,
                    Qtde = origem.Qtde,
                    Faltando = origem.Faltando,
                    CorFaltante = origem.CorFaltante,
                    Preco_Lista = origem.Preco_NF,
                    VlLista = origem.Preco_Lista,
                    Desconto = origem.Desc_Dado,
                    VlVenda = origem.Preco_Venda ?? 0m,
                    VlTotalItem = origem.VlTotalItem,
                    VlTotalItemComRA = origem.VlTotalItemComRA,
                    Comissao = origem.Comissao

                    //acertar as variáveis novas
                    //CustoFinancFornecPrecoListaBase = origem.CustoFinancFornecPrecoListaBase,
                    //Preco_NF = origem.Preco,
                    //Desc_Dado = origem.Desc_Dado,
                    //Preco_Venda = origem.Preco_Venda,
                    //TotalItem = origem.TotalItem,
                    //TotalItemRA = origem.TotalItemRA,
                    //CustoFinancFornecCoeficiente = origem.CustoFinancFornecCoeficiente
                });
            }
            return ret;
        }

        public static List<PedidoProdutosDtoPedido> ListaPedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(IEnumerable<PedidoProdutosPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoProdutosDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(p));
            return ret;
        }
        public static PedidoProdutosDtoPedido PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(PedidoProdutosPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoProdutosDtoPedido()
            {
                Fabricante = origem.Fabricante,
                NumProduto = origem.Produto,
                Descricao = origem.Descricao,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco_Lista = origem.Preco_NF,
                VlLista = origem.Preco_Lista,
                Desconto = origem.Desc_Dado,
                VlVenda = origem.Preco_Venda ?? 0m,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,
                Comissao = origem.Comissao
            };
        }
    }
}
