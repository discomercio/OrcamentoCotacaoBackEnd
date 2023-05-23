using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Mensagem
{
    public class ListaQuantidadeMensagemPendenteResponse:UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaQtdeMensagemPendente")]
        public List<QuantidadeMensagemPendenteResponse> ListaQtdeMensagemPendente { get; set; }
    }
}
