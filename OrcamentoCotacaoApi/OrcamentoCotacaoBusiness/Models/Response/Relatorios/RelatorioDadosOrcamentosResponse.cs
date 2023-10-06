using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Relatorios
{
    public class RelatorioDadosOrcamentosResponse: UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaDadosOrcamento")]
        public List<DadosOrcamentoResponse> ListaDadosOrcamento { get; set; }
    }
}
