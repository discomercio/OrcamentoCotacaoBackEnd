using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_AVISO_EXIBIDO")]
    public class TavisoExibido
    {
        [Required]
        [MaxLength(12)]
        public string Id { get; set; }
        
        [Required]
        [Column("usuario")]
        [MaxLength(10)]
        public string Usuario { get; set; }

        [Column("dt_hr_ult_exibicao")]
        public DateTime Dt_hr_ult_exibicao { get; set; }
    }
}
