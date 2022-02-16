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
        [Column("id")]
        [Required]
        public int Id { get; set; }
        
        [Column("IdOrcamentoCotacao")]
        [Required]
        public int IdOrcamentoCotacao { get; set; }
        
        [Column("IdOrcamentoCotacao")]
        [Required]
        public int IdTipoUsuarioContextoRemetente { get; set; }

        [Column("IdUsuarioRemetente")]
        public int IdUsuarioRemetente { get; set; }

        [Column("IdTipoUsuarioContextoDestinatario")]
        [Required]
        public int IdTipoUsuarioContextoDestinatario { get; set; }

        [Column("IdUsuarioDestinatario")]
        public int IdUsuarioDestinatario { get; set; }

        [Column("Lida")]
        [Required]
        public byte Lida { get; set; }

        [Column("DataLida")]
        public DateTime DataLida { get; set; }

        [Column("DataHoraLida")]
        public DateTime DataHoraLida { get; set; }

        [Column("Mensagem")]
        [Required]
        public String Mensagem { get; set; }

        [Column("DataCadastro")]
        [Required]
        public DateTime DataCadastro { get; set; }

        [Column("DataHoraCadastro")]
        [Required]
        public DateTime DataHoraCadastro { get; set; }

    }
}
