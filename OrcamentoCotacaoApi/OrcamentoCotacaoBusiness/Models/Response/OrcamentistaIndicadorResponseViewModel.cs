using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentistaIndicadorResponseViewModel : IViewModelResponse
    {
        [JsonProperty("nome")]
        public string Apelido { get; set; }
    }
}
