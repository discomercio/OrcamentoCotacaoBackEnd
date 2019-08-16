using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTO")]
    public class Torcamento
    {
        [Key]
        [Column("orcamento")]
        [MaxLength(9)]
        [Required]
        public string Orcamento { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("data")]
        public DateTime? Data { get; set; }

        [Column("hora")]
        [MaxLength(6)]
        public string Hora { get; set; }

        [Column("id_cliente")]
        [MaxLength(12)]
        [ForeignKey("Tcliente")]
        public string Id_Cliente { get; set; }

        [Column("midia")]
        [MaxLength(3)]
        public string Midia { get; set; }

        [Column("servicos")]
        [MaxLength(60)]
        public string Servicos { get; set; }

        [Column("vl_servicos")]
        [Required]
        public decimal Vl_Servicos { get; set; }

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("obs_1")]
        [MaxLength(500)]
        public string Obs_1 { get; set; }
        
        [Column("obs_2")]
        [MaxLength(10)]
        public string Obs_2 { get; set; }

        [Column("qtde_parcelas")]
        public short? Qtde_Parcelas { get; set; }

        [Column("forma_pagto")]
        [MaxLength(250)]
        public string Forma_Pagamento { get; set; }

        [Column("st_orcamento")]
        [MaxLength(3)]
        public string St_Orcamento { get; set; }

        [Column("cancelado_data")]
        public DateTime? Cancelado_Data { get; set; }

        [Column("cancelado_usuario")]
        [MaxLength(20)]
        public string Cancelado_Usuario { get; set; }

        [Column("st_fechamento")]        
        [MaxLength(1)]
        public string St_Fechamento { get; set; }

        [Column("fechamento_data")]
        public DateTime? Fechamento_Data { get; set; }

        [Column("fechamento_usuario")]
        [MaxLength(10)]
        public string Fechamento_Usuario { get; set; }

        [Column("loja_indicou")]
        [MaxLength(3)]
        public string Loja_Indicou { get; set; }

        [Column("comissao_loja_indicou")]
        public float? Comissao_Loja_Indicou { get; set; }

        [Column("venda_externa")]
        public short? Venda_Externa { get; set; }

        //[Column("timestamp")]
        //public DateTime? Timestamp { get; set; }

        [Column("tipo_parcelamento")]
        [Required]
        public short Tipo_Parcelamento { get; set; }

        [Column("av_forma_pagto")]
        [Required]
        public short Av_Forma_Pagto { get; set; }

        [Column("pc_qtde_parcelas")]
        [Required]
        public short Pc_Qtde_Parcelas { get; set; }

        [Column("pc_valor_parcela")]
        public decimal? Pc_Valor_Parcela { get; set; }

        [Column("pce_forma_pagto_entrada")]
        [Required]
        public short Pce_Forma_Pagto_Entrada { get; set; }

        [Column("pce_forma_pagto_prestacao")]
        [Required]
        public short Pce_Forma_Pagto_Prestacao { get; set; }

        [Column("pce_entrada_valor")]
        public decimal? Pce_Entrada_Valor { get; set; }

        [Column("pce_prestacao_qtde")]
        [Required]
        public short Pce_Prestacao_Qtde { get; set; }

        [Column("pce_prestacao_valor")]
        public decimal? Pce_Prestacao_Valor { get; set; }

        [Column("pce_prestacao_periodo")]
        [Required]
        public short Pce_Prestacao_Periodo { get; set; }

        [Column("pse_forma_pagto_prim_prest")]
        [Required]
        public short Pse_Forma_Pagto_Prim_Prest { get; set; }

        [Column("pse_forma_pagto_demais_prest")]
        [Required]
        public short Pse_Forma_Pagto_Demais_Prest { get; set; }

        [Column("pse_prim_prest_valor")]
        public decimal? Pse_Prim_Prest_Valor { get; set; }

        [Column("pse_prim_prest_apos")]
        [Required]
        public short Pse_Prim_Prest_Apos { get; set; }

        [Column("pse_demais_prest_qtde")]
        [Required]
        public short Pse_Demais_Prest_Qtde { get; set; }

        [Column("pse_demais_prest_valor")]
        public decimal Pse_Demais_Prest_Valor { get; set; }

        [Column("pse_demais_prest_periodo")]
        [Required]
        public short Pse_Demais_Prest_Periodo { get; set; }

        [Column("pu_forma_pagto")]
        [Required]
        public short Pu_Forma_Pagto { get; set; }

        [Column("pu_valor")]
        public decimal? Pu_Valor { get; set; }

        [Column("pu_vencto_apos")]
        [Required]
        public short Pu_Vencto_Apos { get; set; }

        [Column("orcamentista")]
        [MaxLength(20)]
        public string Orcamentista { get; set; }

        [Column("vl_total")]
        public decimal? Vl_Total { get; set; }

        [Column("vl_total_NF")]
        public decimal? Vl_Total_NF { get; set; }

        [Column("vl_total_RA")]
        public decimal? Vl_Total_RA { get; set; }

        [Column("perc_RT")]
        public float? Perc_RT { get; set; }

        [Column("st_orc_virou_pedido")]
        public short? St_Orc_Virou_Pedido { get; set; }

        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("st_end_entrega")]
        [Required]
        public short St_End_Entrega { get; set; }

        [Column("EndEtg_endereco")]
        [MaxLength(80)]
        public string EndEtg_Endereco { get; set; }

        [Column("EndEtg_bairro")]
        [MaxLength(72)]
        public string EndEtg_Bairro { get; set; }

        [Column("EndEtg_cidade")]
        [MaxLength(60)]
        public string EndEtg_Cidade { get; set; }

        [Column("EndEtg_uf")]
        [MaxLength(2)]
        public string EndEtg_UF { get; set; }

        [Column("EndEtg_cep")]
        [MaxLength(8)]
        public string EndEtg_CEP { get; set; }

        [Column("st_etg_imediata")]
        [Required]
        public short St_Etg_Imediata { get; set; }

        [Column("etg_imediata_data")]
        public DateTime? Etg_Imediata_Data { get; set; }

        [Column("etg_imediata_usuario")]
        [MaxLength(20)]
        public string Etg_Imediata_Usuario { get; set; }

        [Column("StBemUsoConsumo")]
        [Required]
        public short StBemUsoConsumo { get; set; }

        [Column("custoFinancFornecTipoParcelamento")]        
        [MaxLength(2)]
        public string CustoFinancFornecParcelamento { get; set; }

        [Column("custoFinancFornecQtdeParcelas")]
        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }

        [Column("EndEtg_endereco_numero")]
        [MaxLength(20)]
        public string EndEtg_Endereco_Numero { get; set; }

        [Column("EndEtg_endereco_complemento")]
        [MaxLength(60)]
        public string EndEtg_Endereco_Complemento { get; set; }

        [Column("InstaladorInstalaStatus")]
        [Required]
        public short InstaladorInstalaStatus { get; set; }

        [Column("InstaladorInstalaUsuarioUltAtualiz")]
        [MaxLength(20)]
        public string InstaladorInstalaUsuarioUltAtualiz { get; set; }

        [Column("InstaladorInstalaDtHrUltAtualiz")]
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }

        [Column("GarantiaIndicadorStatus")]
        [Required]
        public Byte GarantiaIndicadorStatus { get; set; }

        [Column("GarantiaIndicadorUsuarioUltAtualiz")]
        [MaxLength(20)]
        public string GarantiaIndicadorUsuarioUltAtualiz { get; set; }

        [Column("GarantiaIndicadorDtHrUltAtualiz")]
        public DateTime? GarantiaInidicadorDtHrUltAtualiz { get; set; }

        [Column("perc_desagio_RA_liquida")]
        [Required]
        public float Perc_Desagio_RA_Liquida { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }

        [Column("st_violado_permite_RA_status")]
        [Required]
        public short St_Violado_Permite_RA_Status { get; set; }

        [Column("dt_hr_violado_permite_RA_status")]
        public DateTime? Dt_Hr_Violado_Permite_RA_Status { get; set; }

        [Column("usuario_violado_permite_RA_status")]
        [MaxLength(20)]
        public string Usuario_Violado_Permite_RA_Status { get; set; }

        [Column("numero_loja")]
        public short? Numero_Loja { get; set; }

        [Column("data_hora")]
        public DateTime? Data_Hora { get; set; }

        [Column("EndEtg_obs")]
        [MaxLength(100)]
        public string EndEtg_Obs { get; set; }

        [Column("EndEtg_cod_justificativa")]
        [MaxLength(3)]
        public string EndEtg_Cod_Justificativa { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        [Required]
        public short Pc_Maquineta_Qtde_Parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela")]
        [Required]
        public decimal Pc_Maquineta_Valor_Parcela { get; set; }

        //propriedades
        public Tcliente Tcliente { get; set; }
    }
}
