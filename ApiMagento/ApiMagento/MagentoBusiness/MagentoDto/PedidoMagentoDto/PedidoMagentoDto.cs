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
        
        /// <summary>
        /// Usuário do sistema
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }
        public EnderecoCadastralClientePrepedidoMagentoDto EnderecoCadastralCliente { get; set; }

        [Required]
        public bool OutroEndereco { get; set; }
        public EnderecoEntregaClienteCadastroMagentoDto EnderecoEntrega { get; set; }
        public List<PrePedidoProdutoPrePedidoMagentoDto> ListaProdutos { get; set; }
        //acho que essa flag não precisa, pois ao escolher indicador pode ser que o indicador selecionado 
        //permita ou não RA e caso permita pode ser escolhido com RA

        /// <summary>
        /// Verificar com Hamilton se há necessidade desse campo 
        /// </summary>
        public bool PermiteRAStatus { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesPrePedidoMagentoDto DetalhesPrepedido { get; set; }
        public FormaPagtoCriacaoMagentoDto FormaPagtoCriacao { get; set; }
        //public int CDSelecionado { get; set; }
        //[Required]
        //public bool CDManual { get; set; }
        /// <summary>
        /// Percentual de comissão
        /// </summary>
        public float? PercRT { get; set; }
        public bool OpcaoVendaSemEstoque { get; set; }

        //verificar se esse campo pode ser bool pq ele recebe "S" ou "N"
        //obs:esse campo não é salvo na base, é utilizado para saber se é com RA ou sem RA, talvez mudar para bool
        /// <summary>
        /// Verificar com Hamilton se há necessidade desse campo.
        /// Armazena a opção com ou sem RA, é habilitado dependendo do Indicador selecionado para o Pedido
        /// </summary>
        public string OpcaoPossuiRA { get; set; }
        /// <summary>
        /// Indicador selecionado para o Pedido
        /// </summary>
        public string NomeIndicador { get; set; }
    }
}
