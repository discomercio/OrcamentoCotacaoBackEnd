using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Usuario
{
    public class UsuarioPorListaLojaResponse
    {
        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("nomeIniciaisMaiusculo")]
        public string NomeIniciaisMaiusculo { get; set; }
    }
}
