using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("NFE_MUNICIPIO")]
    public class NfeMunicipio
    {
        [Key]
        [Required]
        [MaxLength(7)]
        [Column("CodMunic")]
        public string CodMunic { get; set; }

        [Column("Descricao")]
        [Required]
        [MaxLength(150)]
        public string Descricao { get; set; }

        [Column("DescricaoSemAcento")]
        [MaxLength(150)]
        public string DescricaoSemAcento { get; set; }
    }
}
