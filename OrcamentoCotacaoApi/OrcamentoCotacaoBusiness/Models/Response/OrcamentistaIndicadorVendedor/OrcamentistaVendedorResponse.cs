using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.OrcamentistaIndicadorVendedor
{
    public class OrcamentistaVendedorResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("indicador")]
        public int Indicador { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("ativoLabel")]
        public string AtivoLabel { get; set; }

        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("vendedorResponsavel")]
        public string VendedorResponsavel { get; set; }

        [JsonProperty("bloqueado")]
        public string Bloqueado { get; set; }

        [JsonProperty("dtCriacao")]
        public DateTime? DtCriacao { get; set; }

        [JsonProperty("ultimoLogin")]
        public DateTime? UltimoLogin { get; set; }
    }
}
