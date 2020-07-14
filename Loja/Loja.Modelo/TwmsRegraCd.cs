using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
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

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_cadastro { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_hr_cadastro { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        [Required]
        public string Usuario_ult_atualizacao { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        [Required]
        public DateTime Dt_hr_ult_atualizacao { get; set; }


    }
}
