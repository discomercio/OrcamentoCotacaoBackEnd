using UtilsGlobais.RequestResponse;

namespace Arquivo.Requests
{
    public sealed class ArquivoCriarPastaRequest : RequestBase
    {
        public string IdPai { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}