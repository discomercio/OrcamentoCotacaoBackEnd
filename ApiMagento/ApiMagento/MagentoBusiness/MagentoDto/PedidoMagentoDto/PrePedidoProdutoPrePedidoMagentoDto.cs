using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PrePedidoProdutoPrePedidoMagentoDto
    {
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto

        [Required]
        public short Qtde { get; set; }

        [Required]
        public float Desc_Dado { get; set; }// = Desconto

        /// <summary>
        /// Preco_Venda = (Preco_Fabricante * CustoFinancFornecCoeficiente) * (1 - Desc_Dado / 100)
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }// = VlUnitario

        [Required]
        public decimal Preco_Fabricante { get; set; }

        /// <summary>
        /// Caso Preco_Lista seja igual a Preco_Venda, o RA será zero. Caso seja maior gerará um valor de RA (até o limite máximo de RA permitido).
        /// </summary>
        [Required]
        public decimal Preco_Lista { get; set; }// = VlLista

        /// <summary>
        /// Preco_NF = PrePedidoUnisDto.PermiteRAStatus == true ? Preco_Lista : Preco_Venda
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // se permite RA = Preco_Lista / senão VlUnitario

        /// <summary>
        /// Caso seja pagamento a vista, deve ser 1. Caso contrário, o coeficiente do fabricante para a quantidade de parcelas e forma de pagamento.
        /// </summary>
        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        /// <summary>
        /// CustoFinancFornecPrecoListaBase = Preco_Fabricante * CustoFinancFornecCoeficiente
        /// </summary>
        [Required]
        public decimal CustoFinancFornecPrecoListaBase { get; set; } //recebe Preco_Lista
    }
}
