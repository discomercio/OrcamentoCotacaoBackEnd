using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class FormaPagtoDto
    {
        [JsonProperty("id")]

        public int Id { get; set; }

        [JsonProperty("idOrcamentoCotacaoOpcao")]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [JsonProperty("aprovado")]
        public bool Aprovado { get; set; }

        [JsonProperty("observacoesGerais")]
        public string Observacao { get; set; }

        [JsonProperty("formaPagamento")]
        public string FormaPagamento { get; set; }
    }
}
