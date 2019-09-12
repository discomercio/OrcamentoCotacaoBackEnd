using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoDto
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public decimal? Qtde { get; set; }
        public decimal? ValorLista { get; set; }
        public float? Desconto { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? ValorTotal { get; set; }
    }
}
