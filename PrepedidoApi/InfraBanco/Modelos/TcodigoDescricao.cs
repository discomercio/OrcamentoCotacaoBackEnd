using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CODIGO_DESCRICAO")]
    public class TcodigoDescricao
    {
        [Key]
        [Required]
        [MaxLength(60)]
        [Column("grupo")]
        public string Grupo { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_Inativo { get; set; }

        [Column("descricao")]
        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Column("lojas_habilitadas")]
        [MaxLength]
        public string Lojas_Habilitadas { get; set; }
    }
}
