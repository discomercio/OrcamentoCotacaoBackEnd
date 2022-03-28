using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentistaEIndicadorVendedorResponseViewModel : IViewModelResponse
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("indicador")]
        public string IdIndicador { get; set; }
        [JsonProperty("ativo")]
        public bool Ativo { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("telefone")]
        public string Telefone { get; set; }
        [JsonProperty("celular")]
        public string Celular { get; set; }
        [JsonProperty("vendedorResponsavel")]
        public string VendedorResponsavel { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

    }
}
