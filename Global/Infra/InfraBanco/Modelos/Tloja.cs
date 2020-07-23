using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_LOJA")]
    public class Tloja
    {
        [Column("loja")]
        [Key]
        [Required]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("nome")]
        [MaxLength(30)]
        public string Nome { get; set; }

        [Column("razao_social")]
        [MaxLength(60)]
        public string Razao_Social { get; set; }

        [Column("unidade_negocio")]
        [MaxLength(5)]
        public string Unidade_Negocio { get; set; }
    }
}
