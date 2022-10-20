using UtilsGlobais.RequestResponse;

namespace Arquivo.Responses
{
    public sealed class ArquivoDownloadResponse : ResponseBase
    {
        public string Nome { get; set; }
        public string FileLength { get; set; }
        public byte[] ByteArray { get; set; }
    }
}