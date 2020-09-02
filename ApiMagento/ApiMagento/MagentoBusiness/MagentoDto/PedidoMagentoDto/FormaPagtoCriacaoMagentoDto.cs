using MagentoBusiness.UtilsMagento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class FormaPagtoCriacaoMagentoDto
    {
        /// <summary>
        /// Pedido que vier do Markeplace deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"  
        /// e Op_pu_forma_pagto = "2" "Depósito" e C_pu_vencto_apos = 30 dias (Definido no appsettings)
        /// <br />
        /// Pedido que vier do Magento deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1" 
        /// e Op_av_forma_pagto = "6" "Boleto" ou Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2" 
        /// (Definido no appsettings)
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

        public decimal? C_pu_valor { get; set; }

        public int? C_pc_qtde { get; set; }
        public decimal? C_pc_valor { get; set; }

        //C_forma_pagto = "" (deixar em branco)

        //CustoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA ou COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA,
        //preenchemos conforme Tipo_Parcelamento

        //CustoFinancFornecQtdeParcelas = 1 se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA
        //CustoFinancFornecQtdeParcelas = C_pc_qtde se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO

        public static Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(FormaPagtoCriacaoMagentoDto formaPagtoCriacaoMagento,
            ConfiguracaoApiMagento configuracaoApiMagento, string marketplace_codigo_origem)
        {
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados  ret = new Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados();

            if (formaPagtoCriacaoMagento?.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA && 
                string.IsNullOrEmpty(marketplace_codigo_origem))
            {
                ret.Qtde_Parcelas = 1;
                ret.Op_av_forma_pagto = configuracaoApiMagento.FormaPagto.Op_av_forma_pagto;//boleto
                ret.CustoFinancFornecTipoParcelamento = 
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
            }
            if (formaPagtoCriacaoMagento?.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                !string.IsNullOrEmpty(marketplace_codigo_origem))
            {
                ret.Qtde_Parcelas = 1;
                ret.C_pu_valor = formaPagtoCriacaoMagento.C_pu_valor;
                ret.Op_pu_forma_pagto = configuracaoApiMagento.FormaPagto.Op_pu_forma_pagto; //Depósito
                ret.C_pu_vencto_apos = configuracaoApiMagento.FormaPagto.C_pu_vencto_apos;//ler do appsettings = 30 dias
                ret.CustoFinancFornecTipoParcelamento = 
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            }
            if (formaPagtoCriacaoMagento?.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO && 
                string.IsNullOrEmpty(marketplace_codigo_origem))
            {
                ret.Qtde_Parcelas = (int)formaPagtoCriacaoMagento.C_pc_qtde;
                ret.C_pc_qtde = formaPagtoCriacaoMagento.C_pc_qtde;
                ret.C_pc_valor = formaPagtoCriacaoMagento.C_pc_valor;
                ret.CustoFinancFornecTipoParcelamento = 
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            }

            ret.Rb_forma_pagto = formaPagtoCriacaoMagento?.Tipo_Parcelamento.ToString(); //Tipo da forma de pagto
            
            ret.C_pc_maquineta_qtde = 0;
            ret.C_pc_maquineta_valor = 0M;
            ret.Op_pce_entrada_forma_pagto = "0";
            ret.C_pce_entrada_valor = 0M;
            ret.Op_pce_prestacao_forma_pagto = "0";
            ret.C_pce_prestacao_qtde = 0;
            ret.C_pce_prestacao_valor = 0M;
            ret.C_pce_prestacao_periodo = 0;
            ret.Op_pse_prim_prest_forma_pagto = "0";
            ret.C_pse_prim_prest_valor = 0M;
            ret.C_pse_prim_prest_apos = 0;
            ret.Op_pse_demais_prest_forma_pagto = "0";
            ret.C_pse_demais_prest_qtde = 0;
            ret.C_pse_demais_prest_valor = 0M;
            ret.C_pse_demais_prest_periodo = 0;
            ret.C_forma_pagto = "";//(deixar em branco)
            ret.Descricao_meio_pagto = "";
            ret.Tipo_parcelamento = formaPagtoCriacaoMagento.Tipo_Parcelamento;//informa o tipo de parcelamento que foi escolhido

            return ret;
        }
    }
}
