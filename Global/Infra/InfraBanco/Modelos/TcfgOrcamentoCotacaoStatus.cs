using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_ORCAMENTO_COTACAO_STATUS")]
    public class TcfgOrcamentoCotacaoStatus
    {
        [Key]
        [Column("Id")]
        public Int16 Id { get; set; }

        [Column("Descricao")]
        [MaxLength(30)]
        [Required]
        public string Descricao { get; set; }

        public virtual TorcamentoCotacao TorcamentoCotacao { get; set; }

    }
}
