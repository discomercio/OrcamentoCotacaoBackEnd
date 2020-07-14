using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class ProdutoCriacao
    {
        public string NumProduto { get; set; }
        public int Qtde { get; set; }
        public decimal Preco { get; set; }
        public decimal VlLista { get; set; }
        public decimal Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal VlTotalItem { get; set; }

        
    }
}
