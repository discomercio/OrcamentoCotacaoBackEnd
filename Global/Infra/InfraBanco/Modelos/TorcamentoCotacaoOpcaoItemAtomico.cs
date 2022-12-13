using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public  class TorcamentoCotacaoOpcaoItemAtomico: IModel
    {
        public TorcamentoCotacaoOpcaoItemAtomico()
        {
        }

        public TorcamentoCotacaoOpcaoItemAtomico(int idItemUnificado, string fabricante, string produto, short qtde, string descricao, string descricaoHtml)
        {
            IdItemUnificado = idItemUnificado;
            Fabricante = fabricante;
            Produto = produto;
            Qtde = qtde;
            Descricao = descricao;
            DescricaoHtml = descricaoHtml;
        }

        [Column("Id")]
        public int Id { get; set; }

        [Column("IdItemUnificado")]
        public int IdItemUnificado { get; set; }

        [Column("Fabricante")]
        public string Fabricante { get; set; }

        [Column("Produto")]
        public string Produto { get; set; }

        [Column("Qtde")]
        public short Qtde { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }

        [Column("DescricaoHtml")]
        public string DescricaoHtml { get; set; }

        public TorcamentoCotacaoItemUnificado TorcamentoCotacaoItemUnificado { get; set; }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoItemAtomicoCustoFin { get; set; }
    }
}
