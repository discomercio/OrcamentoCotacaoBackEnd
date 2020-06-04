using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_SUBGRUPO")]
    public class TprodutoSubgrupo
    {
        [Key]
        [Column("codigo")]
        [MaxLength(10)]
        [Required]
        public string Codigo { get; set; }

        [Column("descricao")]
        [MaxLength(10)]
        [Required]
        public string Descricao { get; set; }

        [Column("inativo")]
        [Required]
        public byte Inativo { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_hr_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        [Required]
        public DateTime Dt_hr_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        [Required]
        public string Usuario_ult_atualizacao { get; set; }
    }
}
