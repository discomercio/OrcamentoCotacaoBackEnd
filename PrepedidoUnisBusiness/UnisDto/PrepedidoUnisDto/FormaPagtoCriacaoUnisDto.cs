using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class FormaPagtoCriacaoUnisDto
    {
        /// <summary>
        /// Rb_forma_pagto:
        ///     COD_FORMA_PAGTO_A_VISTA = "1",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
        ///     COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA = "3",
        ///     COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA = "4",
        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA = "6";
        /// </summary>
        [Required]
        [MaxLength(1)]
        public short Tipo_Parcelamento { get; set; }//Tipo da forma de pagto

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_av_forma_pagto { get; set; }

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pu_forma_pagto { get; set; }
        public decimal? C_pu_valor { get; set; }
        public int? C_pu_vencto_apos { get; set; }
        public int? C_pc_qtde { get; set; }
        public decimal? C_pc_valor { get; set; }
        public int? C_pc_maquineta_qtde { get; set; }
        public decimal? C_pc_maquineta_valor { get; set; }

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pce_entrada_forma_pagto { get; set; }
        public decimal? C_pce_entrada_valor { get; set; }

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pce_prestacao_forma_pagto { get; set; }
        public int? C_pce_prestacao_qtde { get; set; }
        public decimal? C_pce_prestacao_valor { get; set; }
        public int? C_pce_prestacao_periodo { get; set; }

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pse_prim_prest_forma_pagto { get; set; }//Parcelado sem entrada
        public decimal? C_pse_prim_prest_valor { get; set; }
        public int? C_pse_prim_prest_apos { get; set; }

        /// <summary>
        /// Id retornado por /api/prepedidoUnis/buscarFormasPagto
        /// </summary>
        [MaxLength(1)]
        public string Op_pse_demais_prest_forma_pagto { get; set; }
        public int? C_pse_demais_prest_qtde { get; set; }
        public decimal? C_pse_demais_prest_valor { get; set; }
        public int? C_pse_demais_prest_periodo { get; set; }

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

        [Required]
        public short CustoFinancFornecQtdeParcelas { get; set; }
    }
}
