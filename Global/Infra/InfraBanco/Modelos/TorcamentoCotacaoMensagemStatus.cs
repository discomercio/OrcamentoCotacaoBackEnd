using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_MENSAGEM_STATUS")]
    public class TorcamentoCotacaoMensagemStatus : IModel
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }
        
        [Column("IdOrcamentoCotacaoMensagem")]
        [Required]
        public int IdOrcamentoCotacaoMensagem { get; set; }
        
        [Column("IdTipoUsuarioContexto")]
        [Required]
        public short IdTipoUsuarioContexto { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Lida")]
        [Required]
        public bool Lida { get; set; }

        [Column("DataLida")]
        public DateTime? DataLida { get; set; }

        [Column("DataHoraLida")]
        public DateTime? DataHoraLida { get; set; }
        
        [Column("PendenciaTratada")]
        public bool? PendenciaTratada { get; set; }

        [Column("DataPendenciaTratada")]
        public DateTime? DataPendenciaTratada { get; set; }

        [Column("DataHoraPendenciaTratada")]
        public DateTime? DataHoraPendenciaTratada { get; set; }
        public TorcamentoCotacao TorcamentoCotacao { get; set; }

    }
}
