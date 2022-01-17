using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

namespace InfraBanco.Modelos
{
    [Table("t_USUARIO_X_LOJA")]
    public class TusuarioXLoja
    {
        [Column("usuario")]
        [MaxLength(10)]
        [Required]
        [ForeignKey("Tusuario")]
        public string Usuario { get; set; }

        [Column("loja")]
        [Key]
        [Required]
        [MaxLength(3)]
        [ForeignKey("Tloja")]
        public string Loja { get; set; }

        [Column("dt_cadastro")]
        public DateTime Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        public string Usuario_cadastro { get; set; }

        [Column("excluido_status")]
        public short? Excluido_Status { get; set; }

        [Column("timestamp")]
        public byte[]? Timestamp { get; set; }

        public Tusuario Tusuario { get; set; }
        public Tloja Tloja { get; set; }
    }
}
#endif

