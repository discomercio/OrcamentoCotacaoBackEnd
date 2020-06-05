using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_WMS_REGRA_CD")]
    public class TwmsRegraCd
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_inativo { get; set; }

        [Column("apelido")]
        [MaxLength(30)]
        [Required]
        public string Apelido { get; set; }

        [Column("descricao")]
        [MaxLength(800)]
        public string Descricao { get; set; }
    }
}
