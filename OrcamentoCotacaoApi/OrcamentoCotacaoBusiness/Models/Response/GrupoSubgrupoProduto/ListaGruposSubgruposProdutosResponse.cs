using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.GrupoSubgrupoProduto
{
    public class ListaGruposSubgruposProdutosResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaGruposSubgruposProdutos")]
        public List<GrupoSubgrupoProdutoResponse> ListaGruposSubgruposProdutos { get; set; }
    }
}
