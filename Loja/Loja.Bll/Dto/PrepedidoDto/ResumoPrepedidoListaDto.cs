using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace Loja.Bll.Dto.PrepedidoDto
{
    public class ResumoPrepedidoListaDto
    {
        public List<ResumoPrepedidoDto> Itens { get; } = new List<ResumoPrepedidoDto>();
    }
}

