using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public abstract class ResponseBase
    {
        public bool Sucesso { get; set; } = true;
        public string Mensagem { get; set; } = string.Empty;
        public List<string> Mensagens { get; set; }
    }
}