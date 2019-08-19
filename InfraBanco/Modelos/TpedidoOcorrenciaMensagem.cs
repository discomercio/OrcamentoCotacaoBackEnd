using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_OCORRENCIA_MENSAGEM")]
    public class TpedidoOcorrenciaMensagem
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_ocorrencia")]
        [Required]
        //[ForeignKey("TpedidoOcorrencia")]
        public int Id_Ocorrencia { get; set; }

        [Column("usuario_cadastro")]
        [Required]
        [MaxLength(10)]
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

        [Column("fluxo_mensagem")]
        [Required]
        [MaxLength(6)]
        public string Fluxo_Mensagem { get; set; }

        [Column("texto_mensagem")]
        [Required]
        [MaxLength(1200)]
        public string Texto_Mensagem { get; set; }
    }
}