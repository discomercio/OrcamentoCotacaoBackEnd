using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class UsuarioRequestViewModel: IViewModelRequest
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("celular")]
        public string Celular { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("StLoginBloqueadoAutomatico")]
        public bool StLoginBloqueadoAutomatico { get; set; }
        
    }
}
