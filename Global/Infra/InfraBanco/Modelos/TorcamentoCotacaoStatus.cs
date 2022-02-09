using Interfaces;
using System.Collections.Generic;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoStatus : IModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public virtual List<TorcamentoCotacao> TorcamentoCotacaos { get; set; }

    }
}
