using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoComboUnisDto : PrepedidoBusiness.Dto.Produto.ProdutoComboDto
    {
        public List<ProdutoUnisDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoUnisDto> ProdutoCompostoDto { get; set; }
    }
}
