using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("LOG_BAIRRO")]
    public class LogBairro
    {
        [Key]
        [Required]
        [Column("BAI_NU_SEQUENCIAL")]
        public int Bai_nu_sequencial { get; set; }

        [Column("UFE_SG")]
        [Required]
        [MaxLength(2)]
        public string Ufe_sg { get; set; }

        [Column("LOC_NU_SEQUENCIAL")]
        [Required]
        public int Loc_nu_sequencial { get; set; }

        [Column("BAI_NO")]
        [Required]
        [MaxLength(72)]
        public string Bai_no { get; set; }

        [Column("BAI_NO_ABREV")]
        [MaxLength(36)]
        public string Bai_no_abrev { get; set; }
    }
}
