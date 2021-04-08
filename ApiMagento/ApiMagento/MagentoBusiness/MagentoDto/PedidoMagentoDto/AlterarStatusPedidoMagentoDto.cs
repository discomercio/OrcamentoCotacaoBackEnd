using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class AlterarStatusPedidoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        public string TokenAcesso { get; set; }

        /// <summary>
        /// Este é o número do pedido no magento (no ASP, é C_numero_magento; precisa ter 9 dígitos)
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string Pedido_magento { get; set; }

        [Required]
        public StatusPedidoMagentoDto StatusPedido { get; set; }

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

    }
}
