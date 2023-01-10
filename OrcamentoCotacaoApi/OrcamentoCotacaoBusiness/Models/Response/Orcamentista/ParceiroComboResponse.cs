using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamentista
{
    public class ParceiroComboResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("razaoSocial")]
        public string RazaoSocial { get; set; }
    }
}
