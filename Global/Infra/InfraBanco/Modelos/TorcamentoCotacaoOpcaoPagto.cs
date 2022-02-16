using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO_COTACAO_OPCAO_PAGTO")]
    public class TorcamentoCotacaoOpcaoPagto
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacaoOpcao")]
        [Required]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [Column("Aprovado")]
        [Required]
        public byte Aprovado { get; set; }

        [Column("Observacao")]
        [Required]
        public string Observacao { get; set; }

        [Column("tipo_parcelamento")]
        [Required]
        public int tipo_parcelamento { get; set; }

        [Column("av_forma_pagto")]
        [Required]
        public int av_forma_pagto { get; set; }

        [Column("pc_qtde_parcelas")]
        [Required]
        public int pc_qtde_parcelas { get; set; }

        [Column("pc_valor_parcela")]
        [Required]
        public decimal pc_valor_parcela { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        [Required]
        public int pc_maquineta_qtde_parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela")]
        [Required]
        public decimal pc_maquineta_valor_parcela { get; set; }

        [Column("pce_forma_pagto_entrada")]
        [Required]
        public int pce_forma_pagto_entrada { get; set; }

        [Column("pce_forma_pagto_prestacao")]
        [Required]
        public int pce_forma_pagto_prestacao { get; set; }

        [Column("pce_entrada_valor")]
        [Required]
        public decimal pce_entrada_valor { get; set; }

        [Column("pce_prestacao_qtde")]
        [Required]
        public int pce_prestacao_qtde { get; set; }

        [Column("pce_prestacao_valor")]
        [Required]
        public decimal pce_prestacao_valor { get; set; }

        [Column("pce_prestacao_periodo")]
        [Required]
        public int pce_prestacao_periodo { get; set; }

        [Column("pse_forma_pagto_prim_prest")]
        [Required]
        public int pse_forma_pagto_prim_prest { get; set; }

        [Column("pse_forma_pagto_demais_prest")]
        [Required]
        public int pse_forma_pagto_demais_prest { get; set; }

        [Column("pse_prim_prest_valor")]
        [Required]
        public decimal pse_prim_prest_valor { get; set; }

        [Column("pse_prim_prest_apos")]
        [Required]
        public int pse_prim_prest_apos { get; set; }

        [Column("pse_demais_prest_qtde")]
        [Required]
        public int pse_demais_prest_qtde { get; set; }

        [Column("pse_demais_prest_valor")]
        [Required]
        public decimal pse_demais_prest_valor { get; set; }

        [Column("pse_demais_prest_periodo")]
        [Required]
        public int pse_demais_prest_periodo { get; set; }

        [Column("pu_forma_pagto")]
        [Required]
        public int pu_forma_pagto { get; set; }

        [Column("pu_valor")]
        [Required]
        public decimal pu_valor { get; set; }

        [Column("pu_vencto_apos")]
        [Required]
        public int pu_vencto_apos { get; set; }        
    }
}
