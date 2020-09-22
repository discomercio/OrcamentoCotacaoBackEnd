﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

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

        [Column("data")]
        [Required]
        public DateTime Data { get; set; }

        [Column("hora")]
        [MaxLength(6)]
        public string Hora { get; set; }

        [Column("usuario")]
        [MaxLength(10)]
        public string Usuario { get; set; }

        [Column("id_estoque")]
        [MaxLength(12)]
        [Required]
        public string Id_Estoque { get; set; }

        [Column("operacao")]
        [MaxLength(3)]
        public string Operacao { get; set; }

        [Column("kit")]
        public short Kit { get; set; }
    }
}
#endif