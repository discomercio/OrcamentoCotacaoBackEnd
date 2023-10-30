using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.Propriedades
{
    public class PropriedadesRequest: UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("ativo")]
        public bool? Ativo { get; set; }

        [JsonProperty("pagina")]
        public int Pagina { get; set; }

        [JsonProperty("qtdeItensPagina")]
        public int QtdeItensPorPagina { get; set; }

        [JsonProperty("ordenacaoAscendente")]
        public bool OrdenacaoAscendente { get; set; }

        [JsonProperty("nomeColuna")]
        public string NomeColunaOrdenacao { get; set; }
    }
}
