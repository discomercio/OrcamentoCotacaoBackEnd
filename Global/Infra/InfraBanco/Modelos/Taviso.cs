using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_AVISO")]
    public class Taviso
    {
        [Required]
        [Column("id")]
        [MaxLength(12)]
        [Key]
        public string Id { get; set; }

        [Column("mensagem")]
        public string Mensagem { get; set; }

        [Required]
        [Column("usuario")]
        [MaxLength(10)]
        public string Usuario { get; set; }

        [Column("destinatario")]
        [MaxLength(255)]
        public string Destinatario { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("timestamp")]
        public byte[] Timestamp { get; }
    }
}
