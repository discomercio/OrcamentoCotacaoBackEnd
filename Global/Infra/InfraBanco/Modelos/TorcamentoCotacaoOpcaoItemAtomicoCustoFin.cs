using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_OPCAO_ITEM_ATOMICO_CUSTO_FIN")]
    public class TorcamentoCotacaoItemAtomicoCustoFin
    {

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Key]
        [Column("IdItemAtomico")]
        public int IdItemAtomico { get; set; }

        [Column("IdOpcaoPagto")]
        [Required]
        public int IdOpcaoPagto { get; set; }

        [Column("DescDado")]
        [Required]
        public decimal DescDado { get; set; }

        [Column("PrecoLista")]
        [Required]
        public decimal PrecoLista { get; set; }

        [Column("PrecoVenda")]
        [Required]
        public decimal PrecoVenda { get; set; }

        [Column("PrecoNF")]
        [Required]
        public decimal PrecoNF { get; set; }

        [Column("CustoFinancFornecCoeficiente")]
        [Required]
        public decimal CustoFinancFornecCoeficiente { get; set; }

        [Column("CustoFinancFornecPrecoListaBase")]
        [Required]
        public int CustoFinancFornecPrecoListaBase { get; set; }

    }
}
