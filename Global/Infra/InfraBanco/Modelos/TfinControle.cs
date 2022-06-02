using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_FIN_CONTROLE")]
    public class TfinControle
    {
        [Key]
        [Required]
        [Column("id")]
        [MaxLength(80)]
        public string Id { get; set; }

        [Required]
        [Column("nsu")]
        public int Nsu { get; set; }

        [Required]
        [Column("dt_hr_ult_atualizacao")]
        public DateTime Dt_hr_ult_atualizacao { get; set; }

        [Column("dummy")]
        public Boolean Dummy { get; set; }
    }
}
