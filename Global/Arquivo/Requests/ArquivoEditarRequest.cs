using UtilsGlobais.RequestResponse;

namespace Arquivo.Requests
{
    public sealed class ArquivoEditarRequest : RequestBase
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Loja { get; set; }
    }
}