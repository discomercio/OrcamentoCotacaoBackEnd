using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    #region Comentários
    /// <summary>
    /// Nos campos Op_av_forma_pagto, Op_pu_forma_pagto, Op_pce_entrada_forma_pagto, 
    /// Op_pce_prestacao_forma_pagto, Op_pse_prim_prest_forma_pagto, Op_pse_demais_prest_forma_pagto 
    /// deve ser informado o código do meio de pagamento usado:
    /// <br/> 
    /// ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2", ID_FORMA_PAGTO_CHEQUE = "3", 
    /// ID_FORMA_PAGTO_BOLETO = "4", ID_FORMA_PAGTO_CARTAO = "5", ID_FORMA_PAGTO_BOLETO_AV = "6", 
    /// ID_FORMA_PAGTO_CARTAO_MAQUINETA = "7".
    /// <br/>
    /// <br/>
    /// Op_av_forma_pagto => A Vista
    /// <br/>
    /// <br/>
    /// Parcela Única => Op_pu_forma_pagto, C_pu_valor, C_pu_vencto_apos 
    /// <br/>
    /// <br/>
    /// Cartão de crédito (Pagamento via internet) => C_pc_qtde, C_pc_valor  
    /// <br/>
    /// <br/>
    /// Cartão de crédito (Pagamento na maquineta) => C_pc_maquineta_qtde, C_pc_maquineta_valor  
    /// <br/>
    /// <br/>
    /// Parcelado Com Entrada => Op_pce_entrada_forma_pagto, C_pce_entrada_valor, Op_pce_prestacao_forma_pagto, C_pce_prestacao_qtde, 
    /// C_pce_prestacao_valor, C_pce_prestacao_periodo  
    /// <br/>
    /// <br/>
    /// Parcelado Sem Entrada => Op_pse_prim_prest_forma_pagto, C_pse_prim_prest_valor, C_pse_prim_prest_apos, 
    /// Op_pse_demais_prest_forma_pagto, C_pse_demais_prest_qtde, C_pse_demais_prest_valor, C_pse_demais_prest_periodo 
    /// <br/>
    /// <br/>
    /// </summary>
    #endregion
    public class FormaPagtoCriacaoUnisDto
    {
        /// <summary>
        /// Tipo_Parcelamento:
        ///     COD_FORMA_PAGTO_A_VISTA = "1",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
        ///     COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA = "3",
        ///     COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA = "4",
        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA = "6";
        /// </summary>
        [Required]
        public short Tipo_Parcelamento { get; set; }//Tipo da forma de pagto

        /// <summary>
        /// Opção da forma de pagamento para tipo Á Vista
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_av_forma_pagto { get; set; }

        /// <summary>
        /// Opção da forma de pagamento para tipo Parcela Única
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pu_forma_pagto { get; set; }

        /// <summary>
        /// Valor para pagamento para Parcela Única
        /// </summary>
        public decimal? C_pu_valor { get; set; }

        /// <summary>
        /// Prazo de vencimento para pagamento Parcela Única
        /// </summary>
        public int? C_pu_vencto_apos { get; set; }

        /// <summary>
        /// Quantidade de parcelas para pagamento Cartão de crédito (pagamento via Internet)
        /// </summary>
        public int? C_pc_qtde { get; set; }

        /// <summary>
        /// Valor de parcelas para pagamento Cartão de crédito (pagamento via Internet)
        /// </summary>
        public decimal? C_pc_valor { get; set; }

        /// <summary>
        /// Quantidade de parcelas para pagamento Cartão de crédito (Pagamento na Maquineta)
        /// </summary>
        public int? C_pc_maquineta_qtde { get; set; }

        /// <summary>
        /// Valor de parcelas para pagamento Cartão de crédito (Pagamento na Maquineta)
        /// </summary>
        public decimal? C_pc_maquineta_valor { get; set; }

        /// <summary>
        /// Opção da forma de pagamento da entrada para tipo Pagamento com Entrada
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pce_entrada_forma_pagto { get; set; }

        /// <summary>
        /// Valor da entrada para pagamento tipo Pagamento com Entrada 
        /// </summary>
        public decimal? C_pce_entrada_valor { get; set; }

        /// <summary>
        /// Opção da forma de pagamento da prestação para tipo Pagamento com Entrada
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pce_prestacao_forma_pagto { get; set; }

        /// <summary>
        /// Quantidade de parcela de prestação para tipo Pagamento com Entrada
        /// </summary>
        public int? C_pce_prestacao_qtde { get; set; }

        /// <summary>
        /// Valor da parcela da prestação para tipo Pagamento com Entrada
        /// </summary>
        public decimal? C_pce_prestacao_valor { get; set; }

        /// <summary>
        /// Dias para vencimento da parcela da prestação para tipo Pagamento com Entrada
        /// </summary>
        public int? C_pce_prestacao_periodo { get; set; }

        /// <summary>
        /// Opção da forma de pagamento da primeira prestação para tipo Pagamento sem Entrada
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pse_prim_prest_forma_pagto { get; set; }//Parcelado sem entrada

        /// <summary>
        /// Valor da primeira prestação para tipo Pagamento sem Entrada
        /// </summary>
        public decimal? C_pse_prim_prest_valor { get; set; }

        /// <summary>
        /// Vancimento da primeira prestação para tipo Pagamento sem Entrada
        /// </summary>
        public int? C_pse_prim_prest_apos { get; set; }

        /// <summary>
        /// Opção da forma de pagamento para demais prestações para tipo Pagamento sem Entrada
        /// <br/>
        /// ex: ID_FORMA_PAGTO_DINHEIRO = "1", ID_FORMA_PAGTO_DEPOSITO = "2"
        /// <br/>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pse_demais_prest_forma_pagto { get; set; }

        /// <summary>
        /// Quantidade de parcela para demais prestações para tipo Pagamento sem Entrada
        /// </summary>
        public int? C_pse_demais_prest_qtde { get; set; }

        /// <summary>
        /// Valor da parcela para demais prestações para tipo Pagamento sem Entrada
        /// </summary>
        public decimal? C_pse_demais_prest_valor { get; set; }

        /// <summary>
        /// Vencimento da parcela para demais prestações para tipo Pagamento sem Entrada
        /// </summary>
        public int? C_pse_demais_prest_periodo { get; set; }

        /// <summary>
        /// Descrição da forma de pagamento
        /// </summary>
        [MaxLength(250)]
        public string C_forma_pagto { get; set; }//Descrição da forma de pagto

        /// <summary>
        /// Tipo_parcelamento: 
        ///     COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA = "CE", 
        ///     COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA = "SE",
        ///     COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA = "AV"
        /// </summary>
        [MaxLength(2)]
        public string CustoFinancFornecTipoParcelamento { get; set; }

        /// <summary>
        /// Quantidade de parcelas (sem contar a parcela de entrada). 
        /// <br/>
        /// Ex: 5 = (0+5) ou(1+5)
        /// <br/>
        /// 0 = somente no caso de ser à vista
        /// </summary>
        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }

        public static FormaPagtoCriacaoUnisDto FormaPagtoCriacaoUnisDtoDeFormaPagtoCriacaoDto(FormaPagtoCriacaoDto fpCriacao,
            string custoFinancFornecTipoParcelamento, short custoFinancFornecQtdeParcelas)
        {
            var ret = new FormaPagtoCriacaoUnisDto()
            {
                //Tipo_Parcelamento = 
                Op_av_forma_pagto = fpCriacao.Op_av_forma_pagto,
                Op_pu_forma_pagto = fpCriacao.Op_pu_forma_pagto,
                C_pu_valor = fpCriacao.C_pu_valor,
                C_pu_vencto_apos = fpCriacao.C_pu_vencto_apos,
                C_pc_qtde = fpCriacao.C_pc_qtde,
                C_pc_valor = fpCriacao.C_pc_valor,
                C_pc_maquineta_qtde = fpCriacao.C_pc_maquineta_qtde,
                C_pc_maquineta_valor = fpCriacao.C_pc_maquineta_valor,
                Op_pce_entrada_forma_pagto = fpCriacao.Op_pce_entrada_forma_pagto,
                C_pce_entrada_valor = fpCriacao.C_pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = fpCriacao.Op_pce_prestacao_forma_pagto,
                C_pce_prestacao_qtde = fpCriacao.C_pce_prestacao_qtde,
                C_pce_prestacao_valor = fpCriacao.C_pce_prestacao_valor,
                C_pce_prestacao_periodo = fpCriacao.C_pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = fpCriacao.Op_pse_prim_prest_forma_pagto,
                C_pse_prim_prest_valor = fpCriacao.C_pse_prim_prest_valor,
                C_pse_prim_prest_apos = fpCriacao.C_pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = fpCriacao.Op_pse_demais_prest_forma_pagto,
                C_pse_demais_prest_qtde = fpCriacao.C_pse_demais_prest_qtde,
                C_pse_demais_prest_valor = fpCriacao.C_pse_demais_prest_valor,
                C_pse_demais_prest_periodo = fpCriacao.C_pse_demais_prest_periodo,
                C_forma_pagto = fpCriacao.C_forma_pagto,
                CustoFinancFornecTipoParcelamento = custoFinancFornecTipoParcelamento,
                CustoFinancFornecQtdeParcelas = custoFinancFornecQtdeParcelas
            };

            return ret;
        }

        public static FormaPagtoCriacaoDto FormaPagtoCriacaoDtoDeFormaPagtoCriacaoUnisDto(FormaPagtoCriacaoUnisDto fpCriacaoUnis)
        {
            var ret = new FormaPagtoCriacaoDto()
            {
                Qtde_Parcelas = fpCriacaoUnis.CustoFinancFornecQtdeParcelas,
                Rb_forma_pagto = fpCriacaoUnis.Tipo_Parcelamento.ToString(),
                Op_av_forma_pagto = fpCriacaoUnis.Op_av_forma_pagto,
                Op_pu_forma_pagto = fpCriacaoUnis.Op_pu_forma_pagto,
                C_pu_valor = fpCriacaoUnis.C_pu_valor,
                C_pu_vencto_apos = fpCriacaoUnis.C_pu_vencto_apos,
                C_pc_qtde = fpCriacaoUnis.C_pc_qtde,
                C_pc_valor = fpCriacaoUnis.C_pc_valor,
                C_pc_maquineta_qtde = fpCriacaoUnis.C_pc_maquineta_qtde,
                C_pc_maquineta_valor = fpCriacaoUnis.C_pc_maquineta_valor,
                Op_pce_entrada_forma_pagto = fpCriacaoUnis.Op_pce_entrada_forma_pagto,
                C_pce_entrada_valor = fpCriacaoUnis.C_pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = fpCriacaoUnis.Op_pce_prestacao_forma_pagto,
                C_pce_prestacao_qtde = fpCriacaoUnis.C_pce_prestacao_qtde,
                C_pce_prestacao_valor = fpCriacaoUnis.C_pce_prestacao_valor,
                C_pce_prestacao_periodo = fpCriacaoUnis.C_pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = fpCriacaoUnis.Op_pse_prim_prest_forma_pagto,
                C_pse_prim_prest_valor = fpCriacaoUnis.C_pse_prim_prest_valor,
                C_pse_prim_prest_apos = fpCriacaoUnis.C_pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = fpCriacaoUnis.Op_pse_demais_prest_forma_pagto,
                C_pse_demais_prest_qtde = fpCriacaoUnis.C_pse_demais_prest_qtde,
                C_pse_demais_prest_valor = fpCriacaoUnis.C_pse_demais_prest_valor,
                C_pse_demais_prest_periodo = fpCriacaoUnis.C_pse_demais_prest_periodo,
                C_forma_pagto = fpCriacaoUnis.C_forma_pagto,
                CustoFinancFornecTipoParcelamento = fpCriacaoUnis.CustoFinancFornecTipoParcelamento
            };

            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacaoDadosDeFormaPagtoCriacaoUnisDto(FormaPagtoCriacaoUnisDto fpCriacaoUnis)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados()
            {
                Qtde_Parcelas = fpCriacaoUnis.CustoFinancFornecQtdeParcelas,
                Rb_forma_pagto = fpCriacaoUnis.Tipo_Parcelamento.ToString(),
                Op_av_forma_pagto = fpCriacaoUnis.Op_av_forma_pagto,
                Op_pu_forma_pagto = fpCriacaoUnis.Op_pu_forma_pagto,
                C_pu_valor = fpCriacaoUnis.C_pu_valor,
                C_pu_vencto_apos = fpCriacaoUnis.C_pu_vencto_apos,
                C_pc_qtde = fpCriacaoUnis.C_pc_qtde,
                C_pc_valor = fpCriacaoUnis.C_pc_valor,
                C_pc_maquineta_qtde = fpCriacaoUnis.C_pc_maquineta_qtde,
                C_pc_maquineta_valor = fpCriacaoUnis.C_pc_maquineta_valor,
                Op_pce_entrada_forma_pagto = fpCriacaoUnis.Op_pce_entrada_forma_pagto,
                C_pce_entrada_valor = fpCriacaoUnis.C_pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = fpCriacaoUnis.Op_pce_prestacao_forma_pagto,
                C_pce_prestacao_qtde = fpCriacaoUnis.C_pce_prestacao_qtde,
                C_pce_prestacao_valor = fpCriacaoUnis.C_pce_prestacao_valor,
                C_pce_prestacao_periodo = fpCriacaoUnis.C_pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = fpCriacaoUnis.Op_pse_prim_prest_forma_pagto,
                C_pse_prim_prest_valor = fpCriacaoUnis.C_pse_prim_prest_valor,
                C_pse_prim_prest_apos = fpCriacaoUnis.C_pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = fpCriacaoUnis.Op_pse_demais_prest_forma_pagto,
                C_pse_demais_prest_qtde = fpCriacaoUnis.C_pse_demais_prest_qtde,
                C_pse_demais_prest_valor = fpCriacaoUnis.C_pse_demais_prest_valor,
                C_pse_demais_prest_periodo = fpCriacaoUnis.C_pse_demais_prest_periodo,
                C_forma_pagto = fpCriacaoUnis.C_forma_pagto,
                CustoFinancFornecTipoParcelamento = fpCriacaoUnis.CustoFinancFornecTipoParcelamento
            };

            return ret;
        }
    }
}
