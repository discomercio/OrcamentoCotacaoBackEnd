using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.CodigoDescricao
{
    public class ListaCodigoDescricaoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaCodigoDescricao")]
        public List<CodigoDescricaoResponse> ListaCodigoDescricao { get; set; }
    }
}
