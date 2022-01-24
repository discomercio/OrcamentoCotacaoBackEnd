﻿using Newtonsoft.Json;

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

        [JsonProperty("telefone1")]
        public string Telefone { get; set; }

        [JsonProperty("telefone2")]
        public string Celular { get; set; }
    }
}