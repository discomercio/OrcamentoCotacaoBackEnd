using Loja.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Loja.Modelos
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

        [Column("midia")]
        [MaxLength(3)]
        public string Midia { get; set; }

        [Column("servicos")]
        [MaxLength(60)]
        public string Servicos { get; set; }

        [Column("vl_servicos", TypeName = "money")]
        [Required]
        public Decimal Vl_Servicos { get; set; }

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("st_entrega")]
        [MaxLength(3)]
        public string St_Entrega { get; set; }

        [Column("entregue_data")]
        public DateTime? Entregue_Data { get; set; }

        [Column("entregue_usuario")]
        [MaxLength(10)]
        public string Entregue_Usuario { get; set; }

        [Column("cancelado_data")]
        public DateTime? Cancelado_Data { get; set; }

        [Column("cancelado_usuario")]
        [MaxLength(10)]
        public string Cancelado_Usuario { get; set; }

        [Column("st_pagto")]
        [MaxLength(1)]
        public string St_Pagto { get; set; }

        [Column("st_recebido")]
        [MaxLength(1)]
        public string St_Recebido { get; set; }

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
        public string Forma_Pagto { get; set; }

        [Column("vl_total_familia", TypeName = "money")]
        [Required]
        public decimal Vl_Total_Familia { get; set; }

        [Column("vl_pago_familia", TypeName = "money")]
        [Required]
        public decimal Vl_Pagto_Familia { get; set; }

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

        [Column("a_entregar_status")]
        public short? A_Entregar_Status { get; set; }

        [Column("a_entregar_data_marcada")]
        public DateTime? A_Entregar_Data_Marcada { get; set; }

        [Column("a_entregar_data")]
        public DateTime? A_Entregar_Data { get; set; }

        [Column("a_entregar_hora")]
        [MaxLength(6)]
        public string A_Entregar_Hora { get; set; }

        [Column("a_entregar_usuario")]
        [MaxLength(10)]
        public string A_Entregar_Usuario { get; set; }

        //[Column("timestamp")]
        //public byte[] Timestamp { get; set; }

        [Column("loja_indicou")]
        [MaxLength(3)]
        public string Loja_Indicou { get; set; }

        [Column("comissao_loja_indicou")]
        public float? Comissao_Loja_Indicou { get; set; }

        [Column("venda_externa")]
        public short? Venda_Externa { get; set; }

        [Column("vl_frete", TypeName = "money")]
        public decimal? Vl_Frete { get; set; }

        [Column("transportadora_id")]
        [MaxLength(10)]
        public string Transportadora_Id { get; set; }

        [Column("transportadora_data")]
        public DateTime? Transportadora_Data { get; set; }

        [Column("transportadora_usuario")]
        [MaxLength(10)]
        public string Transportadora_Usuario { get; set; }

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

        [Column("vl_total_RA", TypeName = "money")]
        public decimal? Vl_Total_RA { get; set; }

        [Column("perc_RT")]
        public float? Perc_RT { get; set; }

        [Column("st_orc_virou_pedido")]
        public short? St_Orc_Virou_Pedido { get; set; }

        [Column("orcamento")]
        [MaxLength(9)]
        public string Orcamento { get; set; }

        [Column("orcamentista")]
        [MaxLength(20)]
        public string Orcamentista { get; set; }

        [Column("comissao_paga")]
        [Required]
        public short Comissao_Paga { get; set; }

        [Column("comissao_paga_ult_op")]
        [MaxLength(1)]
        public string Comissao_Paga_Ult_Op { get; set; }

        [Column("comissao_paga_data")]
        public DateTime? Comissao_Paga_Data { get; set; }

        [Column("comissao_paga_usuario")]
        [MaxLength(10)]
        public string Comissao_Paga_Usuario { get; set; }

        [Column("perc_desagio_RA")]
        public float? Perc_Desagio_RA { get; set; }

        [Column("perc_limite_RA_sem_desagio")]
        public float? Perc_Limite_RA_Sem_Desagio { get; set; }

        [Column("vl_total_RA_liquido", TypeName = "money")]
        public decimal? Vl_Total_RA_Liquido { get; set; }

        [Column("st_tem_desagio_RA")]
        [Required]
        public short St_Tem_Desagio_RA { get; set; }

        [Column("qtde_parcelas_desagio_RA")]
        [Required]
        public short Qtde_Parcelas_Desagio_RA { get; set; }

        [Column("transportadora_num_coleta")]
        [MaxLength(8)]
        public string Transportadora_Num_Coleta { get; set; }

        [Column("transportadora_contato")]
        [MaxLength(10)]
        public string Transportadora_Contato { get; set; }

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
        public string EndEtg_Cep { get; set; }

        [Column("st_etg_imediata")]
        [Required]
        public short St_Etg_Imediata { get; set; }

        [Column("etg_imediata_data")]
        public DateTime? Etg_Imediata_Data { get; set; }

        [Column("etg_imediata_usuario")]
        [MaxLength(20)]
        public string Etg_Imediata_Usuario { get; set; }

        [Column("frete_status")]
        [Required]
        public short Frete_Status { get; set; }

        [Column("frete_valor", TypeName = "money")]
        [Required]
        public decimal Frete_Valor { get; set; }

        [Column("frete_data")]
        public DateTime? Frete_Data { get; set; }

        [Column("frete_usuario")]
        [MaxLength(10)]
        public string Frete_Usuario { get; set; }

        [Column("StBemUsoConsumo")]
        [Required]
        public short StBemUsoConsumo { get; set; }

        [Column("PedidoRecebidoStatus")]
        [Required]
        public short PedidoRecebidoStatus { get; set; }

        [Column("PedidoRecebidoData")]
        public DateTime? PedidoRecebidoData { get; set; }

        [Column("PedidoRecebidoUsuarioUltAtualiz")]
        [MaxLength(10)]
        public string PedidoRecebidoUsuarioUltAtualiz { get; set; }

        [Column("PedidoRecebidoDtHrUltAtualiz")]
        public DateTime? PedidoRecebidoDtHrUltAtualiz { get; set; }

        [Column("InstaladorInstalaStatus")]
        [Required]
        public short InstaladorInstalaStatus { get; set; }

        [Column("InstaladorInstalaUsuarioUltAtualiz")]
        [MaxLength(10)]
        public string InstaladorInstalaUsuarioUltAtualiz { get; set; }

        [Column("InstaladorInstalaDtHrUltAtualiz")]
        public DateTime? InstaladorInstalaDtHrUltAtualiz { get; set; }

        [Column("custoFinancFornecTipoParcelamento")]
        [MaxLength(2)]
        public string CustoFinancFornecTipoParcelamento { get; set; }

        [Column("custoFinancFornecQtdeParcelas")]
        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }

        [Column("BoletoConfeccionadoStatus")]
        [Required]
        public byte BoletoConfeccionadoStatus { get; set; }

        [Column("BoletoConfeccionadoData")]
        public DateTime? BoletoConfeccionadoData { get; set; }

        [Column("GarantiaIndicadorStatus")]
        [Required]
        public byte GarantiaIndicadorStatus { get; set; }

        [Column("GarantiaIndicadorUsuarioUltAtualiz")]
        [MaxLength(10)]
        public string GarantiaIndicadorUsuarioUltAtualiz { get; set; }

        [Column("GarantiaIndicadorDtHrUltAtualiz")]
        public DateTime? GarantiaIndicadorDtHrUltAtualiz { get; set; }

        [Column("EndEtg_endereco_numero")]
        [MaxLength(20)]
        public string EndEtg_Endereco_Numero { get; set; }

        [Column("EndEtg_endereco_complemento")]
        [MaxLength(60)]
        public string EndEtg_Endereco_Complemento { get; set; }

        [Column("romaneio_status")]
        [Required]
        public byte Romaneio_Status { get; set; }

        [Column("romaneio_data")]
        public DateTime? Romaneio_Data { get; set; }

        [Column("romaneio_data_hora")]
        public DateTime? Romaneio_Data_Hora { get; set; }


        [Column("romaneio_usuario")]
        [MaxLength(10)]
        public string Romaneio_Usuario { get; set; }

        [Column("danfe_impressa_status")]
        [Required]
        public byte Danfe_Impressa_Status { get; set; }

        [Column("danfe_impressa_data")]
        public DateTime? Danfe_Impressa_Data { get; set; }

        [Column("danfe_impressa_data_hora")]
        public DateTime? Danfe_Impressa_Data_Hora { get; set; }

        [Column("danfe_impressa_usuario")]
        [MaxLength(10)]
        public string Danfe_Impressa_Usuario { get; set; }

        [Column("transportadora_conferente")]
        [MaxLength(30)]
        public string Transportadora_Conferente { get; set; }

        [Column("transportadora_motorista")]
        [MaxLength(30)]
        public string Transportadora_Motorista { get; set; }

        [Column("transportadora_placa_veiculo")]
        [MaxLength(9)]
        public string Transportadora_Placa_Veiculo { get; set; }

        [Column("perc_desagio_RA_liquida")]
        [Required]
        public float Perc_Desagio_RA_Liquida { get; set; }

        [Column("indicador_editado_manual_status")]
        [Required]
        public byte Indicador_Editado_Manual_Status { get; set; }

        [Column("indicador_editado_manual_data_hora")]
        public DateTime? Indicador_Editado_Manual_Data_Hora { get; set; }

        [Column("indicador_editado_manual_usuario")]
        [MaxLength(10)]
        public string Indicador_Editado_Manual_Usuario { get; set; }

        [Column("indicador_editado_manual_indicador_original")]
        [MaxLength(20)]
        public string Indicador_Editado_Manual_Indicaro_Original { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }

        [Column("st_violado_permite_RA_status")]
        [Required]
        public short St_Violado_Permite_RA_Status { get; set; }

        [Column("dt_hr_violado_permite_RA_status")]
        public DateTime? Dt_Hr_Violado_Permite_RA_Status { get; set; }

        [Column("usuario_violado_permite_RA_status")]
        [MaxLength(10)]
        public string Usuario_Violado_Permite_RA_Status { get; set; }

        [Column("opcao_possui_RA")]
        [MaxLength(1)]
        public string Opcao_Possui_RA { get; set; }

        [Column("tamanho_num_pedido")]
        public int? Tamanho_Num_Pedido { get; set; }

        [Column("pedido_base")]
        [MaxLength(7)]
        public string Pedido_Base { get; set; }

        [Column("numero_loja")]
        public short? Numero_Loja { get; set; }

        [Column("data_hora")]
        public DateTime? Data_Hora { get; set; }

        [Column("st_forma_pagto_somente_cartao")]
        public byte? St_Forma_Pagto_Somente_Cartao { get; set; }

        [Column("endereco_memorizado_status")]
        [Required]
        public byte Endereco_Memorizado_Status { get; set; }

        [Column("endereco_logradouro")]
        [MaxLength(80)]
        public string Endereco_Logradouro { get; set; }

        [Column("endereco_bairro")]
        [MaxLength(72)]
        public string Endereco_Bairro { get; set; }

        [Column("endereco_cidade")]
        [MaxLength(60)]
        public string Endereco_Cidade { get; set; }

        [Column("endereco_uf")]
        [MaxLength(2)]
        public string Endereco_Uf { get; set; }

        [Column("endereco_cep")]
        [MaxLength(8)]
        public string Endereco_Cep { get; set; }

        [Column("endereco_numero")]
        [MaxLength(20)]
        public string Endereco_Numero { get; set; }

        [Column("endereco_complemento")]
        [MaxLength(60)]
        public string Endereco_Complemento { get; set; }

        [Column("analise_endereco_tratar_status")]
        [Required]
        public byte Analise_Endereco_Tratar_Status { get; set; }

        [Column("analise_endereco_tratado_status")]
        [Required]
        public byte Analise_Endereco_Tratado_Status { get; set; }

        [Column("analise_endereco_tratado_data")]
        public DateTime? Analise_Endereco_Tratado_Data { get; set; }

        [Column("analise_endereco_tratado_data_hora")]
        public DateTime? Analise_Endereco_Tratado_Data_Hora { get; set; }

        [Column("analise_endereco_tratado_usuario")]
        [MaxLength(10)]
        public string Analise_Endereco_Tratado_Usuario { get; set; }

        [Column("analise_credito_data_sem_hora")]
        public DateTime? Analise_Credito_Data_Sem_Hora { get; set; }

        [Column("cancelado_auto_status")]
        [Required]
        public byte Cancelado_Auto_Status { get; set; }

        [Column("cancelado_auto_data")]
        public DateTime? Cancelado_Auto_Data { get; set; }

        [Column("cancelado_auto_data_hora")]
        public DateTime? Cancelado_Auto_Data_Hora { get; set; }

        [Column("cancelado_auto_motivo")]
        [MaxLength(160)]
        public string Cancel_Auto_Motivo { get; set; }

        [Column("obs_3")]
        [MaxLength(3)]
        public string Obs_3 { get; set; }

        [Column("danfe_a_imprimir_status")]
        [Required]
        public byte Danfe_A_Imprimir_Status { get; set; }

        [Column("danfe_a_imprimir_data_hora")]
        public DateTime? Danfe_A_Imprimir_Data_Hora { get; set; }

        [Column("danfe_a_imprimir_usuario")]
        [MaxLength(10)]
        public string Danfe_A_Imprimir_Usuario { get; set; }

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

        [Column("pedido_bs_x_at")]
        [MaxLength(9)]
        public string Pedido_Bs_X_At { get; set; }

        [Column("cancelado_data_hora")]
        public DateTime? Cancelado_Data_Hora { get; set; }

        [Column("cancelado_codigo_motivo")]
        [MaxLength(3)]
        public string Cancelado_Codigo_Motivo { get; set; }

        [Column("cancelado_codigo_sub_motivo")]
        [MaxLength(3)]
        public string Cancelado_Codigo_Sub_Motivo { get; set; }

        [Column("cancelado_motivo")]
        [MaxLength(800)]
        public string Cancelado_Motivo { get; set; }

        [Column("EndEtg_obs")]
        [MaxLength(100)]
        public string EndEtg_Obs { get; set; }

        [Column("pedido_bs_x_ac")]
        [MaxLength(9)]
        public string Pedido_Bs_X_Ac { get; set; }

        [Column("pedido_bs_x_ac_reverso")]
        [MaxLength(9)]
        public string Pedido_Bs_X_Ac_Reverso { get; set; }

        [Column("EndEtg_cod_justificativa")]
        [MaxLength(3)]
        public string EndEtg_Cod_Justificativa { get; set; }

        [Column("pedido_bs_x_marketplace")]
        [MaxLength(20)]
        public string Pedido_Bs_X_Marketplace { get; set; }

        [Column("marketplace_codigo_origem")]
        [MaxLength(3)]
        public string Marketplace_codigo_origem { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_Nfe_Emitente { get; set; }

        [Column("st_pedido_novo_analise_credito_msg_alerta")]
        [Required]
        public byte St_Pedido_Novo_Analise_Credito_Msg_Alerta { get; set; }

        [Column("dt_hr_pedido_novo_analise_credito_msg_alerta")]
        public DateTime? Dt_Hr_Pedido_Novo_Analise_Credito_Msg_Alerta { get; set; }

        [Column("st_forma_pagto_possui_parcela_cartao")]
        public byte? St_Forma_Pagto_Possui_Parcela_Cartao { get; set; }

        [Column("vl_previsto_cartao", TypeName = "money")]
        public decimal? Vl_Previsto_Cartao { get; set; }

        [Column("NFe_texto_constar")]
        [MaxLength(800)]
        public string Nfe_Texto_Constar { get; set; }

        [Column("NFe_xPed")]
        [MaxLength(15)]
        public string Nfe_XPed { get; set; }

        [Column("MarketplacePedidoRecebidoRegistrarStatus")]
        [Required]
        public byte MarketplacePedidoRecebidoRegistrarStatus { get; set; }

        [Column("MarketplacePedidoRecebidoRegistrarDataRecebido")]
        public DateTime? MarketplacePedidoRecebidoRegistrarDataRecebido { get; set; }

        [Column("MarketplacePedidoRecebidoRegistrarDataHora")]
        public DateTime? MarketplacePedidoRecebidoRegistrarDataHora { get; set; }

        [Column("MarketplacePedidoRecebidoRegistrarUsuario")]
        [MaxLength(10)]
        public string MarketplacePedidoRecebidoRegistrarUsuario { get; set; }

        [Column("MarketplacePedidoRecebidoRegistradoStatus")]
        [Required]
        public byte MarketplacePedidoRecebidoRegistradoStatus { get; set; }

        [Column("MarketplacePedidoRecebidoRegistradoDataHora")]
        public DateTime? MarketplacePedidoRecebidoRegistradoDataHora { get; set; }

        [Column("MarketplacePedidoRecebidoRegistradoUsuario")]
        [MaxLength(10)]
        public string MarketplacePedidoRecebidoRegistradoUsuario { get; set; }

        [Column("st_auto_split")]
        [Required]
        public byte St_Auto_Split { get; set; }

        [Column("analise_credito_pendente_vendas_motivo")]
        [MaxLength(3)]
        public string Analise_Credito_Pendente_Vendas_Motivo { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_Cadastro { get; set; }

        [Column("plataforma_origem_pedido")]
        [Required]
        public int Plataforma_Origem_Pedido { get; set; }

        [Column("magento_installer_commission_value", TypeName = "decimal(18,4)")]
        [Required]
        public decimal Magento_Installer_Comission_Value { get; set; }

        [Column("magento_installer_commission_discount", TypeName = "decimal(18,4)")]
        [Required]
        public decimal Magento_Installer_Comission_Discount { get; set; }

        [Column("magento_shipping_amount", TypeName = "decimal(18,4)")]
        [Required]
        public decimal Magento_Shipping_Amount { get; set; }

        [Column("dt_st_pagto")]
        public DateTime? Dt_St_Pagto { get; set; }

        [Column("dt_hr_st_pagto")]
        public DateTime? Dt_Hr_St_Pagto { get; set; }

        [Column("usuario_st_pagto")]
        [MaxLength(20)]
        public string Usuario_St_Pagto { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        [Required]
        public short Pc_Maquineta_Qtde_Parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela", TypeName = "money")]
        [Required]
        public decimal Pc_Maquineta_Valor_Parcela { get; set; }

        public Tcliente Tcliente { get; set; }
    }
}