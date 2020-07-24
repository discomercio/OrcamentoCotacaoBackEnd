using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_BANCO")]
    public class Tbanco
    {
        [Key]
        [MaxLength(3)]
        [Column("codigo")]
        [Required]
        public string Codigo { get; set; }

        [Column("descricao")]
        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }
    }
}
