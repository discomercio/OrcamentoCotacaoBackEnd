using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN")]
    public class TorcamentoCotacaoItemAtomicoCustoFin
    {
        public int Id { get; set; }
        public int IdItemAtomico { get; set; }
        public int IdOpcaoPagto { get; set; }
        public decimal DescDado { get; set; }
        public decimal PrecoLista { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal PrecoNF { get; set; }
        public decimal CustoFinancFornecCoeficiente { get; set; }
        public int CustoFinancFornecPrecoListaBase { get; set; }

        public TorcamentoCotacaoOpcaoItemAtomico TorcamentoCotacaoOpcaoItemAtomico { get; set; }
        public TorcamentoCotacaoOpcaoPagto TorcamentoCotacaoOpcaoPagto { get; set; }

    }
}
