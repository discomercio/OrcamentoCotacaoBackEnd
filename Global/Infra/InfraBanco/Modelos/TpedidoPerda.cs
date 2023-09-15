using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_PERDA")]
    public class TpedidoPerda
    {
        [Key]
        [Column("id")]
        [MaxLength(12)]
        public string Id { get; set; }

        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [Column("hora")]
        [MaxLength(6)]
        public string Hora { get; set; }

        [Column("valor", TypeName = "money")]
        public decimal Valor { get; set; }

        [Column("obs")]
        [MaxLength(80)]
        public string Obs { get; set; }

        [Column("comissao_descontada")]
        public short ComissaoDescontada { get; set; }
    }
}
