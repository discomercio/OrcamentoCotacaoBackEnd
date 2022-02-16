using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_OPCAO_ITEM_UNIFICADO")]
    public class TorcamentoCotacaoItemUnificado
    {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Key]
        [Column("IdOrcamentoCotacaoOpcao")]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [Column("Fabricante")]
        [Required]
        public string Fabricante { get; set; }

        [Column("Produto")]
        [Required]
        public string Produto { get; set; }

        [Column("Qtde")]
        [Required]
        public int Qtde { get; set; }

        [Column("Descricao")]
        public byte Descricao { get; set; }

        [Column("DescricaoHtml")]
        public byte DescricaoHtml { get; set; }

        [Column("Sequencia")]
        [Required]
        public int Sequencia { get; set; }

    }
}
