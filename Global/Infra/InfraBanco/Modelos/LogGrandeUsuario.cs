using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("LOG_GRANDE_USUARIO")]
    public class LogGrandeUsuario
    {
        [Column("GRU_NU_SEQUENCIAL")]
        [Required]
        [Key]
        public int Gru_nu_sequencial { get; set; }

        [Column("UFE_SG")]
        [MaxLength(2)]
        [Required]
        public string Ufe_sg { get; set; }

        [Column("LOC_NU_SEQUENCIAL")]
        [Required]
        public int Loc_nu_sequencial { get; set; }

        [Column("LOG_NU_SEQUENCIAL")]
        public int? Log_nu_sequencial { get; set; }

        [Column("BAI_NU_SEQUENCIAL")]
        [Required]
        public int Bai_nu_sequencial { get; set; }

        [Column("GRU_NO")]
        [MaxLength(96)]
        [Required]
        public string Ggru_no { get; set; }

        [Column("CEP")]
        [MaxLength(16)]
        [Required]
        public string Cep { get; set; }

        [Column("GRU_ENDERECO")]
        [MaxLength(200)]
        [Required]
        public string Gru_endereco { get; set; }

        [Column("GRU_KEY_DNE")]
        [MaxLength(16)]
        [Required]
        public string Gru_key_dne { get; set; }

        [Column("TEMP")]
        [MaxLength(8)]
        [Required]
        public string Temp { get; set; }

        [Column("CEP_DIG")]
        [MaxLength(8)]
        [Required]
        public string Cep_dig { get; set; }
    }
}
