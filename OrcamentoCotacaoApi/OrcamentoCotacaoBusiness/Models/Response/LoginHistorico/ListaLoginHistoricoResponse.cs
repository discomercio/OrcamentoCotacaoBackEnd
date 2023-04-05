using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.LoginHistorico
{
    public class ListaLoginHistoricoResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("lstLoginHistoricoResponse")]
        public List<LoginHistoricoResponse> LstLoginHistoricoResponse { get; set; }
    }
}
