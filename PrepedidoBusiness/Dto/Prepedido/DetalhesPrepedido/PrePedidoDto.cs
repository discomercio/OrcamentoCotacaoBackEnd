using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido
{
    public class PrePedidoDto
    {
        public DateTime DataPrepedido { get; set; }
        public string NumeroPrePedido { get; set; }
        public DadosClienteDtoPrepedido DadosClientePrePedido { get; set; }
        public IEnumerable<ProdutoComboDtoPrepedido> ListaProdutos { get; set; }
        public DetalhesDtoPrepedido DetalhesPrepedidos { get; set; }
    }
}