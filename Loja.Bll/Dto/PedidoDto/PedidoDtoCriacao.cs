using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto
{
    public class PedidoDtoCriacao
    {
        //podemos montar o dto com as informações existente e armazenar na Session
        //DadosClienteCadastroDto
        //EnderecoEntregaDtoClienteCadastro
        //List<PedidoProdutosPedidoDto>                    
        //FormaPagtoCriacao
        //opção de venda sem estoque
        //strPercLimiteRASemDesagio
        //strPercDesagio
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PedidoProdutosDtoPedido> ListaProdutos { get; set; }
        public FormaPagtoCriacaoDto FormaPagtoCriacao { get; set; }
        public bool OpcaoVendaSemEstoque { get; set; }
        public decimal StrPercLimiteRASemDesagio { get; set; }
        public decimal StrPercDesagio { get; set; }
        public bool ComRA { get; set; }
        public string Orcamentista { get; set; }
        public string NumPedido { get; set; }
        public bool ComIndicacao { get; set; }
        public float PercComissao { get; set; }
        public string Loja { get; set; }
        public string DataHoraPedido { get; set; }
        public DetalhesNFPedidoDtoPedido DetalhesNF { get; set; }





    }
}
