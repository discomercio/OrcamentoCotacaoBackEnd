using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("T_ORCAMENTO_COTACAO_EMAIL_QUEUE")]
    public class TorcamentoCotacaoEmailQueue : IModel
    {
        [Key]
        public long Id { get; set; }
        public int? IdCfgUnidadeNegocio { get; set; }

        [Required]
        [MaxLength(1024)]
        public string From { get; set; }
        [MaxLength(1024)]
        public string FromDisplayName { get; set; }
        [MaxLength(1024)]
        public string To { get; set; }
        [MaxLength(1024)]
        public string Cc { get; set; }
        [MaxLength(1024)]
        public string Bcc { get; set; }
        [MaxLength(240)]
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? Sent { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime DateCreated { get; set; }
        public short Status { get; set; }
        [Required]
        public byte AttemptsQty { get; set; }
        public DateTime? DateLastAttempt { get; set; }
        public string Attachment { get; set; }
        public string ErrorMsgLastAttempt { get; set; }
    }
}
