using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class PrepedidoProdutoPrepedidoDados
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }

        //preço base 
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        //preço base com coeficiente
        public decimal Preco_Lista { get; set; }

        //preço calculado com coeficiente e desconto
        public decimal Preco_Venda { get; set; }

        //preço da nota fiscal. Se orçamentista não permite RA, recebe "Preco_Venda".
        public decimal Preco_NF { get; set; }

        //incluimos esse campos apenas para validar o que esta sendo enviado pela API da Unis
        public float CustoFinancFornecCoeficiente { get; set; }
        public float Desc_Dado { get; set; }        
        public decimal VlTotalItem { get; set; }
        public decimal VlTotalRA { get; set; }
        public float? Comissao { get; set; }
        public decimal TotalItemRA { get; set; }
        public decimal TotalItem { get; set; }
        public string Obs { get; set; }

        //campos "Permite_Ra_Status" e "BlnTemRa" 
        //não devem ser uteis nos produtos, pois o Permite RA é com base no Orçamentista/Indicador
        public short Permite_Ra_Status { get; set; }
        public bool BlnTemRa { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
    }
}
