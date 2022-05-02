using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoCatalogoItemProdutosAtivosResponseViewModel : IViewModelResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("produto")]
        public string Produto { get; set; }
        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }
        [JsonProperty("fabricanteNome")]
        public string FabricanteNome { get; set; }
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("descricaoCompleta")]
        public string DescricaoCompleta { get; set; }
        [JsonProperty("idPropriedade")]
        public int IdPropriedade { get; set; }
        [JsonProperty("nomePropriedade")]
        public string NomePropriedade { get; set; }
        [JsonProperty("valorPropriedade")]
        public string ValorPropriedade { get; set; }
        [JsonProperty("ordem")]
        public string Ordem { get; set; }
    }
}
