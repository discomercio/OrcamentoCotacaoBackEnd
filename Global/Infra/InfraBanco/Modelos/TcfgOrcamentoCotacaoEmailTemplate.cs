using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_ORCAMENTO_COTACAO_EMAIL_TEMPLATE")]
    public class TcfgOrcamentoCotacaoEmailTemplate : IModel
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdCfgUnidadeNegocio")]
        [MaxLength(4)]
        public int IdCfgUnidadeNegocio { get; set; }

        [Column("IdCfgParametro")]
        [MaxLength(4)]
        [Required]
        public int IdCfgParametro { get; set; }

        [Column("NomeTemplate")]
        [MaxLength(100)]
        [Required]
        public string NomeTemplate { get; set; }

        [Column("EmailTemplateSubject")]
        [MaxLength(240)]
        public string EmailTemplateSubject { get; set; }

        [Column("EmailTemplateBody")]
        [Required]
        public string EmailTemplateBody { get; set; }

        [Column("Ativo")]
        [Required]
        public bool Ativo { get; set; }

        [Column("DataHoraCadastro")]
        [Required]
        public DateTime DataHoraCadastro { get; set; }

        [Column("UsuarioCadastro")]
        [MaxLength(10)]
        [Required]
        public string UsuarioCadastro { get; set; }

        [Column("DataHoraUltAtualizacao")]
        [MaxLength(8)]
        public DateTime DataHoraUltAtualizacao { get; set; }

        [Column("UsuarioUltAtualizacao")]
        [MaxLength(10)]
        public string UsuarioUltAtualizacao { get; set; }

        [Column("Obs")]
        public string Obs { get; set; }

    }
}
