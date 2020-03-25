using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ESTOQUE")]
    public class Testoque
    {
        [Key]
        [Required]
        [Column("id_estoque")]
        [MaxLength(12)]
        [ForeignKey("TestoqueItem")]
        public string Id_estoque { get; set; }

        //[Column("data_entrada")]
        //[Required]
        //public DateTime Data_entrada { get; set; }

        //[Column("hora_entrada")]
        //[Required]
        //[MaxLength(6)]
        //public string Hora_entrada { get; set; }

        //[Column("fabricante")]
        //[Required]
        //[MaxLength(4)]
        //public string Fabricante { get; set; }

        //[Column("documento")]
        //[MaxLength(30)]
        //public string Documento { get; set; }

        //[Column("usuario")]
        //[MaxLength(10)]
        //public string Usuario { get; set; }

        //[Column("data_ult_movimento")]
        //[Required]
        //public DateTime Data_ult_movimento { get; set; }

        //[Column("timestamp")]
        //public byte? Timestamp { get; set; }

        //[Column("kit")]
        //public short? Kit { get; set; }

        //[Column("entrada_especial")]
        //public short? Entrada_especial { get; set; }

        //[Column("devolucao_status")]
        //public short? Devolucao_status { get; set; }

        //[Column("devolucao_data")]
        //public DateTime? Devolucao_data { get; set; }

        //[Column("devolucao_hora")]
        //[MaxLength(6)]
        //public string Devolucao_hora { get; set; }

        //[Column("devolucao_usuario")]
        //[MaxLength(10)]
        //public string Devolucao_usuario { get; set; }

        //[Column("devolucao_loja")]
        //[MaxLength(3)]
        //public string Devolucao_loja { get; set; }

        //[Column("devolucao_pedido")]
        //[MaxLength(9)]
        //public string Devolucao_pedido { get; set; }

        //[Column("devolucao_id_item_devolvido")]
        //[MaxLength(12)]
        //public string Devolucao_id_item_devolvido { get; set; }

        //[Column("devolucao_id_estoque")]
        //[MaxLength(12)]
        //public string Devolucao_id_estoque { get; set; }

        //[Column("obs")]
        //[MaxLength(500)]
        //public string Obs { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_nfe_emitente { get; set; }

        //[Column("entrada_tipo")]
        //public short? Entrada_tipo { get; set; }

        //[Column("perc_agio")]
        //public float? Perc_agio { get; set; }

        public TestoqueItem TestoqueItem { get; set; }
    }
}
