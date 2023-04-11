using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.OrcamentistaIndicadorVendedor
{
    public class OrcamentistaIndicadorVendedorDeleteRequest: UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("idIndicadorVendedor")]
        public int IdIndicadorVendedor { get; set; }
    }
}
