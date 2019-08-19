using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_BLOCOS_NOTAS")]
    public class TpedidoBlocosNotas
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        //[ForeignKey("Tpedido")]
        public string Pedido { get; set; }

        [Column("usuario")]
        [Required]
        [MaxLength(10)]
        //[ForeignKey("Tusuario")]
        public string Usuario { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_Hr_Cadastro { get; set; }

        [Column("nivel_acesso")]
        [Required]
        public short Nivel_Acesso { get; set; }

        [Column("mensagem")]
        [Required]
        [MaxLength(4000)]
        public string Mensagem { get; set; }

        [Column("anulado_status")]
        [Required]
        public short Anulado_Status { get; set; }

        [Column("anulado_usuario")]
        [MaxLength(10)]
        public string Anulado_Usuario { get; set; }

        [Column("anulado_data")]
        public DateTime? Anulado_Data { get; set; }

        [Column("anulado_data_hora")]
        public DateTime? Anulado_Data_Hora { get; set; }


    }
}