using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoCompostoResponseViewModel
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public string PaiDescricao { get; set; }
        public decimal? PaiPrecoTotal { get; set; }
        public List<ProdutoSimplesResponseViewModel> Filhos { get; set; }
    }
}
