using Newtonsoft.Json;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class FormaPagtoCriacaoRequest : RequestBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idOrcamentoCotacaoOpcao")]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [JsonProperty("aprovado")]
        public bool Aprovado { get; set; }

        [JsonProperty("observacoesGerais")]
        public string Observacao { get; set; }

        [JsonProperty("tipo_parcelamento")]
        public int Tipo_parcelamento { get; set; }

        [JsonProperty("op_av_forma_pagto")]
        public int Av_forma_pagto { get; set; }

        [JsonProperty("c_pc_qtde")]
        public int Pc_qtde_parcelas { get; set; }

        [JsonProperty("c_pc_valor")]
        public decimal Pc_valor_parcela { get; set; }

        [JsonProperty("c_pc_maquineta_qtde")]
        public int Pc_maquineta_qtde_parcelas { get; set; }

        [JsonProperty("c_pc_maquineta_valor")]
        public decimal Pc_maquineta_valor_parcela { get; set; }

        [JsonProperty("op_pce_entrada_forma_pagto")]
        public int Pce_forma_pagto_entrada { get; set; }

        [JsonProperty("op_pce_prestacao_forma_pagto")]
        public int Pce_forma_pagto_prestacao { get; set; }

        [JsonProperty("o_pce_entrada_valor")]
        public decimal Pce_entrada_valor { get; set; }

        [JsonProperty("c_pce_prestacao_qtde")]
        public int Pce_prestacao_qtde { get; set; }

        [JsonProperty("c_pce_prestacao_valor")]
        public decimal Pce_prestacao_valor { get; set; }

        [JsonProperty("c_pce_prestacao_periodo")]
        public int Pce_prestacao_periodo { get; set; }

        [JsonProperty("op_pse_prim_prest_forma_pagto")]
        public int Pse_forma_pagto_prim_prest { get; set; }

        [JsonProperty("op_pse_demais_prest_forma_pagto")]
        public int Pse_forma_pagto_demais_prest { get; set; }

        [JsonProperty("c_pse_prim_prest_valor")]
        public decimal Pse_prim_prest_valor { get; set; }

        [JsonProperty("c_pse_prim_prest_apos")]
        public int Pse_prim_prest_apos { get; set; }

        [JsonProperty("c_pse_demais_prest_qtde")]
        public int Pse_demais_prest_qtde { get; set; }

        [JsonProperty("c_pse_demais_prest_valor")]
        public decimal Pse_demais_prest_valor { get; set; }

        [JsonProperty("c_pse_demais_prest_periodo")]
        public int Pse_demais_prest_periodo { get; set; }

        [JsonProperty("op_pu_forma_pagto")]
        public int Pu_forma_pagto { get; set; }

        [JsonProperty("c_pu_valor")]
        public decimal Pu_valor { get; set; }

        [JsonProperty("c_pu_vencto_apos")]
        public int Pu_vencto_apos { get; set; }

        [JsonProperty("qtde_Parcelas")]
        public int Qtde_Parcelas { get; set; }
    }
}
