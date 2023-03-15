using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.GrupoSubgrupoProduto
{
    public class GrupoSubgrupoProdutoRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }
    }
}
