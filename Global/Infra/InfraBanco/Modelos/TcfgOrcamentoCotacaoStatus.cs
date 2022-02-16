using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_ORCAMENTO_COTACAO_STATUS")]
    public class TcfgOrcamentoCotacaoStatus
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }

        [Column("Descricao")]
        [MaxLength(30)]
        [Required]
        public string Descricao { get; set; }

    }
}
