using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Propriedade
{
    public class ListaPropriedadesResponse:UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaPropriedades")]
        public List<PropriedadesResponse> ListaPropriedades { get; set; }

        [JsonProperty("qtdeRegistros")]
        public int QtdeRegistros { get; set; }
    }
}
