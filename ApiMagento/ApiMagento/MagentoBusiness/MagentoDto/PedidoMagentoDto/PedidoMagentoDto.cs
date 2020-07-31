using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoMagentoDto
    {
        [Required]
        public string TokenAcesso { get; set; }

        [MaxLength(14)]
        [Required]
        public string Cnpj_Cpf { get; set; }

        [Required]
        public InfCriacaoPedidoMagentoDto InfCriacaoPedido { get; set; }

        [Required]
        public EnderecoCadastralClienteMagentoDto EnderecoCadastralCliente { get; set; }

        /// <summary>
        /// Indica se existe um endereço de entrega
        /// <hr />
        /// </summary>
        [Required]
        public bool OutroEndereco { get; set; }

        public EnderecoEntregaClienteMagentoDto EnderecoEntrega { get; set; }

        [Required]
        public List<PedidoProdutoMagentoDto> ListaProdutos { get; set; }

        //acho que essa flag não precisa, pois ao escolher indicador pode ser que o indicador selecionado 
        //permita ou não RA e caso permita pode ser escolhido com RA
        /// <summary>
        /// Verificar com Hamilton se há necessidade desse campo 
        /// <hr />
        /// </summary>
        [Required]
        public bool PermiteRAStatus { get; set; }

        [Required]
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        [Required]
        public decimal? VlTotalDestePedido { get; set; }
        [Required]
        public DetalhesPedidoMagentoDto DetalhesPrepedido { get; set; }
        [Required]
        public FormaPagtoCriacaoMagentoDto FormaPagtoCriacao { get; set; }

        //public int CDSelecionado { get; set; }
        //[Required]
        //public bool CDManual { get; set; }
        /// <summary>
        /// Percentual de comissão
        /// <hr />
        /// </summary>
        public float? PercRT { get; set; }

        /// <summary>
        /// Verificar com Hamilton se há necessidade desse campo 
        /// <hr />
        /// </summary>
        [Required]
        public bool OpcaoVendaSemEstoque { get; set; }

        //verificar se esse campo pode ser bool pq ele recebe "S" ou "N"
        //obs:esse campo não é salvo na base, é utilizado para saber se é com RA ou sem RA, talvez mudar para bool
        /// <summary>
        /// Verificar com Hamilton se há necessidade desse campo.
        /// <br />
        /// Armazena a opção com ou sem RA, é habilitado dependendo do Indicador selecionado para o Pedido
        /// <hr />
        /// </summary>
        [Required]
        public string OpcaoPossuiRA { get; set; }
    }
}
