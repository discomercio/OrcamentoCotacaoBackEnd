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
        public TorcamentoCotacaoItemUnificado()
        {
        }

        public TorcamentoCotacaoItemUnificado(int idOrcamentoCotacaoOpcao, string fabricante, string produto, int qtde, string descricao, string descricaoHtml, int sequencia)
        {
            IdOrcamentoCotacaoOpcao = idOrcamentoCotacaoOpcao;
            Fabricante = fabricante;
            Produto = produto;
            Qtde = qtde;
            Descricao = descricao;
            DescricaoHtml = descricaoHtml;
            Sequencia = sequencia;
        }

        [Column("Id")]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacaoOpcao")]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [Column("Fabricante")]
        public string Fabricante { get; set; }

        [Column("Produto")]
        public string Produto { get; set; }

        [Column("Qtde")]
        public int Qtde { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }

        [Column("DescricaoHtml")]
        public string DescricaoHtml { get; set; }

        [Column("Sequencia")]
        public int Sequencia { get; set; }

        public TorcamentoCotacaoOpcao TorcamentoCotacaoOpcao { get; set; }
        public List<TorcamentoCotacaoOpcaoItemAtomico> TorcamentoCotacaoOpcaoItemAtomicos { get; set; }

    }
}
