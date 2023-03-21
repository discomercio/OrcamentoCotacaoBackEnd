using Newtonsoft.Json;
using System;
namespace OrcamentoCotacaoBusiness.Models.Response.LoginHistorico
{
    public class LoginHistoricoResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("dataHora")]
        public DateTime DataHora { get; set; }

        [JsonProperty("idTipoUsuarioContexto")]
        public int? IdTipoUsuarioContexto { get; set; }

        [JsonProperty("idUsuario")]
        public int? IdUsuario { get; set; }

        [JsonProperty("stSucesso")]
        public bool StSucesso { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("sistemaResponsavel")]
        public int sistema_responsavel { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("motivo")]
        public string Motivo { get; set; }

        [JsonProperty("idCfgModulo")]
        public short IdCfgModulo { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }
    }
}
