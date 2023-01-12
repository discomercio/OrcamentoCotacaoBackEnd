using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamentista
{
    public class ParceirosComboResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("parceiros")]
        public List<ParceiroComboResponse> Parceiros { get; set; }
    }
}
