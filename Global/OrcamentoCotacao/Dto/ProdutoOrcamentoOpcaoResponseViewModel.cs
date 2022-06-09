using OrcamentoCotacaoBusiness.Models.Request;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoOrcamentoOpcaoResponseViewModel : ProdutoRequestViewModel
    {
        public int Id { get; set; }
        public int IdItemUnificado { get; set; }
        public int IdOpcaoPagto { get; set; }
    }
}
