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
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdItemAtomico")]
        public int IdItemAtomico { get; set; }

        [Column("IdOpcaoPagto")]
        public int IdOpcaoPagto { get; set; }

        [Column("DescDado")]
        public float DescDado { get; set; }

        [Column("PrecoLista")]
        public decimal PrecoLista { get; set; }

        [Column("PrecoVenda")]
        public decimal PrecoVenda { get; set; }

        [Column("PrecoNF")]
        public decimal PrecoNF { get; set; }

        [Column("CustoFinancFornecCoeficiente")]
        public float CustoFinancFornecCoeficiente { get; set; }

        [Column("CustoFinancFornecPrecoListaBase")]
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        [Column("StatusDescontoSuperior")]
        public bool StatusDescontoSuperior { get; set; }

        [Column("IdUsuarioDescontoSuperior")]
        public int? IdUsuarioDescontoSuperior { get; set; }

        [Column("DataHoraDescontoSuperior")]
        public DateTime? DataHoraDescontoSuperior { get; set; }

        [Column("IdOperacaoAlcadaDescontoSuperior")]
        public int? IdOperacaoAlcadaDescontoSuperior { get; set; }

        public TorcamentoCotacaoOpcaoItemAtomico TorcamentoCotacaoOpcaoItemAtomico { get; set; }
        public TorcamentoCotacaoOpcaoPagto TorcamentoCotacaoOpcaoPagto { get; set; }
        public virtual Tusuario Tusuario { get; set; }

    }
}
