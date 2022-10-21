using Microsoft.AspNetCore.Http;

namespace Arquivo.Requests
{
    public sealed class ArquivoUploadRequest
    {
        public string IdPai { get; set; }
        public string Caminho { get; set; }
        public IFormFile Arquivo { get; set; }
    }
}