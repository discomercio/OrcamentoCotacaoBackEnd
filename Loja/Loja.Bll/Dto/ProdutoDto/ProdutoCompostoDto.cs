using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ProdutoCompostoDto
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public decimal Preco_total_Itens { get; set; }
        public List<ProdutoFilhoDto> Filhos { get; set; }
    }
}
