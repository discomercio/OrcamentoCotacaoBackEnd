using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoComboUnisDto
    {
        public List<ProdutoUnisDto> ProdutoUnisDto { get; set; }
        public List<ProdutoCompostoUnisDto> ProdutoCompostoUnisDto { get; set; }
    }
}
