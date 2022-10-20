using System.Collections.Generic;

namespace UtilsGlobais.RequestResponse
{
    public abstract class ResponseBase
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<string> Mensagens { get; set; }
    }
}