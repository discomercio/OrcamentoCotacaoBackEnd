using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArclubePrepedidosWebapi.Dtos.Prepedido
{
    public class PrePedidoClienteDto
    {
        public DadosClienteDtoPrepedido DadosClientePrePedido { get; set; }
        public IEnumerable<ProdutoComboDtoPrepedido> ListaProdutos { get; set; }
    }
}