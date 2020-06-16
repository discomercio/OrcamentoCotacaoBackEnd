using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }

        [MaxLength(14)]
        [Required]
        public string Cnpj_Cpf { get; set; }

        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }
        //public DadosClienteCadastroUnisDto DadosCliente { get; set; }

        //Talvez incluir uma Flag para endereço cadastral
        public EnderecoCadastralClientePrepedidoUnisDto EnderecoCadastralCliente { get; set; }
        public EnderecoEntregaClienteCadastroUnisDto EnderecoEntrega { get; set; }
        public List<PrePedidoProdutoPrePedidoUnisDto> ListaProdutos { get; set; }
        public bool PermiteRAStatus { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesPrePedidoUnisDto DetalhesPrepedido { get; set; }
        public FormaPagtoCriacaoUnisDto FormaPagtoCriacao { get; set; }        
    }
}
