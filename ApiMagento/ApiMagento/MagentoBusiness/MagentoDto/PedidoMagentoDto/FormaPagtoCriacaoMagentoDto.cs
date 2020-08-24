using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class FormaPagtoCriacaoMagentoDto
    {
        /// <summary>
        /// Verificar com Hamilton como é a forma de pagamento
        /// <br />
        /// <br />
        /// Tipo_Parcelamento:
        ///     COD_FORMA_PAGTO_A_VISTA = "1",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
        /// <hr />
        /// </summary>
        [Required]
        public short Tipo_Parcelamento { get; set; }//Tipo da forma de pagto

        //Op_av_forma_pagto = sempre código de "Transferência" se Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA ???? afazer: confirmar com Karina 
        //Op_pu_forma_pagto = sempre código de "Depósito" se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA

        public decimal? C_pu_valor { get; set; }
        //C_pu_vencto_apos  = 30 dias (ler do appsettings)

        public int? C_pc_qtde { get; set; }
        public decimal? C_pc_valor { get; set; }

        //C_forma_pagto = "" (deixar em branco)

        //CustoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA ou COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA,
        //preenchemos conforme Tipo_Parcelamento

        //CustoFinancFornecQtdeParcelas = 1 se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA
        //CustoFinancFornecQtdeParcelas = C_pc_qtde se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO
    }
}
