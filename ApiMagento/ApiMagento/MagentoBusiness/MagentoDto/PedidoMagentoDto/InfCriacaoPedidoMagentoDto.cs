using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class InfCriacaoPedidoMagentoDto
    {
        /// <summary>
        /// Este é o número do pedido no magento (no ASP, é C_numero_magento)
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string Pedido_bs_x_ac { get; set; }


        /// <summary>
        /// Código obtido através da chamada ??????ainda não definido
        /// Somente quando for um pedido de marketplace
        /// <hr />
        /// </summary>
        [MaxLength(3)]
        public string Marketplace_codigo_origem { get; set; }

        /// <summary>
        /// Número do pedido no marketplace (opcional, se o pedido é do magento este campo não existe)
        /// <hr />
        /// </summary>
        [MaxLength(20)]
        public string Pedido_bs_x_marketplace { get; set; }
    }
}
