using MagentoBusiness.UtilsMagento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    /// <summary>
    /// FormaPagtoCriacao: a forma de pagamento dos produtos. Os serviços não são incluídos aqui.
    /// <hr />
    /// </summary>
    public class FormaPagtoCriacaoMagentoDto
    {
        /// <summary>
        /// <br />
        /// Tipo_Parcelamento:
        ///     COD_FORMA_PAGTO_A_VISTA = "1",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
        /// <hr />
        /// Se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5" ou COD_FORMA_PAGTO_A_VISTA = "1", 
        /// <br />
        /// C_pu_valor = somatoria de RowTotal da lista de produtos + Valor liquido do frete
        /// <br />
        /// <br />
        /// Se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2" 
        /// <br />
        /// C_pc_qtde * C_pc_valor = somatoria de RowTotal da lista de produtos + Valor liquido do frete
        /// <br />
        /// <hr />
        /// Pedido que vier do Markeplace deve ter Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"  
        /// <br />
        /// Pedido que vier do Magento deve ter Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1" ou Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2" 
        /// <br />
        /// <hr />
        /// Valores lidos do appsettings.json:
        /// <br />
        /// Se Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"
        /// <br />
        /// - Op_pu_forma_pagto, default = "2" "Depósito" 
        /// <br />
        /// - C_pu_vencto_apos, default = 30 dias
        /// <br />
        /// <br />
        /// Se Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1" 
        /// <br />
        /// - Op_av_forma_pagto, default = "6" "Boleto" 
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

        public static Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(
            FormaPagtoCriacaoMagentoDto formaPagtoCriacaoMagento, ConfiguracaoApiMagento configuracaoApiMagento,
            InfCriacaoPedidoMagentoDto infCriacaoPedidostring)
        {
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados ret = new Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados();
            //Verificar com Edu se essa condição está correta
            if (formaPagtoCriacaoMagento.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
                !string.IsNullOrEmpty(infCriacaoPedidostring.Pedido_bs_x_ac) &&
                infCriacaoPedidostring.Marketplace_codigo_origem == InfraBanco.Constantes.Constantes.COD_MARKETPLACE_ARCLUBE)
            {
                ret.CustoFinancFornecQtdeParcelas = 0;
                ret.Op_av_forma_pagto = configuracaoApiMagento.FormaPagto.Magento.Op_av_forma_pagto;//boleto
                ret.CustoFinancFornecTipoParcelamento =
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
            }
            if (formaPagtoCriacaoMagento.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                !string.IsNullOrEmpty(infCriacaoPedidostring.Pedido_bs_x_marketplace) &&
                infCriacaoPedidostring.Marketplace_codigo_origem != InfraBanco.Constantes.Constantes.COD_MARKETPLACE_ARCLUBE)
            {
                ret.CustoFinancFornecQtdeParcelas = 1;
                ret.C_pu_valor = formaPagtoCriacaoMagento.C_pu_valor;
                ret.Op_pu_forma_pagto = configuracaoApiMagento.FormaPagto.Markeplace.Op_pu_forma_pagto; //Depósito
                ret.C_pu_vencto_apos = configuracaoApiMagento.FormaPagto.Markeplace.C_pu_vencto_apos;//ler do appsettings = 30 dias
                ret.CustoFinancFornecTipoParcelamento =
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            }
            if (formaPagtoCriacaoMagento.Tipo_Parcelamento.ToString() ==
                InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO &&
                !string.IsNullOrEmpty(infCriacaoPedidostring.Pedido_bs_x_ac) &&
                infCriacaoPedidostring.Marketplace_codigo_origem == InfraBanco.Constantes.Constantes.COD_MARKETPLACE_ARCLUBE)
            {
                ret.CustoFinancFornecQtdeParcelas = formaPagtoCriacaoMagento.C_pc_qtde ?? 1;
                ret.C_pc_qtde = formaPagtoCriacaoMagento.C_pc_qtde;
                ret.C_pc_valor = formaPagtoCriacaoMagento.C_pc_valor;
                ret.CustoFinancFornecTipoParcelamento =
                    InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            }

            ret.Rb_forma_pagto = formaPagtoCriacaoMagento.Tipo_Parcelamento.ToString(); //Tipo da forma de pagto

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
