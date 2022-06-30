using Newtonsoft.Json;

namespace Produto.Dto
{
    public class ProdutoAtivoDto
    {
        public string id { get; set; }
        public string produto { get; set; }
        public string fabricante { get; set; }
        public string descricao { get; set; }

        public string calculadoraVRF { get; set; }
        public string tipoUnidade { get; set; }
        public string descargaCondensadora { get; set; }
        public string voltagem { get; set; }
        public string capacidadeBTU { get; set; }
        public string linhaProduto { get; set; }

        [JsonIgnore]
        public int propId { get; set; }
        [JsonIgnore]
        public string propValor { get; set; }
        public string ciclo { get; internal set; }
    }
}
