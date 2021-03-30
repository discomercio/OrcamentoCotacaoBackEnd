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
        public short Qtde { get; set; }

        // Verificar no magento qual o campo correto, possivelmente: original_price.
        /// <summary>
        /// Preco_bruto: preço de venda do item. 
        /// <br />É gravado em t_PEDIDO_ITEM_SERVICO.preco_lista
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_bruto { get; set; }

        // Verificar no magento como calcular este valor, possivelmente: row_total / campo qty_ordered.
        /// <summary>
        /// Preco_liquido: preço efetivamente cobrado, que seria impresso na nota fiscal. 
        /// <br />É gravado em t_PEDIDO_ITEM_SERVICO.preco_venda e t_PEDIDO_ITEM_SERVICO.preco_nf 
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_liquido { get; set; }

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
