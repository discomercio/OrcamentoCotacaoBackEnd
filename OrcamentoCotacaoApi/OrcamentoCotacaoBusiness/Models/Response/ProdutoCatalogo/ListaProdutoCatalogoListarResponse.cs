using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public class ListaProdutoCatalogoListarResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        [JsonProperty("listaProdutoCatalogoResponse")]
        public List<ProdutoCatalogoListarResponse>  ListaProdutoCatalogoResponse{ get; set; }

        [JsonProperty("qtdeRegistros")]
        public int QtdeRegistros { get; set; }
    }
}
