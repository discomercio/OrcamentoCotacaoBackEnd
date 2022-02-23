using System.Collections.Generic;

namespace FormaPagamento.Dados
{
    public class FormaPagamentoDados
    {
        public int IdTipoPagamento { get; set; }

        public List<MeioPagamentoDados> MeiosPagamentos { get; set; }
    }
}
