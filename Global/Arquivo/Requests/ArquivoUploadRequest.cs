using Microsoft.AspNetCore.Http;
using UtilsGlobais.RequestResponse;

namespace Arquivo.Requests
{
    public sealed class ArquivoUploadRequest : RequestBase
    {
        public string IdPai { get; set; }
        public string CaminhoArquivo { get; set; }
        public IFormFile Arquivo { get; set; }
        public string Loja { get; set; }
    }
}