using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_SESSAO_HISTORICO")]
    public class TsessaoHistorico
    {
        [Column("Usuario")]
        [MaxLength(20)]
        [Required]
        public string Usuario { get; set; }

    }
}
