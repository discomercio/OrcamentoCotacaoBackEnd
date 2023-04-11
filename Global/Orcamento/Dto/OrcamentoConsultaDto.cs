using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace Orcamento.Dto
{
    public sealed class OrcamentoConsultaDto : ResponseBase
    {
        public List<OrcamentoCotacaoListaDto> OrcamentoCotacaoLista { get; set; }
        public int QtdeRegistros { get; set; }
    }
}