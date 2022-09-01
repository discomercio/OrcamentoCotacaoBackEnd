using Newtonsoft.Json;

namespace Loja.Dto
{
    public class LojaViewModel
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("imagemLogotipo")]
        public string ImagemLogotipo { get; set; }

        [JsonProperty("corCabecalho")]
        public string CorCabecalho { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }
    }
}
