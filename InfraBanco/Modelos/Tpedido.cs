using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


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
        [MaxLength(20)]
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
        //[Column("st_memorizacao_completa_enderecos")] => TALVEZ NÃO SERÁ UTILIZADO
        //[Required]
        //public byte St_memorizacao_completa_enderecos { get; set; }

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