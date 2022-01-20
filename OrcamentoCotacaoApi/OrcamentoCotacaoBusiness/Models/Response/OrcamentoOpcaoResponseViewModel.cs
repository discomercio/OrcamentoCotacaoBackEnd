using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentoOpcaoResponseViewModel : IViewModelResponse
    {
        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("Numero")]
        public string Numero { get; set; }

        [JsonProperty("Nome")]
        public string Nome { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Valor")]
        public double Valor { get; set; }
    }
}
