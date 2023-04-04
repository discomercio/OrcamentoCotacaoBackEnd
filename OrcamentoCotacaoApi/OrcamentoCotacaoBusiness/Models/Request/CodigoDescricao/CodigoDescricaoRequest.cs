using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.CodigoDescricao
{
    public class CodigoDescricaoRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("grupo")]
        public string Grupo { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }
    }
}
