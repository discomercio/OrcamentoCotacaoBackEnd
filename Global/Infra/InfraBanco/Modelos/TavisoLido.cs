using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_AVISO_LIDO")]
    public class TavisoLido
    {
        [Required]
        [Column("id")]
        [MaxLength(12)]
        public string Id { get; set; }

        [Required]
        [Column("usuario")]
        [MaxLength(20)]
        public string Usuario { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }
    }
}
