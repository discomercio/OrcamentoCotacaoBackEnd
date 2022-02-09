using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_MODULO")]
    public class TcfgModulo
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }

        [Column("Descricao")]
        [MaxLength(20)]
        [Required]
        public string Descricao { get; set; }

        [Column("Ordenacao")]
        public short Ordenacao { get; set; }

        public ICollection<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get; set; }
        public ICollection<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get; set; }
    }
}
