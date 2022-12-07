using System;

namespace UtilsGlobais.RequestResponse
{
    public abstract class RequestBase
    {
        public Guid CorrelationId { get; set; }
        public string Usuario { get; set; }
        public string IP { get; set; }
    }
}