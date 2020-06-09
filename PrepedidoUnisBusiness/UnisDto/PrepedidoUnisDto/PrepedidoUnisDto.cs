using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoUnisDto
    {
        public string TokenAcesso { get; set; }
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        public EnderecoCadastroClientePrepedidoUnisDto EnderecoCadastroClienteNovo { get; set; }
        public EnderecoEntregaClienteCadastroUnisDto EnderecoEntrega { get; set; }
        public List<PrePedidoProdutoPrePedidoUnisDto> ListaProdutos { get; set; }
        public short PermiteRAStatus { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesPrePedidoUnisDto DetalhesPrepedido { get; set; }
        public FormaPagtoCriacaoUnisDto FormaPagtoCriacao { get; set; }        
    }
}
