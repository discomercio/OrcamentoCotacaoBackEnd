using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_LINK")]
    public class TorcamentoCotacaoLink : IModel
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacao")]
        [Required]
        public int IdOrcamentoCotacao { get; set; }

        [Column("Guid")]
        [Required]
        public Guid Guid { get; set; }

        [Column("Status")]
        [Required]
        public Int16 Status { get; set; }

        [Column("IdTipoUsuarioContextoUltStatus")]
        [Required]
        public Int16 IdTipoUsuarioContextoUltStatus { get; set; }
        
        [Column("IdUsuarioUltStatus")]
        [Required]
        public int IdUsuarioUltStatus { get; set; }

        [Column("DataUltStatus")]
        [Required]
        public DateTime DataUltStatus { get; set; }

        [Column("DataHoraUltStatus")]
        [Required]
        public DateTime DataHoraUltStatus { get; set; }

        [Column("IdTipoUsuarioContextoCadastro")]
        [Required]
        public Int16 IdTipoUsuarioContextoCadastro { get; set; }

        [Column("IdUsuarioCadastro")]        
        public int? IdUsuarioCadastro { get; set; }

        [Column("DataCadastro")]
        [Required]
        public DateTime DataCadastro { get; set; }

        [Column("DataHoraCadastro")]
        [Required]
        public DateTime DataHoraCadastro { get; set; }

        public virtual TorcamentoCotacao TorcamentoCotacao { get; set; }
    }
}
