using PrepedidoBusiness.Dtos.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class PedidoDto
    {
        public string NumeroPedido { get; set; }
        public string StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public List<PedidoProdutosDtoPedido> ListaProdutos { get; set; }
        public DetalhesNFPedidoDtoPedido DetalhesNF { get; set; }
        public DetalhesFormaPagamentos DetalhesFormaPagto { get; set; }
        public IEnumerable<ProdutoDevolvidoDtoPedido> ListaProdutoDevolvido{ get; set; }
        public IEnumerable<PedidoPerdasDtoPedido> ListaPerdas { get; set; }
        public BlocoNotasDtoPedido BlocoNotas { get; set; }

        //Incluir o dto de endereço de entrega
        //verificar os telefones comerciais
    }
}
