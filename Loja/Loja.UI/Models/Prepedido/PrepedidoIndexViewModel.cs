using Loja.Bll.Dto.PrepedidoDto;
using Loja.UI.Models.Comuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable enable

namespace Loja.UI.Models.Prepedido
{
    public class PrepedidoIndexViewModel
    {
        public PrepedidoIndexViewModel(ResumoPrepedidoListaDto resumoPrepedidoListaDto, ListaLojasViewModel listaLojasViewModel)
        {
            ResumoPrepedidoListaDto = resumoPrepedidoListaDto ?? throw new ArgumentNullException(nameof(resumoPrepedidoListaDto));
            ListaLojasViewModel = listaLojasViewModel ?? throw new ArgumentNullException(nameof(listaLojasViewModel));
        }

        public ResumoPrepedidoListaDto ResumoPrepedidoListaDto{ get; }
        public ListaLojasViewModel ListaLojasViewModel { get; }
    }
}
