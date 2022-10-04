using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_MENSAGEM")]
    public class TorcamentoCotacaoMensagem : IModel
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }
        
        [Column("IdOrcamentoCotacao")]
        [Required]
        public int IdOrcamentoCotacao { get; set; }
        
        [Column("IdTipoUsuarioContextoRemetente")]
        [Required]
        public short IdTipoUsuarioContextoRemetente { get; set; }

        [Column("IdUsuarioRemetente")]
        public int IdUsuarioRemetente { get; set; }

        [Column("IdTipoUsuarioContextoDestinatario")]
        [Required]
        public short IdTipoUsuarioContextoDestinatario { get; set; }

        [Column("IdUsuarioDestinatario")]
        public int IdUsuarioDestinatario { get; set; }       

        [Column("Mensagem")]
        [Required]
        public String Mensagem { get; set; }

        [Column("DataCadastro")]
        [Required]
        public DateTime DataCadastro { get; set; }

        [Column("DataHoraCadastro")]
        [Required]
        public DateTime DataHoraCadastro { get; set; }

        [Column("IdOrcamentoCotacaoEmailQueue")]
        public long? IdOrcamentoCotacaoEmailQueue { get; set; }

        public TorcamentoCotacao TorcamentoCotacao { get; set; }

    }
}
