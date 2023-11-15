using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Models.Request.Usuario
{
    public class UsuariosRequest : UtilsGlobais.RequestResponse.RequestBase
    {
        [JsonProperty("parceiro")]
        public string Parceiro { get; set; }

        [JsonProperty("vendedor")]
        public string Vendedor { get; set; }

        [JsonProperty("tipoUsuario")]
        public int TipoUsuario { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("ativo")]
        public bool? Ativo { get; set; }

        [JsonProperty("pesquisa")]
        public string Pesquisa { get; set; }

        [JsonProperty("pagina")]
        public int Pagina { get; set; }

        [JsonProperty("qtdeItensPagina")]
        public int QtdeItensPagina { get; set; }

        [JsonProperty("ordenacaoAscendente")]
        public bool OrdenacaoAscendente { get; set; }

        [JsonProperty("nomeColuna")]
        public string NomeColuna { get; set; }

        [JsonProperty("parceiros")]
        public string[] Parceiros { get; set; }

        [JsonProperty("vendedores")]
        public int[] Vendedores { get; set; }

        [JsonProperty("stLoginBloqueadoAutomatico")]
        public bool? stLoginBloqueadoAutomatico { get; set; }
    }
}
