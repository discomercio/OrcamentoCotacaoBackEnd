using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("LOG_BAIRRO")]
    public class LogBairro
    {
        [Key]
        [Required]
        [Column("BAI_NU_SEQUENCIAL")]
        public int Bai_nu_sequencial { get; set; }

        [Column("BAI_NO")]
        [Required]
        [MaxLength(72)]
        public string Bai_no { get; set; }
    }
}
