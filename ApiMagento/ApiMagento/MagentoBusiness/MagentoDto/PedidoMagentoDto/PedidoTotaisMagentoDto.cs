using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    /// <summary>
    /// TotaisPedido: armazena os valores totais do pedido
    /// <br />
    /// Os campo estão com os mesmos nomes exibidos no Painel Admin na consulta do Pedido
    /// <hr />
    /// </summary>
    public class PedidoTotaisMagentoDto
    {
        /// <summary>
        /// Subtotal: o valor total incluindo o serviço
        /// <hr />
        /// </summary>
        [Required]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// DiscountAmount: valor de desconto no pedido incluindo o desconto no frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// BSellerInterest: nos casos em que seja diferente de zero, o pedido deve ser processado manualmente.
        /// <hr />
        /// </summary>
        [Required]
        public decimal BSellerInterest { get; set; }

        /// <summary>
        /// FreteBruto: valor bruto do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal? FreteBruto { get; set; }

        /// <summary>
        /// DescontoFrete: valor de desconto no frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal DescontoFrete { get; set; }

        [Required]
        public decimal GrandTotal { get; set; }
    }
}
