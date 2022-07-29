using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public  class ClienteOrcamentoCotacaoRequestViewModel
    {
        [JsonProperty("nomeCliente")]
        public string NomeCliente { get; set; }

        [JsonProperty("nomeObra")]
        public string NomeObra { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("contribuinteICMS")]
        public byte? ContribuinteICMS { get; set; }

    }
}
