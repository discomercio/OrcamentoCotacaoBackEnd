using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO")]
    public class Tpedido
    {
        [Key]
        [Required]
        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

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

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("st_entrega")]
        [MaxLength(3)]
        public string St_Entrega { get; set; }

        [Column("entregue_data")]
        public DateTime? Entregue_Data { get; set; }

        [Column("cancelado_data")]
        public DateTime? Cancelado_Data { get; set; }

        [Column("st_pagto")]
        [MaxLength(1)]
        public string St_Pagto { get; set; }

        [Column("obs_1")]
        [MaxLength(500)]
        public string Obs_1 { get; set; }

        [Column("obs_2")]
        [MaxLength(10)]
        public string Obs_2 { get; set; }

        [Column("forma_pagto")]
        [MaxLength(250)]
        public string Forma_Pagto { get; set; }

        [Column("vl_total_familia", TypeName = "money")]
        [Required]
        public decimal Vl_Total_Familia { get; set; }

        [Column("a_entregar_data_marcada")]
        public DateTime? A_Entregar_Data_Marcada { get; set; }

        [Column("transportadora_id")]
        [MaxLength(10)]
        public string Transportadora_Id { get; set; }

        [Column("analise_credito")]
        [Required]
        public short Analise_Credito { get; set; }

        [Column("analise_credito_data")]
        public DateTime? Analise_credito_Data { get; set; }

        [Column("analise_credito_usuario")]
        [MaxLength(10)]
        public string Analise_Credito_Usuario { get; set; }

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
        public decimal? Pse_Demais_Prest_Valor { get; set; }

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

        [Column("indicador")]
        [MaxLength(20)]
        public string Indicador { get; set; }

        [Column("vl_total_NF", TypeName = "money")]
        public decimal? Vl_Total_NF { get; set; }

        [Column("perc_RT")]
        public float? Perc_RT { get; set; }

        [Column("orcamento")]
        [MaxLength(9)]
        public string Orcamento { get; set; }

        [Column("orcamentista")]
        [MaxLength(20)]
        public string Orcamentista { get; set; }

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
        public string EndEtg_Cep { get; set; }

        [Column("st_etg_imediata")]
        [Required]
        public short St_Etg_Imediata { get; set; }

        [Column("etg_imediata_data")]
        public DateTime? Etg_Imediata_Data { get; set; }

        [Column("etg_imediata_usuario")]
        [MaxLength(20)]
        public string Etg_Imediata_Usuario { get; set; }

        [Column("frete_valor", TypeName = "money")]
        [Required]
        public decimal Frete_Valor { get; set; }

        [Column("StBemUsoConsumo")]
        [Required]
        public short StBemUsoConsumo { get; set; }

        [Column("PedidoRecebidoStatus")]
        [Required]
        public short PedidoRecebidoStatus { get; set; }

        [Column("PedidoRecebidoData")]
        public DateTime? PedidoRecebidoData { get; set; }

        [Column("InstaladorInstalaStatus")]
        [Required]
        public short InstaladorInstalaStatus { get; set; }

        [Column("GarantiaIndicadorStatus")]
        [Required]
        public byte GarantiaIndicadorStatus { get; set; }

        [Column("EndEtg_endereco_numero")]
        [MaxLength(60)]
        public string EndEtg_Endereco_Numero { get; set; }

        [Column("EndEtg_endereco_complemento")]
        [MaxLength(60)]
        public string EndEtg_Endereco_Complemento { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }

        [Column("opcao_possui_RA")]
        [MaxLength(1)]
        public string Opcao_Possui_RA { get; set; }

        [Column("obs_3")]
        [MaxLength(3)]
        public string Obs_3 { get; set; }

        [Column("pedido_bs_x_ac")]
        [MaxLength(9)]
        public string Pedido_Bs_X_Ac { get; set; }

        [Column("EndEtg_cod_justificativa")]
        [MaxLength(3)]
        public string EndEtg_Cod_Justificativa { get; set; }

        [Column("pedido_bs_x_marketplace")]
        [MaxLength(20)]
        public string Pedido_Bs_X_Marketplace { get; set; }

        [Column("marketplace_codigo_origem")]
        [MaxLength(3)]
        public string Marketplace_codigo_origem { get; set; }

        [Column("NFe_texto_constar")]
        [MaxLength(800)]
        public string Nfe_Texto_Constar { get; set; }

        [Column("NFe_xPed")]
        [MaxLength(15)]
        public string Nfe_XPed { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        [Required]
        public short Pc_Maquineta_Qtde_Parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela", TypeName = "money")]
        [Required]
        public decimal Pc_Maquineta_Valor_Parcela { get; set; }
        public Tcliente Tcliente { get; set; }

        /*Novos campos de memorização de endereço */
        [Column("st_memorizacao_completa_enderecos")]
        [Required]
        public byte St_memorizacao_completa_enderecos { get; set; }

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

        [Column("endereco_memorizado_status")]
        [Required]
        public byte Endereco_memorizado_status { get; set; }

        [Column("endereco_logradouro")]
        [MaxLength(80)]
        public string Endereco_logradouro { get; set; }

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

        [Column("endereco_numero")]
        [MaxLength(60)]
        public string Endereco_numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_complemento { get; set; }


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

        [Column("IdOrcamentoCotacao")]
        public int? IdOrcamentoCotacao { get; set; }

        [Column("IdIndicadorVendedor")]
        public int? IdIndicadorVendedor { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("data_hora")]
        public DateTime? Data_Hora { get; private set; }

        [Column("st_end_entrega")]
        [Required]
        public short St_End_Entrega { get; set; }

        [Column("vl_total_RA", TypeName = "money")]
        public decimal? Vl_Total_RA { get; set; }


        [Column("qtde_parcelas")]
        public short? Qtde_Parcelas { get; set; }


        [Column("servicos")]
        [MaxLength(60)]
        public string Servicos { get; set; }

        public ICollection<TpedidoItemDevolvido> TpedidoItemDevolvido { get; set; }

        [Column("vl_frete")]
        public decimal? Vl_Frete { get; set; }

        [Column("st_auto_split")]
        [Required]
        public byte St_Auto_Split { get; set; }

        [Column("dt_st_pagto")]
        public DateTime? Dt_St_Pagto { get; set; }

        [Column("dt_hr_st_pagto")]
        public DateTime? Dt_Hr_St_Pagto { get; set; }

        [Column("usuario_st_pagto")]
        [MaxLength(20)]
        public string Usuario_St_Pagto { get; set; }

        [Column("st_recebido")]
        [MaxLength(1)]
        public string St_Recebido { get; set; }

        [Column("custoFinancFornecQtdeParcelas")]
        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }

        [Column("analise_credito_pendente_vendas_motivo")]
        [MaxLength(3)]
        public string Analise_Credito_Pendente_Vendas_Motivo { get; set; }

        [Column("custoFinancFornecTipoParcelamento")]
        [MaxLength(2)]
        public string CustoFinancFornecTipoParcelamento { get; set; }

        [Column("perc_desagio_RA")]
        public float? Perc_Desagio_RA { get; set; }

        [Column("perc_limite_RA_sem_desagio")]
        public float? Perc_Limite_RA_Sem_Desagio { get; set; }

        [Column("midia")]
        [MaxLength(3)]
        public string Midia { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Cadastro { get; set; }

        [Column("magento_shipping_amount")]
        [Required]
        public decimal Magento_shipping_amount { get; set; }

        [Column("pedido_bs_x_at")]
        [MaxLength(9)]
        public string Pedido_Bs_X_At { get; set; }

        [Column("InstaladorInstalaUsuarioUltAtualiz")]
        [MaxLength(10)]
        public string InstaladorInstalaUsuarioUltAtualiz { get; set; }

        [Column("InstaladorInstalaDtHrUltAtualiz")]
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }

        [Column("venda_externa")]
        public short? Venda_Externa { get; set; }

        [Column("loja_indicou")]
        [MaxLength(3)]
        public string Loja_Indicou { get; set; }

        [Column("comissao_loja_indicou")]
        public float? Comissao_Loja_Indicou { get; set; }

        [Column("GarantiaIndicadorUsuarioUltAtualiz")]
        [MaxLength(10)]
        public string GarantiaIndicadorUsuarioUltAtualiz { get; set; }

        [Column("GarantiaIndicadorDtHrUltAtualiz")]
        public DateTime? GarantiaIndicadorDtHrUltAtualiz { get; set; }

        [Column("sistema_responsavel_atualizacao")]
        [Required]
        public int Sistema_responsavel_atualizacao { get; set; }

        [Column("sistema_responsavel_cadastro")]
        [Required]
        public int Sistema_responsavel_cadastro { get; set; }

        [Column("perc_desagio_RA_liquida")]
        [Required]
        public float Perc_Desagio_RA_Liquida { get; set; }

        [Column("plataforma_origem_pedido")]
        [Required]
        public int Plataforma_Origem_Pedido { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_Nfe_Emitente { get; set; }

        [Column("vl_total_RA_liquido", TypeName = "money")]
        public decimal? Vl_Total_RA_Liquido { get; set; }

        [Column("qtde_parcelas_desagio_RA")]
        [Required]
        public short Qtde_Parcelas_Desagio_RA { get; set; }

        [Column("st_tem_desagio_RA")]
        [Required]
        public short St_Tem_Desagio_RA { get; set; }

        [Column("analise_endereco_tratar_status")]
        [Required]
        public byte Analise_Endereco_Tratar_Status { get; set; }

        [Column("transportadora_data")]
        public DateTime? Transportadora_Data { get; set; }

        [Column("transportadora_usuario")]
        [MaxLength(10)]
        public string Transportadora_Usuario { get; set; }

        [Column("transportadora_selecao_auto_status")]
        [Required]
        public byte Transportadora_Selecao_Auto_Status { get; set; }

        [Column("transportadora_selecao_auto_cep")]
        [MaxLength(8)]
        public string Transportadora_Selecao_Auto_Cep { get; set; }

        [Column("transportadora_selecao_auto_tipo_endereco")]
        [Required]
        public byte Transportadora_Selecao_Auto_Tipo_Endereco { get; set; }

        [Column("transportadora_selecao_auto_transportadora")]
        [MaxLength(10)]
        public string Transportadora_Selecao_Auto_Transportadora { get; set; }

        [Column("transportadora_selecao_auto_data_hora")]
        public DateTime? Transportadora_Selecao_Auto_Data_Hora { get; set; }

        [Column("split_status")]
        public short? Split_Status { get; set; }

        [Column("split_data")]
        public DateTime? Split_Data { get; set; }

        [Column("split_hora")]
        [MaxLength(6)]
        public string Split_Hora { get; set; }

        [Column("split_usuario")]
        [MaxLength(10)]
        public string Split_Usuario { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("st_forma_pagto_possui_parcela_cartao_maquineta")]
        public byte? St_forma_pagto_possui_parcela_cartao_maquineta { get; private set; }

        //inseridos pelo 0192-atualiza-PagtoAntecipado.sql
        [Column("PagtoAntecipadoStatus")]
        public Int16 PagtoAntecipadoStatus { get; set; }

        [Column("PagtoAntecipadoDataHora")]
        public DateTime? PagtoAntecipadoDataHora { get; set; }

        [Column("PagtoAntecipadoUsuario")]
        [MaxLength(10)]
        public string PagtoAntecipadoUsuario { get; set; }

        [Column("PagtoAntecipadoQuitadoStatus")]
        public Int16 PagtoAntecipadoQuitadoStatus { get; set; }

        [Column("PagtoAntecipadoQuitadoDataHora")]
        public DateTime? PagtoAntecipadoQuitadoDataHora { get; set; }

        [Column("PagtoAntecipadoQuitadoUsuario")]
        [MaxLength(10)]
        public string PagtoAntecipadoQuitadoUsuario { get; set; }

        [Column("obs_4")]
        [MaxLength(10)]
        public string Obs_4 { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("num_obs_4")]
        public Int32 Num_obs_4 { get; private set; }

        [Column("perc_max_comissao_padrao")]
        public float? Perc_max_comissao_padrao { get; set; }

        [Column("perc_max_comissao_e_desconto_padrao")]
        public float? Perc_max_comissao_e_desconto_padrao { get; set; }
    }
}

