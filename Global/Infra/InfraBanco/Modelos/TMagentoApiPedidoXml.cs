using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

namespace InfraBanco.Modelos
{
    [Table("t_MAGENTO_API_PEDIDO_XML")]
    public class TMagentoApiPedidoXml
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("pedido_magento")]
        [MaxLength(9)]
        [Required]
        public string Pedido_magento { get; set; }

        [Column("pedido_marketplace")]
        [MaxLength(20)]
        public string Pedido_marketplace { get; set; }

        [Column("marketplace_codigo_origem")]
        [MaxLength(3)]
        public string Marketplace_codigo_origem { get; set; }
    }
}
#endif