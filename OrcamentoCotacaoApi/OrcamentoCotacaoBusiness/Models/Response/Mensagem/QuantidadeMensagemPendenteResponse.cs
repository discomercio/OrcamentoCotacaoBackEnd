using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.Mensagem
{
    public class QuantidadeMensagemPendenteResponse
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("qtde")]
        public int Qtde { get; set; }
    }
}
