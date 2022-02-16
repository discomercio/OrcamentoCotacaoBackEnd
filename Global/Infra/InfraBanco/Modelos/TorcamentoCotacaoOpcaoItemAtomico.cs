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
        [Required]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("IdItemUnificado")]
        public int IdItemUnificado { get; set; }

        [Required]
        [MaxLength(4)]
        [Column("Fabricante")]
        public string Fabricante { get; set; }

        [Required]
        [MaxLength(8)]
        [Column("Produto")]
        public string Produto { get; set; }

        [Required]
        [Column("Qtde")]
        public short Qtde { get; set; }

        [MaxLength(120)]
        [Column("Descricao")]
        public string Descicao { get; set; }

        [MaxLength(4000)]
        [Column("DescricaoHtml")]
        public string DescricaoHtml { get; set; }

        public TorcamentoCotacaoItemAtomicoCustoFin TorcamentoCotacaoItemAtomicoCustoFin { get; set; }

        public TorcamentoCotacaoItemUnificado TorcamentoCotacaoItemUnificado { get; set; }
    }
}
