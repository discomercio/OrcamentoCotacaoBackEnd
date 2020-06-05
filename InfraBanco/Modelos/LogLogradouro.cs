using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("LOG_LOGRADOURO")]
    public class LogLogradouro
    {
        [Key]
        [Required]
        [Column("LOG_NU_SEQUENCIAL")]
        public int Log_nu_sequencial { get; set; }

        [Column("UFE_SG")]
        [Required]
        [MaxLength(2)]
        public string Ufe_sg { get; set; }

        [Column("LOC_NU_SEQUENCIAL")]
        [Required]
        public int Loc_nu_sequencial { get; set; }

        [Column("LOG_NO")]
        [Required]
        [MaxLength(70)]
        public string Log_no { get; set; }

        [Column("BAI_NU_SEQUENCIAL_INI")]
        [Required]
        public int Bai_nu_sequencial_ini { get; set; }

        [Column("LOG_COMPLEMENTO")]
        [MaxLength(100)]
        public string Log_complemento { get; set; }

        [Column("LOG_TIPO_LOGRADOURO")]
        [MaxLength(72)]
        public string Log_tipo_logradouro { get; set; }

        [Column("CEP_DIG")]
        [MaxLength(8)]
        public string Cep_dig { get; set; }
    }
}
