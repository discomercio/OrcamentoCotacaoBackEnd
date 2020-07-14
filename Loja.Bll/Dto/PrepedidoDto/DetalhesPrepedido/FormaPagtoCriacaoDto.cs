using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido
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

    }
}
