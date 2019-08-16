using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ArclubePrepedidosWebapi.Models
{
    [Table("t_PEDIDO_OCORRENCIA")]
    public class TpedidoOcorrencia
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("pedido")]
        //[ForeignKey("Tpedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        //[ForeignKey("Tusuario")]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_Hr_Cadastro { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("contato")]
        [MaxLength(60)]
        public string Contato { get; set; }

        [Column("ddd_1")]
        [MaxLength(2)]
        public string Ddd_1 { get; set; }

        [Column("tel_1")]
        [MaxLength(9)]
        public string Tel_1 { get; set; }

        [Column("ddd_2")]
        [MaxLength(2)]
        public string Ddd_2 { get; set; }

        [Column("tel_2")]
        [MaxLength(9)]
        public string Tel_2 { get; set; }

        [Column("tipo_ocorrencia")]
        [MaxLength(3)]
        public string Tipo_Ocorrencia { get; set; }

        [Column("finalizado_status")]
        [Required]
        public byte Finalizado_Status { get; set; }

        [Column("finalizado_usuario")]
        [MaxLength(10)]
        public string Finalizado_Usuario { get; set; }

        [Column("finalizado_data")]
        public DateTime? Finalizado_Data { get; set; }

        [Column("finalizado_data_hora")]
        public DateTime? Finalizado_Data_Hora { get; set; }

        [Column("texto_ocorrencia")]
        [MaxLength(800)]
        public string Texto_Ocorrencia { get; set; }

        [Column("texto_finalizacao")]
        [MaxLength(800)]
        public string Texto_Finalizacao { get; set; }

        [Column("cod_motivo_abertura")]
        [MaxLength(3)]
        public string Cod_Motivo_Abertura { get; set; }

    }
}