using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class ListaConsultaGerencialOrcamentoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("lstConsultaGerencialOrcamentoResponse")]
        public List<ConsultaGerencialOrcamentoResponse> LstConsultaGerencialOrcamentoResponse { get; set; }

        [JsonProperty("qtdeRegistros")]
        public int QtdeRegistros { get; set; }
    }
}
