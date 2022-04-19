using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class FormaPagtoCriacaoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("qtde_Parcelas")] 
        public int QtdeParcelas { get; set; }

        [JsonProperty("op_av_forma_pagto")]
        public int AvFormaPagto { get; set; }

        [JsonProperty("op_pu_forma_pagto")]
        public int PuFormaPagto { get; set; }

        [JsonProperty("c_pu_valor")]
        public decimal PuValor { get; set; }

        [JsonProperty("c_pu_vencto_apos")]
        public int PuVenctoApos { get; set; }

        [JsonProperty("c_pc_qtde")]
        public int PcQtdeParcelas { get; set; }

        [JsonProperty("c_pc_valor")]
        public decimal PcValorParcela { get; set; }

        [JsonProperty("c_pc_maquineta_qtde")]
        public int PcMaquinetaQtdeParcelas { get; set; }

        [JsonProperty("c_pc_maquineta_valor")]
        public decimal PcMaquinetaValorParcela { get; set; }

        [JsonProperty("op_pce_entrada_forma_pagto")]
        public int PceFormaPagtoEntrada { get; set; }

        [JsonProperty("o_pce_entrada_valor")]
        public decimal PceFormaPagtoEntradaValor { get; set; }

        [JsonProperty("op_pce_prestacao_forma_pagto")]
        public int PceFormaPagtoPrestacao { get; set; }

        [JsonProperty("c_pce_prestacao_qtde")]
        public int PcePrestacaoQtde { get; set; }

        [JsonProperty("c_pce_prestacao_valor")]
        public decimal PcePrestacaoValor { get; set; }

        [JsonProperty("c_pce_prestacao_periodo")]
        public int PcePrestacaoPeriodo { get; set; }

        [JsonProperty("op_pse_prim_prest_forma_pagto")]
        public int PseFormaPagtoPrimPrest { get; set; }

        [JsonProperty("c_pse_prim_prest_valor")]
        public decimal PsePrimPrestValor { get; set; }

        [JsonProperty("c_pse_prim_prest_apos")]
        public int PsePrimPrestApos { get; set; }

        [JsonProperty("op_pse_demais_prest_forma_pagto")]
        public int PseFormaPagtoDemaisPrest { get; set; }

        [JsonProperty("c_pse_demais_prest_qtde")]
        public int PseDemaisPrestQtde { get; set; }

        [JsonProperty("c_pse_demais_prest_valor")]
        public decimal PseDemaisPrestValor { get; set; }

        [JsonProperty("c_pse_demais_prest_periodo")]
        public int PseDemaisPrestPeriodo { get; set; }

        [JsonProperty("tipo_parcelamento")]
        public int TipoParcelamento { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }
    }
}
