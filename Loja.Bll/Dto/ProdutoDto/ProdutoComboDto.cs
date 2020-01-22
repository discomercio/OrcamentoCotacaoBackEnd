using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ProdutoComboDto
    {
        public List<ProdutoDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoDto> ProdutoCompostoDto { get; set; }
    }
}
