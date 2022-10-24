using UtilsGlobais.RequestResponse;

namespace Arquivo.Requests
{
    public sealed class ArquivoDownloadRequest : RequestBase
    {
        public string Id { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}