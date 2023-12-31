﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PARAMETRO")]
    public class Tparametro
    {
        [Key]
        [Column("id")]
        [MaxLength(100)]
        [Required]
        public string Id { get; set; }

        [Column("campo_inteiro")]
        [Required]
        public int Campo_inteiro { get; set; }

        [Column("campo_real")]
        [Required]
        public float Campo_Real { get; set; }

        [Column("campo_texto")]
        [MaxLength(1024)]
        public string Campo_texto { get; set; }
    }
}
