using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR_HISTORICO")]
    public class TpercentualCustoFinanceiroFornecedorHistorico
    {
        [Column("data")]
        [Required]
        public DateTime Data { get; set; }

        [Column("fabricante")]
        [MaxLength(4)]
        [Required]
        public string Fabricante { get; set; }

        [Column("tipo_parcelamento")]
        [MaxLength(2)]
        [Required]
        public string Tipo_Parcelamento { get; set; }

        [Column("qtde_parcelas")]
        [Required]
        public short Qtde_Parcelas { get; set; }

        [Column("coeficiente")]
        public float Coeficiente { get; set; }
    }
}
