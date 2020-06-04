using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ALERTA_PRODUTO")]
    public class TalertaProduto
    {
        [Required]
        [Column("apelido")]
        [MaxLength(12)]
        public string Apelido { get; set; }

        [Column("mensagem")]
        [Required]
        [MaxLength(2000)]
        public string Mensagem { get; set; }

        [Column("ativo")]
        [Required]
        [MaxLength(1)]
        public string Ativo { get; set; }
    }
}
