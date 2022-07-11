using Newtonsoft.Json;
using OrcamentoCotacaoBusiness.Models.Request;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoOrcamentoOpcaoResponseViewModel : ProdutoRequestViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idItemUnificado")]
        public int IdItemUnificado { get; set; }

        [JsonProperty("idOpcaoPagto")]
        public int IdOpcaoPagto { get; set; }

        [JsonProperty("idOperacaoAlcadaDescontoSuperior")]
        public int? IdOperacaoAlcadaDescontoSuperior { get; set; }
    }
}
