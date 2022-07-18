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
        public float DescDado { get; set; }
        public decimal PrecoLista { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal PrecoNF { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }
        public decimal CustoFinancFornecPrecoListaBase { get; set; }
        public bool StatusDescontoSuperior { get; set; }
        public int? IdUsuarioDescontoSuperior { get; set; }
        public DateTime? DataHoraDescontoSuperior { get; set; }
        public int? IdOperacaoAlcadaDescontoSuperior { get; set; }

        public TorcamentoCotacaoOpcaoItemAtomico TorcamentoCotacaoOpcaoItemAtomico { get; set; }
        public TorcamentoCotacaoOpcaoPagto TorcamentoCotacaoOpcaoPagto { get; set; }
        public virtual Tusuario Tusuario { get; set; }

    }
}
