using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class ConsultaProdutoCatalogoAtivoResponse:UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("fabricanteNome")]
        public string FabricanteNome { get; set; }

        [JsonProperty("produto")]
        public string Produto { get; set; }

        [JsonProperty("produtoNome")]
        public string ProdutoNome { get; set; }

        [JsonProperty("produtoDescricaoCompleta")]
        public string ProdutoDescricaoCompleta { get; set; }

        [JsonProperty("imagemCaminho")]
        public string ImagemCaminho { get; set; }

        [JsonProperty("listaPropriedades")]
        public List<ConsultaProdutoCatalogoPropriedadeResponse> ListPropriedades { get; set; }
    }
}
