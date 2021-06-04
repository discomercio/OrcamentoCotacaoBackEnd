using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class InfCriacaoPedidoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>
        /// Este é o número do pedido no magento (no ASP, é C_numero_magento; precisa ter 9 dígitos)
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string Pedido_magento { get; set; }
public string afazer_Pedido_bs_x_ac { get; set; }

        /// <summary>
        /// Código obtido através da chamada obterCodigoMarketplace, campo Codigo (uma string de 3 dígitos)
        /// <br />
        /// Somente quando for um pedido de marketplace
        /// <hr />
        /// </summary>
        [MaxLength(3)]
        public string? Marketplace_codigo_origem { get; set; }

        /// <summary>
        /// Número do pedido no marketplace (opcional, se o pedido é do magento este campo não existe)
        /// <hr />
        /// </summary>
        [MaxLength(20)]
        public string? Pedido_marketplace { get; set; }
public string? afazer_Pedido_bs_x_marketplace { get; set; }
    }
}
