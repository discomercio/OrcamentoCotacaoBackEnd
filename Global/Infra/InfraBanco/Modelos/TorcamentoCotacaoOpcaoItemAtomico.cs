using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO")]
    public  class TorcamentoCotacaoOpcaoItemAtomico
    {
        public int Id { get; set; }
        public int IdItemUnificado { get; set; }
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public short Qtde { get; set; }
        public string Descicao { get; set; }
        public string DescricaoHtml { get; set; }

        public TorcamentoCotacaoItemUnificado TorcamentoCotacaoItemUnificado { get; set; }

        public TorcamentoCotacaoItemAtomicoCustoFin TorcamentoCotacaoItemAtomicoCustoFin { get; set; }
    }
}
