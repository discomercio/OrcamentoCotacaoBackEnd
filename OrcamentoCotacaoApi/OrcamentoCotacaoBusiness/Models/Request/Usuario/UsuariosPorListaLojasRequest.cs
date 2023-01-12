using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request.Usuario
{
    public class UsuariosPorListaLojasRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("lojas")]
        public List<string> Lojas { get; set; }
    }
}
