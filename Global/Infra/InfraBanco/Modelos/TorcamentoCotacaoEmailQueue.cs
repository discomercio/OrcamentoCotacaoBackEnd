using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoEmailQueue : IModel
    {
        public decimal Id { get; set; }
        public int IdCfgUnidadeNegocio { get; set; }
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Sent { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateScheduled { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public int AttemptsQty { get; set; }
        public DateTime DateLastAttempt { get; set; }
        public string Attachment { get; set; }
        public string ErrorMsgLastAttempt { get; set; }
    }
}
