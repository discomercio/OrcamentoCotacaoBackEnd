using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentistaIndicadorResponseViewModel : IViewModelResponse
    {
        [JsonProperty("idIndicador")]
        public int IdIndicador { get; set; }
        [JsonProperty("nome")]
        public string Apelido { get; set; }
        [JsonProperty("razaoSocial")]
        public string Razao_Social_Nome { get; set; }
    }
}
