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

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; }
    }
}
