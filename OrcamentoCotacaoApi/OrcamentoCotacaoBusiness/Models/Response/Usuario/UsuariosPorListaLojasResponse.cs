using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.Usuario
{
    public class UsuariosPorListaLojasResponse: UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("usuarios")]
        public List<UsuarioPorListaLojaResponse> Usuarios { get; set; }
    }
}
