using Microsoft.AspNetCore.Http;

namespace Arquivo.Requests
{
    public sealed class ArquivoUploadRequest
    {
        public IFormFile Arquivo { get; set; }
        public string Caminho { get; set; }
        public string IdPai { get; set; }
    }
}