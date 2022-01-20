using Newtonsoft.Json;
using System;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Validade")]
        public DateTime Validade { get; set; }

        [JsonProperty("Observacao")]
        public string Observacao { get; set; }

        [JsonProperty("NomeCliente")]
        public string NomeCliente { get; set; }

        [JsonProperty("NomeObra")]
        public string NomeObra { get; set; }

        [JsonProperty("Vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("Telefone")]
        public string Telefone { get; set; }

        [JsonProperty("ConcordaWhatsapp")]
        public bool ConcordaWhatsapp { get; set; }

        [JsonProperty("VendedorParceiro")]
        public string VendedorParceiro { get; set; }

        [JsonProperty("Uf")]
        public string Uf { get; set; }

        [JsonProperty("Tipo")]
        public string Tipo { get; set; }
    }
}
