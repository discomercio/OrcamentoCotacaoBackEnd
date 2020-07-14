using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class ProdutoDevolvidoDtoPedido
    {
        public DateTime? Data { get; set; }
        public string Hora { get; set; }
        public short? Qtde { get; set; }
        public string CodProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Motivo { get; set; }
        public int NumeroNF { get; set; }
    }
}
