using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ProdutosEstoqueDto
    {
        public string Produto { get; set; }
        public int Qtde { get; set; }
        public int Qtde_Utilizada { get; set; }
        public short Id_nfe_emitente { get; set; }
    }
}
