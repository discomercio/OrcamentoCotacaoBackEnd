using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_PAGAMENTO")]
    public class TpedidoPagamento
    {
        [Key]
        [MaxLength(9)]
        [Required]
        [Column("id")]
        public string Id { get; set; }

        [Column("pedido")]
        [MaxLength(9)]
        [Required]
        public string Pedido { get; set; }

        //[Column("data")]
        //[Required]
        //public DateTime Data { get; set; }

        //[Column("hora")]
        //[Required]
        //[MaxLength(6)]
        //public string Hora { get; set; }

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; }

        //[Column("tipo_pagto")]
        //[Required]
        //[MaxLength(1)]
        //public string Tipo_Pagto { get; set; }

        //[Column("usuario")]
        //[MaxLength(10)]
        //public string Usuario { get; set; }

        //[Column("timestamp")]
        //[MaxLength]
        //public byte[] Timestamp { get; set; }

        //[Column("id_pedido_pagto_cielo")]
        //[Required]
        //public int Id_Pedido_Pagto_Cielo { get; set; }

        //[Column("id_pedido_pagto_braspag")]
        //[Required]
        //public int Id_Pedido_Pagto_Braspag { get; set; }

        //[Column("id_pagto_gw_pag_payment")]
        //[Required]
        //public int Id_Pagto_Gw_Pag_Payment { get; set; }

        //[Column("id_braspag_webhook_complementar")]
        //[Required]
        //public int Id_Braspag_Webhook_Complementar { get; set; }

        //[Column("pedido_marketplace")]
        //[MaxLength(20)]
        //public string Pedido_Marketplace { get; set; }
    }
}
