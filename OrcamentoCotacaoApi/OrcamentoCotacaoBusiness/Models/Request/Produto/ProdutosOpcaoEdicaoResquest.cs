using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.Produto
{
    public class ProdutosOpcaoEdicaoResquest: ProdutosRequestViewModel
    {
        [JsonProperty("produtos")]
        public List<string> Produtos { get; set; }

        [JsonProperty("idOpcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("idOpcaoFormaPagto")]
        public int IdOpcaoFormaPagto { get; set; }
    }
}
