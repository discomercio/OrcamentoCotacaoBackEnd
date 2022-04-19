using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcaoItemAtomicoCustoFin : IModel
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
