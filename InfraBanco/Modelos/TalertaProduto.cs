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
        //[Key]
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

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [Required]
        [MaxLength(10)]
        public string Usuario_cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [Required]
        [MaxLength(10)]
        public string Usuario_ult_atualizacao { get; set; }

    }
}
