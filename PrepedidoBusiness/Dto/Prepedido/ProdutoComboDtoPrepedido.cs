using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArclubePrepedidosWebapi.Dtos.Prepedido
{
    public class ProdutoComboDtoPrepedido
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