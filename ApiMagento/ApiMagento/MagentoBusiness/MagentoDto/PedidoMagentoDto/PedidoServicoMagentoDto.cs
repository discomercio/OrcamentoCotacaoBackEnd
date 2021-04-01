using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoServicoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>
        /// SKU: código do serviço
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string SKU { get; set; } //Produto

        /// <summary>
        /// Campo name
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(120)]
        public string Descricao { get; set; }

        /// <summary>
        /// Campo qty_ordered
        /// <hr />
        /// </summary>
        [Required]
        public short Quantidade { get; set; }

        /// <summary>
        /// Subtotal: valor total do serviço sem desconto aplicado
        /// <hr />
        /// </summary>
        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public float TaxAmount { get; set; }

        /// <summary>
        /// DiscountAmount: valor de desconto
        /// <hr />
        /// </summary>
        [Required]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// RowTotal> valor total do serviço com desconto aplicado
        /// <hr />
        /// </summary>
        [Required]
        public decimal RowTotal { get; set; }

        /* DETALHES DE COMO FAZER OS CÁLCULOS
         * ==================================
         * t_PEDIDO_ITEM_SERVICO.preco_venda e t_PEDIDO_ITEM_SERVICO.preco_nf = RowTotal / Quantidade
         * t_PEDIDO_ITEM_SERVICO.preco_lista = Subtotal / Quantidade
         * t_PEDIDO_ITEM_SERVICO.desc_dado = 100 * (preco_lista - preco_venda) / preco_lista
         */

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
