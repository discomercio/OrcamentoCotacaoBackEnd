using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class ConsultaProdutoCatalogoPropriedadeResponse
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valor")]
        public string Valor { get; set; }
    }
}
