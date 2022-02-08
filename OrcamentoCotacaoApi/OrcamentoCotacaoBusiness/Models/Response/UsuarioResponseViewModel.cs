using Newtonsoft.Json;
using System;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class UsuarioResponseViewModel : IViewModelResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("nome")]
        public string Usuario { get; set; }

        [JsonProperty("parceiro")]
        public string IdParceiro { get; set; }

        [JsonProperty("IdVendedor")]
        public string IdVendedor { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telefone1")]
        public string Telefone { get; set; }

        [JsonProperty("telefone2")]
        public string Celular { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }

        [JsonProperty("tipoUsuario")]
        public TipoUsuario TipoUsuario { get; set; }
        [JsonProperty("vendedorResponsavel")]
        public string VendedorResponsavel { get; set; }
    }
}
