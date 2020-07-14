using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelo
{
    [Table("t_OPERACAO")]
    public class Toperacao
    {
        //[Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        public TperfilUsuario TperfilUsuario { get; set; }

        [Required]
        [MaxLength(5)]
        [Column("modulo")]
        public string Modulo { get; set; }

        [Required]
        [MaxLength(6)]
        [Column("tipo_operacao")]
        public string Tipo_operacao { get; set; }

        [Column("ordenacao")]
        public string Ordenacao { get; set; }

        [MaxLength(80)]
        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("timestamp")]
        public byte? Timestamp { get; }

        [Required]
        [Column("st_inativo")]
        public byte St_inativo { get; set; }


    }
}
