using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoResponseViewModel: IViewModelResponse
    {
        [JsonProperty("produtosCompostos")]
        public List<ProdutoCompostoResponseViewModel> ProdutosCompostos { get; set; }

        [JsonProperty("produtosSimples")]
        public List<ProdutoSimplesResponseViewModel> ProdutosSimples { get; set; }
    }
}
