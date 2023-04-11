using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicador
{
    public class BuscarParceiroRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("lojas")]
        public List<string> Lojas { get; set; }

        //[JsonProperty("vendedores")]
        //public string[] Vendedores { get; set; }

        //[JsonProperty("loja")]
        //public string Loja { get; set; }
    }
}