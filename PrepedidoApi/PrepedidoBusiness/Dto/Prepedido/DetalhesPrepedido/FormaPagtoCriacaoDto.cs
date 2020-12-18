using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido
{
    public class FormaPagtoCriacaoDto
    {
        public int Qtde_Parcelas { get; set; }
        public string Rb_forma_pagto { get; set; }//Tipo da forma de pagto
        public string Op_av_forma_pagto { get; set; }
        public string Op_pu_forma_pagto { get; set; }
        public decimal? C_pu_valor { get; set; }
        public int? C_pu_vencto_apos { get; set; }
        public int? C_pc_qtde { get; set; }
        public decimal? C_pc_valor { get; set; }
        public int? C_pc_maquineta_qtde { get; set; }
        public decimal? C_pc_maquineta_valor { get; set; }
        public string Op_pce_entrada_forma_pagto { get; set; }//Parcelado com entrada
        public decimal? C_pce_entrada_valor { get; set; }
        public string Op_pce_prestacao_forma_pagto { get; set; }
        public int? C_pce_prestacao_qtde { get; set; }
        public decimal? C_pce_prestacao_valor { get; set; }
        public int? C_pce_prestacao_periodo { get; set; }
        public string Op_pse_prim_prest_forma_pagto { get; set; }//Parcelado sem entrada
        public decimal? C_pse_prim_prest_valor { get; set; }
        public int? C_pse_prim_prest_apos { get; set; }
        public string Op_pse_demais_prest_forma_pagto { get; set; }
        public int? C_pse_demais_prest_qtde { get; set; }
        public decimal? C_pse_demais_prest_valor { get; set; }
        public int? C_pse_demais_prest_periodo { get; set; }
        public string C_forma_pagto { get; set; }//Descrição da forma de pagto
        public string Descricao_meio_pagto { get; set; }//para mostrar 
        public short Tipo_parcelamento { get; set; }//informa o tipo de parcelamento que foi escolhido

        //incluimos esse campo apenas para validar o que esta sendo enviado pela API Unis
        public string CustoFinancFornecTipoParcelamento { get; set; }

        public static FormaPagtoCriacaoDto FormaPagtoCriacaoDto_De_FormaPagtoCriacaoDados(FormaPagtoCriacaoDados origem)
        {
            if (origem == null) return null;
            return new FormaPagtoCriacaoDto()
            {
                Qtde_Parcelas = origem.Qtde_Parcelas_Para_Exibicao,
                Rb_forma_pagto = origem.Rb_forma_pagto,
                Op_av_forma_pagto = origem.Op_av_forma_pagto,
                Op_pu_forma_pagto = origem.Op_pu_forma_pagto,
                C_pu_valor = origem.C_pu_valor,
                C_pu_vencto_apos = origem.C_pu_vencto_apos,
                C_pc_qtde = origem.C_pc_qtde,
                C_pc_valor = origem.C_pc_valor,
                C_pc_maquineta_qtde = origem.C_pc_maquineta_qtde,
                C_pc_maquineta_valor = origem.C_pc_maquineta_valor,
                Op_pce_entrada_forma_pagto = origem.Op_pce_entrada_forma_pagto,
                C_pce_entrada_valor = origem.C_pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = origem.Op_pce_prestacao_forma_pagto,
                C_pce_prestacao_qtde = origem.C_pce_prestacao_qtde,
                C_pce_prestacao_valor = origem.C_pce_prestacao_valor,
                C_pce_prestacao_periodo = origem.C_pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = origem.Op_pse_prim_prest_forma_pagto,
                C_pse_prim_prest_valor = origem.C_pse_prim_prest_valor,
                C_pse_prim_prest_apos = origem.C_pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = origem.Op_pse_demais_prest_forma_pagto,
                C_pse_demais_prest_qtde = origem.C_pse_demais_prest_qtde,
                C_pse_demais_prest_valor = origem.C_pse_demais_prest_valor,
                C_pse_demais_prest_periodo = origem.C_pse_demais_prest_periodo,
                C_forma_pagto = origem.C_forma_pagto,
                Descricao_meio_pagto = origem.Descricao_meio_pagto,
                Tipo_parcelamento = origem.Tipo_parcelamento,
                CustoFinancFornecTipoParcelamento = origem.CustoFinancFornecTipoParcelamento,
            };
        }
        public static FormaPagtoCriacaoDados FormaPagtoCriacaoDados_De_FormaPagtoCriacaoDto(FormaPagtoCriacaoDto origem)
        {
            if (origem == null) return null;
            var ret = new FormaPagtoCriacaoDados()
            {
                Rb_forma_pagto = origem.Rb_forma_pagto,
                Op_av_forma_pagto = origem.Op_av_forma_pagto,
                Op_pu_forma_pagto = origem.Op_pu_forma_pagto,
                C_pu_valor = origem.C_pu_valor,
                C_pu_vencto_apos = origem.C_pu_vencto_apos,
                C_pc_qtde = origem.C_pc_qtde,
                C_pc_valor = origem.C_pc_valor,
                C_pc_maquineta_qtde = origem.C_pc_maquineta_qtde,
                C_pc_maquineta_valor = origem.C_pc_maquineta_valor,
                Op_pce_entrada_forma_pagto = origem.Op_pce_entrada_forma_pagto,
                C_pce_entrada_valor = origem.C_pce_entrada_valor,
                Op_pce_prestacao_forma_pagto = origem.Op_pce_prestacao_forma_pagto,
                C_pce_prestacao_qtde = origem.C_pce_prestacao_qtde,
                C_pce_prestacao_valor = origem.C_pce_prestacao_valor,
                C_pce_prestacao_periodo = origem.C_pce_prestacao_periodo,
                Op_pse_prim_prest_forma_pagto = origem.Op_pse_prim_prest_forma_pagto,
                C_pse_prim_prest_valor = origem.C_pse_prim_prest_valor,
                C_pse_prim_prest_apos = origem.C_pse_prim_prest_apos,
                Op_pse_demais_prest_forma_pagto = origem.Op_pse_demais_prest_forma_pagto,
                C_pse_demais_prest_qtde = origem.C_pse_demais_prest_qtde,
                C_pse_demais_prest_valor = origem.C_pse_demais_prest_valor,
                C_pse_demais_prest_periodo = origem.C_pse_demais_prest_periodo,
                C_forma_pagto = origem.C_forma_pagto,
                Descricao_meio_pagto = origem.Descricao_meio_pagto,
                Tipo_parcelamento = origem.Tipo_parcelamento,
                CustoFinancFornecTipoParcelamento = origem.CustoFinancFornecTipoParcelamento,
            };

            ret.CustoFinancFornecQtdeParcelas = global::Prepedido.PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(ret);
            return ret;
        }
    }
}
