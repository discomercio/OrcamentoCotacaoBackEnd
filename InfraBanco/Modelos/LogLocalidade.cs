using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("LOG_LOCALIDADE")]
    public class LogLocalidade
    {
        [Key]
        [Required]
        [Column("LOC_NU_SEQUENCIAL")]
        public int Loc_nu_sequencial { get; set; }

        [Column("LOC_NOSUB")]
        [MaxLength(50)]
        public string Loc_nosub { get; set; }
        
        [Column("UFE_SG")]
        [MaxLength(2)]
        public string Ufe_sg { get; set; }

        [Column("CEP_DIG")]
        [MaxLength(8)]
        public string Cep_dig { get; set; }
    }
}
