using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        //[Column("campo_monetario", TypeName = "money(19,4)")]
        //[Required]
        //public decimal Campo_monetario { get; set; }

        //[Column("campo_real")]
        //[Required]
        //public float Campo_real { get; set; }

        //[Column("campo_data")]
        //public DateTime? Campo_data { get; set; }

        //[Column("campo_texto")]
        //[MaxLength(1024)]
        //public string Campo_texto { get; set; }

        //[Column("dt_hr_ult_atualizacao")]
        //[Required]
        //public DateTime Dt_hr_ult_atualizacao { get; set; }

        //[Column("usuario_ult_atualizacao")]
        //[MaxLength(10)]
        //public string Usuario_ult_atualizacao { get; set; }

        //[Column("obs")]
        //[MaxLength(1000)]
        //public string Obs { get; set; }
    }
}
