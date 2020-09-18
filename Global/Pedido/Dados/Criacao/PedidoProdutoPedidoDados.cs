using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoProdutoPedidoDados
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? CustoFinancFornecPrecoListaBase { get; set; }
        public decimal Preco_NF { get; set; }
        public decimal Preco_Lista { get; set; }
        public float? Desc_Dado { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal TotalItem { get; set; }
        public decimal? TotalItemRA { get; set; }
        public float? Comissao { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public string Alertas { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }
    }
}
