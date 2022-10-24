using UtilsGlobais.RequestResponse;

namespace UtilsGlobais.Exceptions
{
    public sealed class MensagemExceptionResponse : ResponseBase
    {
        public int StatusCode { get; set; }
    }
}