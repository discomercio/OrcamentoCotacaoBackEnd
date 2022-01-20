using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoResponseViewModel: IViewModelResponse
    {
        public List<ProdutoCompostoResponseViewModel> ProdutosCompostos { get; set; }
        public List<ProdutoSimplesResponseViewModel> ProdutosSimples { get; set; }
    }
}
