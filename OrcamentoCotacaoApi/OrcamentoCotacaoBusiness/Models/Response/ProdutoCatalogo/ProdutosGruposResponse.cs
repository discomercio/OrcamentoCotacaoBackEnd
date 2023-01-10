using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class ProdutosGruposResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("produtosGrupos")]
        public List<ProdutoGrupoResponse> ProdutosGrupos { get; set; }
    }
}
