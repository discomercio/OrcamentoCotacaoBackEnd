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

        public int Id { get; set; }
        public int IdItemUnificado { get; set; }
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public short Qtde { get; set; }
        public string Descricao { get; set; }
        public string DescricaoHtml { get; set; }

        public TorcamentoCotacaoItemUnificado TorcamentoCotacaoItemUnificado { get; set; }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoItemAtomicoCustoFin { get; set; }
    }
}
