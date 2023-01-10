using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicador
{
    public class BuscarParceiroRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("lojas")]
        public List<string> Lojas { get; set; }
    }
}
