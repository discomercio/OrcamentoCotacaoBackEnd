using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class LojaResponseViewModel
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }
        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}
