using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ESTOQUE_MOVIMENTO")]
    public class TestoqueMovimento
    {
        [Key]
        [Required]
        [Column("id_movimento")]
        [MaxLength(12)]
        public string Id_Movimento { get; set; }

        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("qtde")]
        public short? Qtde { get; set; }

        [Column("estoque")]
        [MaxLength(3)]
        public string Estoque { get; set; }

        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("anulado_status")]
        public short Anulado_Status { get; set; }
    }
}
