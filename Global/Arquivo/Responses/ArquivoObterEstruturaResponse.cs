using Arquivo.Dto;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace Arquivo.Responses
{
    public sealed class ArquivoObterEstruturaResponse : ResponseBase
    {
        public ArquivoObterEstruturaResponse()
        {
            Childs = new List<Child>();
        }

        public List<Child> Childs { get; set; }
        //public Child Childs { get; set; }
    }
}