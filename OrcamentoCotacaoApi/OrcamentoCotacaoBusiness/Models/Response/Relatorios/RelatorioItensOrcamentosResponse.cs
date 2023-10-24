using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Relatorios
{
    public class RelatorioItensOrcamentosResponse:UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaItensOrcamento")]
        public List<ItensOrcamentoResponse> ListaItensOrcamento { get; set; }
    }
}
