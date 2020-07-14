using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
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

        [Column("LOC_NO")]
        [MaxLength(60)]
        public string Loc_no { get; set; }

        [Column("CEP")]
        [MaxLength(16)]
        public string Cep { get; set; }

        [Column("UFE_SG")]
        [MaxLength(2)]
        public string Ufe_sg { get; set; }

        [Column("LOC_IN_SITUACAO")]
        public int Loc_in_situacao { get; set; }

        [Column("LOC_IN_TIPO_LOCALIDADE")]
        [Required]
        [MaxLength(1)]
        public string Loc_in_tipo_localidade { get; set; }

        [Column("LOC_NU_SEQUENCIAL_SUB")]
        public int? Loc_nu_sequencial_sub { get; set; }

        [Column("LOC_KEY_DNE")]
        [MaxLength(16)]
        public string Loc_key_dne { get; set; }

        [Column("TEMP")]
        [MaxLength(8)]
        public string Temp { get; set; }

        [Column("CEP_DIG")]
        [MaxLength(8)]
        public string Cep_dig { get; set; }
    }
}
