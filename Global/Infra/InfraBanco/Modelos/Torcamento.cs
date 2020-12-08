using System;
using System.Collections.Generic;
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

        public ICollection<TorcamentoItem> TorcamentoItem { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("data")]
        public DateTime? Data { get; set; }

        [Column("hora")]
        [MaxLength(6)]
        public string Hora { get; set; }

        [Column("data_hora")]
        public DateTime? Data_hora { get;}

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

        [Column("vl_servicos", TypeName = "money")]
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

        [Column("comissao_loja_indicou")]
        public float? Comissao_Loja_Indicou { get; set; }

        [Column("venda_externa")]
        public short? Venda_Externa { get; set; }

        [Column("tipo_parcelamento")]
        [Required]
        public short Tipo_Parcelamento { get; set; }

        [Column("av_forma_pagto")]
        [Required]
        public short Av_Forma_Pagto { get; set; }

        [Column("pc_qtde_parcelas")]
        [Required]
        public short Pc_Qtde_Parcelas { get; set; }

        [Column("pc_valor_parcela", TypeName = "money")]
        public decimal? Pc_Valor_Parcela { get; set; }

        [Column("pce_forma_pagto_entrada")]
        [Required]
        public short Pce_Forma_Pagto_Entrada { get; set; }

        [Column("pce_forma_pagto_prestacao")]
        [Required]
        public short Pce_Forma_Pagto_Prestacao { get; set; }

        [Column("pce_entrada_valor", TypeName = "money")]
        public decimal? Pce_Entrada_Valor { get; set; }

        [Column("pce_prestacao_qtde")]
        [Required]
        public short Pce_Prestacao_Qtde { get; set; }

        [Column("pce_prestacao_valor", TypeName = "money")]
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

        [Column("pse_prim_prest_valor", TypeName = "money")]
        public decimal? Pse_Prim_Prest_Valor { get; set; }

        [Column("pse_prim_prest_apos")]
        [Required]
        public short Pse_Prim_Prest_Apos { get; set; }

        [Column("pse_demais_prest_qtde")]
        [Required]
        public short Pse_Demais_Prest_Qtde { get; set; }

        [Column("pse_demais_prest_valor", TypeName = "money")]
        public decimal Pse_Demais_Prest_Valor { get; set; }

        [Column("pse_demais_prest_periodo")]
        [Required]
        public short Pse_Demais_Prest_Periodo { get; set; }

        [Column("pu_forma_pagto")]
        [Required]
        public short Pu_Forma_Pagto { get; set; }

        [Column("pu_valor", TypeName = "money")]
        public decimal? Pu_Valor { get; set; }

        [Column("pu_vencto_apos")]
        [Required]
        public short Pu_Vencto_Apos { get; set; }

        [Column("orcamentista")]
        [MaxLength(20)]
        public string Orcamentista { get; set; }

        [Column("vl_total", TypeName = "money")]
        public decimal? Vl_Total { get; set; }

        [Column("vl_total_NF", TypeName = "money")]
        public decimal? Vl_Total_NF { get; set; }

        [Column("vl_total_RA", TypeName = "money")]
        public decimal? Vl_Total_RA { get; set; }

        [Column("perc_RT")]
        public float? Perc_RT { get; set; }

        [Column("st_orc_virou_pedido")]
        public short? St_Orc_Virou_Pedido { get; set; }

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
        public string CustoFinancFornecTipoParcelamento { get; set; }

        [Column("custoFinancFornecQtdeParcelas")]
        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }

        [Column("EndEtg_endereco_numero")]
        [MaxLength(60)]
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

        [Column("EndEtg_cod_justificativa")]
        [MaxLength(3)]
        public string EndEtg_Cod_Justificativa { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        [Required]
        public short Pc_Maquineta_Qtde_Parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela", TypeName = "money")]
        [Required]
        public decimal Pc_Maquineta_Valor_Parcela { get; set; }

        public Tcliente Tcliente { get; set; }

        [Column("sistema_responsavel_cadastro")]
        [Required]
        public int Sistema_responsavel_cadastro { get; set; }

        [Column("sistema_responsavel_atualizacao")]
        [Required]
        public int Sistema_responsavel_atualizacao { get; set; }

        /*Novos campos para memorização de endereço*/
        [Column("st_memorizacao_completa_enderecos")]
        [Required]
        public byte St_memorizacao_completa_enderecos { get; set; }

        [Column("endereco_logradouro")]
        [MaxLength(80)]
        public string Endereco_logradouro { get; set; }

        [Column("endereco_numero")]
        [MaxLength(60)]
        public string Endereco_numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_complemento { get; set; }

        [Column("endereco_bairro")]
        [MaxLength(72)]
        public string Endereco_bairro { get; set; }

        [Column("endereco_cidade")]
        [MaxLength(60)]
        public string Endereco_cidade { get; set; }

        [Column("endereco_uf")]
        [MaxLength(2)]
        public string Endereco_uf { get; set; }

        [Column("endereco_cep")]
        [MaxLength(8)]
        public string Endereco_cep { get; set; }

        [Column("endereco_email")]
        [MaxLength(60)]
        public string Endereco_email { get; set; }

        [Column("endereco_email_xml")]
        [MaxLength(60)]
        public string Endereco_email_xml { get; set; }

        [Column("endereco_nome")]
        [MaxLength(60)]
        public string Endereco_nome { get; set; }

        [Column("endereco_ddd_res")]
        [MaxLength(4)]
        public string Endereco_ddd_res { get; set; }

        [Column("endereco_tel_res")]
        [MaxLength(11)]
        public string Endereco_tel_res { get; set; }

        [Column("endereco_ddd_com")]
        [MaxLength(4)]
        public string Endereco_ddd_com { get; set; }

        [Column("endereco_tel_com")]
        [MaxLength(11)]
        public string Endereco_tel_com { get; set; }

        [Column("endereco_ramal_com")]
        [MaxLength(4)]
        public string Endereco_ramal_com { get; set; }

        [Column("endereco_ddd_cel")]
        [MaxLength(2)]
        public string Endereco_ddd_cel { get; set; }

        [Column("endereco_tel_cel")]
        [MaxLength(9)]
        public string Endereco_tel_cel { get; set; }

        [Column("endereco_ddd_com_2")]
        [MaxLength(2)]
        public string Endereco_ddd_com_2 { get; set; }

        [Column("endereco_tel_com_2")]
        [MaxLength(9)]
        public string Endereco_tel_com_2 { get; set; }

        [Column("endereco_ramal_com_2")]
        [MaxLength(4)]
        public string Endereco_ramal_com_2 { get; set; }

        [Column("endereco_tipo_pessoa")]
        [MaxLength(2)]
        public string Endereco_tipo_pessoa { get; set; }

        [Column("endereco_cnpj_cpf")]
        [MaxLength(14)]
        public string Endereco_cnpj_cpf { get; set; }

        [Column("endereco_contribuinte_icms_status")]
        [Required]
        public byte Endereco_contribuinte_icms_status { get; set; }

        [Column("endereco_produtor_rural_status")]
        [Required]
        public byte Endereco_produtor_rural_status { get; set; }

        [Column("endereco_ie")]
        [MaxLength(20)]
        public string Endereco_ie { get; set; }

        [Column("endereco_rg")]
        [MaxLength(20)]
        public string Endereco_rg { get; set; }

        [Column("endereco_contato")]
        [MaxLength(30)]
        public string Endereco_contato { get; set; }

        [Column("EndEtg_email")]
        [MaxLength(60)]
        public string EndEtg_email { get; set; }

        [Column("EndEtg_email_xml")]
        [MaxLength(60)]
        public string EndEtg_email_xml { get; set; }

        [Column("EndEtg_nome")]
        [MaxLength(60)]
        public string EndEtg_nome { get; set; }

        [Column("EndEtg_ddd_res")]
        [MaxLength(4)]
        public string EndEtg_ddd_res { get; set; }

        [Column("EndEtg_tel_res")]
        [MaxLength(11)]
        public string EndEtg_tel_res { get; set; }

        [Column("EndEtg_ddd_com")]
        [MaxLength(4)]
        public string EndEtg_ddd_com { get; set; }

        [Column("EndEtg_tel_com")]
        [MaxLength(11)]
        public string EndEtg_tel_com { get; set; }

        [Column("EndEtg_ramal_com")]
        [MaxLength(4)]
        public string EndEtg_ramal_com { get; set; }

        [Column("EndEtg_ddd_cel")]
        [MaxLength(2)]
        public string EndEtg_ddd_cel { get; set; }

        [Column("EndEtg_tel_cel")]
        [MaxLength(9)]
        public string EndEtg_tel_cel { get; set; }

        [Column("EndEtg_ddd_com_2")]
        [MaxLength(2)]
        public string EndEtg_ddd_com_2 { get; set; }

        [Column("EndEtg_tel_com_2")]
        [MaxLength(9)]
        public string EndEtg_tel_com_2 { get; set; }

        [Column("EndEtg_ramal_com_2")]
        [MaxLength(4)]
        public string EndEtg_ramal_com_2 { get; set; }

        [Column("EndEtg_tipo_pessoa")]
        [MaxLength(2)]
        public string EndEtg_tipo_pessoa { get; set; }

        [Column("EndEtg_cnpj_cpf")]
        [MaxLength(14)]
        public string EndEtg_cnpj_cpf { get; set; }

        [Column("EndEtg_contribuinte_icms_status")]
        [Required]
        public byte EndEtg_contribuinte_icms_status { get; set; }

        [Column("EndEtg_produtor_rural_status")]
        [Required]
        public byte EndEtg_produtor_rural_status { get; set; }

        [Column("EndEtg_ie")]
        [MaxLength(20)]
        public string EndEtg_ie { get; set; }

        [Column("EndEtg_rg")]
        [MaxLength(20)]
        public string EndEtg_rg { get; set; }

        [Column("PrevisaoEntregaData")]
        public DateTime? PrevisaoEntregaData { get; set; }

        [Column("PrevisaoEntregaUsuarioUltAtualiz")]
        [MaxLength(20)]
        public string PrevisaoEntregaUsuarioUltAtualiz { get; set; }

        [Column("PrevisaoEntregaDtHrUltAtualiz")]
        public DateTime? PrevisaoEntregaDtHrUltAtualiz { get; set; }


    }
}
