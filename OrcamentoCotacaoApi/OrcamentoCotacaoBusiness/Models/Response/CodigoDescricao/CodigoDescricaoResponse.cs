using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.CodigoDescricao
{
    public class CodigoDescricaoResponse
    {
        [JsonProperty("grupo")]
        public string Grupo { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("ordenacao")]
        public short Ordenacao { get; set; }

        [JsonProperty("stInativo")]
        public byte St_Inativo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
