using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class ProdutoGrupoResponse
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
