using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoComboDto
    {
        public List<ProdutoDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoDto> ProdutoCompostoDto { get; set; }
    }
}
