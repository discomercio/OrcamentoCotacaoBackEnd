using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class InfCriacaoPedidoMagentoDto
    {
        /// <summary>
        /// CNPJ do indicador_orcamentista, campo installer_document do magento
        /// <hr />
        /// </summary>
        [Required]
        public string Cnpj_Cpf_Indicador_Orcamentista { get; set; }

        /// <summary>
        /// Verificar com Hamilton - este campo não é gravado no banco de dados?
        /// <hr />
        /// </summary>
        public string C_numero_magento { get; set; }

        /// <summary>
        /// Verificar com Hamilton - este campo é fixo?
        /// <hr />
        /// </summary>
        [MaxLength(3)]
        public string Marketplace_codigo_origem { get; set; }

        /// <summary>
        /// Verificar com Hamilton - precisamos deste campo?
        /// <hr />
        /// </summary>
        [MaxLength(20)]
        public string Pedido_bs_x_marketplace { get; set; }

        /// <summary>
        /// Verificar com Hamilton - precisamos deste campo?
        /// <hr />
        /// </summary>
        [MaxLength(9)]
        public string Pedido_bs_x_ac { get; set; }


        [Required]
        public decimal Magento_installer_commission_discount { get; set; }
        [Required]
        public decimal Magento_installer_commission_value { get; set; }
        [Required]
        public decimal Magento_shipping_amount { get; set; }
    }
}
