using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ProdutoValidadoComEstoqueDto
    {
        public ProdutoDto Produto { get; set; }
        public List<string> ListaErros { get; set; }
    }
}
