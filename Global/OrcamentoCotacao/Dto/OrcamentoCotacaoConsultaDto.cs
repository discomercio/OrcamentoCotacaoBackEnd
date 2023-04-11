using Orcamento.Dto;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacao.Dto
{
    public class OrcamentoCotacaoConsultaDto : ResponseBase
    {
        public List<OrcamentoCotacaoListaDto> OrcamentoCotacaoLista { get; set; }
        public int QtdeRegistros { get; set; }
    }
}