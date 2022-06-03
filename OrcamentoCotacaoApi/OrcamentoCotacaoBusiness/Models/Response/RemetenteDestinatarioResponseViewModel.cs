using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class RemetenteDestinatarioResponseViewModel
    {
        [JsonProperty("idOrcamentoCotacao")]
        public int IdOrcamentoCotacao { get; set; }

        [JsonProperty("idUsuarioRemetente")]
        public int IdUsuarioRemetente { get; set; }

        [JsonProperty("idTipoUsuarioContextoRemetente")]
        public int IdTipoUsuarioContextoRemetente { get; set; }

        [JsonProperty("idUsuarioDestinatario")]
        public int IdUsuarioDestinatario { get; set; }

        [JsonProperty("idTipoUsuarioContextoDestinatario")]
        public int IdTipoUsuarioContextoDestinatario { get; set; }

        [JsonProperty("donoOrcamento")]
        public virtual bool? DonoOrcamento { get; set; }

        [JsonProperty("idDonoOrcamento")]
        public virtual int? IdDonoOrcamento { get; set; }
    }
}
