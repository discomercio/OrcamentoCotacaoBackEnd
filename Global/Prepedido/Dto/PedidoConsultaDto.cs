using Orcamento.Dto;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace Prepedido.Dto
{
    public sealed class PedidoConsultaDto : ResponseBase
    {
        public List<OrcamentoCotacaoListaDto> OrcamentoCotacaoLista { get; set; }
        public int QtdeRegistros { get; set; }
    }
}