using Orcamento.Dto;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response.Orcamento
{
    public sealed class OrcamentoCotacaoListaResponse : UtilsGlobais.RequestResponse.ResponseBase
    {
        public OrcamentoCotacaoListaResponse()
        {
            orcamentoCotacaoListaDto = new List<OrcamentoCotacaoListaDto>();
        }

        public List<OrcamentoCotacaoListaDto> orcamentoCotacaoListaDto { get; set; }
        public int qtdeRegistros { get; set; }
    }
}