using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UtilsGlobais.RequestResponse
{
    public abstract class ResponseBase
    {
        public Guid CorrelationId { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<string> Mensagens { get; set; }
        public string ObterNomeMetodoAtualAsync([CallerMemberName] string name = null)
        {
            return $"Método => [{name}]";
        }
    }
}