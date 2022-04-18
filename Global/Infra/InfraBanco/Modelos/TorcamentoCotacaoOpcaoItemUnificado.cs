using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoItemUnificado : IModel
    {
        public int Id { get; set; }

        public int IdOrcamentoCotacaoOpcao { get; set; }

        public string Fabricante { get; set; }

        public string Produto { get; set; }

        public int Qtde { get; set; }

        public string Descricao { get; set; }

        public string DescricaoHtml { get; set; }

        public int Sequencia { get; set; }

        public TorcamentoCotacaoOpcao TorcamentoCotacaoOpcao { get; set; }
        public ICollection<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomicos { get; set; }

    }
}
