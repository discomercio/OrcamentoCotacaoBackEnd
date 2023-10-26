using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.OrcamentistaIndicadorVendedor
{
    public class ListaOrcamentistaVendedorResponse:UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaOrcamentistaVendedor")]
        public List<OrcamentistaVendedorResponse> ListaOrcamentistaVendedor { get; set; }

        [JsonProperty("qtdeRegistros")]
        public int QtdeRegistros { get; set; }
    }
}
