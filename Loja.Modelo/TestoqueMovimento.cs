using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_ESTOQUE_MOVIMENTO")]
    public class TestoqueMovimento
    {
        [Key]
        [Required]
        [Column("id_movimento")]
        [MaxLength(12)]
        public string Id_Movimento { get; set; }

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

        [Column("operacao")]
        [MaxLength(3)]
        public string Operacao { get; set; }

        [Column("estoque")]
        [MaxLength(3)]
        public string Estoque { get; set; }

        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("anulado_status")]
        public short Anulado_Status { get; set; }

        [Column("anulado_data")]
        public DateTime? Anulado_Data { get; set; }

        [Column("anulado_hora")]
        [MaxLength(6)]
        public string Anulado_Hora { get; set; }

        [Column("anulado_usuario")]
        [MaxLength(10)]
        public string Anulado_Usuario { get; set; }

        [Column("timestamp")]
        [MaxLength]
        public byte[]? Timestamp { get; }

        [Column("kit")]
        public short Kit { get; set; }

        [Column("kit_id_estoque")]
        [MaxLength(12)]
        public string Kit_Id_Estoque { get; set; }

        [Column("id_ordem_servico")]
        [MaxLength(12)]
        public string Id_Ordem_Servico { get; set; }
    }
}
