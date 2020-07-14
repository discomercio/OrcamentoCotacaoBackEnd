using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
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

        [Column("LOG_NOME")]
        [Required]
        [MaxLength(125)]
        public string Log_nome { get; set; }

        [Column("BAI_NU_SEQUENCIAL_INI")]
        [Required]
        public int Bai_nu_sequencial_ini { get; set; }

        [Column("BAI_NU_SEQUENCIAL_FIM")]
        public int? Bai_nu_sequencial_fim { get; set; }

        [Column("CEP")]
        [Required]
        [MaxLength(16)]
        public string Cep { get; set; }

        [Column("LOG_COMPLEMENTO")]
        [MaxLength(100)]
        public string Log_complemento { get; set; }

        [Column("LOG_TIPO_LOGRADOURO")]
        [MaxLength(72)]
        public string Log_tipo_logradouro { get; set; }

        [Column("LOG_STATUS_TIPO_LOG")]
        [Required]
        [MaxLength(1)]
        public string Log_status_tipo_log { get; set; }

        [Column("LOG_NO_SEM_ACENTO")]
        [Required]
        [MaxLength(70)]
        public string Log_no_sem_acento { get; set; }

        [Column("LOG_KEY_DNE")]
        [MaxLength(16)]
        public string Log_key_dne { get; set; }

        [Column("IND_UOP")]
        [MaxLength(1)]
        public string Ind_uop { get; set; }

        [Column("IND_GRU")]
        [MaxLength(1)]
        public string Ind_gru { get; set; }

        [Column("TEMP")]
        [MaxLength(8)]
        public string Temp { get; set; }

        [Column("CEP_DIG")]
        [MaxLength(8)]
        public string Cep_dig { get; set; }
    }
}
