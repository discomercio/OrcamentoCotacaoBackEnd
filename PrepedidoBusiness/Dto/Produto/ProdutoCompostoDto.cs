using PrepedidoBusiness.Dto.Prepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoCompostoDto
    {
        public string PaiFabricante { get; set; }
        public string PaiProduto { get; set; }
        public decimal Preco_total_Itens { get; set; }
        public List<ProdutoFilhoDto> Filhos { get; set; }
    }
}
