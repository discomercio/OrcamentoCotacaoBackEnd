using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("T_ORCAMENTO_COTACAO_EMAIL")]
    public class TorcamentoCotacaoEmail : IModel
    {
        [Key]
        public int IdOrcamentoCotacao { get; set; }
        public long IdOrcamentoCotacaoEmailQueue { get; set; }

    }
}
