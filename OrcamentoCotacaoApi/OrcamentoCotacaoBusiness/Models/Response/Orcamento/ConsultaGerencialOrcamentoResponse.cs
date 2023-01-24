using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public class ConsultaGerencialOrcamentoResponse
    {
        [JsonProperty("orcamento")]
        public int Orcamento { get; set; }

        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("uf")]
        public string UF { get; set; }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("dataExpiracao")]
        public DateTime DataExpiracao { get; set; }
    }
}
