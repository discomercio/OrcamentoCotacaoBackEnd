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
        public int Status { get; set; }

        [Column("IdTipoUsuarioContextoUltStatus")]
        [Required]
        public int IdTipoUsuarioContextoUltStatus { get; set; }
        
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
        public int IdTipoUsuarioContextoCadastro { get; set; }

        [Column("IdUsuarioCadastro")]        
        public int IdUsuarioCadastro { get; set; }

        [Column("DataCadastro")]
        [Required]
        public DateTime IdUsuarDataCadastroioCadastro { get; set; }

        [Column("DataHoraCadastro")]
        [Required]
        public DateTime DataHoraCadastro { get; set; }

    }
}
