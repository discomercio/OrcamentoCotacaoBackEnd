using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.ProdutoCatalogo
{
    public class ConsultaProdutoCatalogoAtivoRequest: UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
