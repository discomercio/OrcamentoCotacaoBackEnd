using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ConsultaProdutosDto
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public decimal? Preco { get; set; }
        public string Cor { get; set; }
        public string Vendavel { get; set; }
    }
    
}
