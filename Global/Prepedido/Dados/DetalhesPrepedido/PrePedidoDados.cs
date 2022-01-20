using Cliente.Dados;
using FormaPagamento.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class PrePedidoDados
    {
        public string CorHeader { get; set; }
        public string TextoHeader { get; set; }
        public string CanceladoData { get; set; }
        public string NumeroPrePedido { get; set; }
        public string DataHoraPedido { get; set; }
        public string Hora_Prepedido { get; set; }
        public DadosClienteCadastroDados DadosCliente { get; set; }
        public EnderecoCadastralClientePrepedidoDados EnderecoCadastroClientePrepedido { get; set; }
        public EnderecoEntregaClienteCadastroDados EnderecoEntrega { get; set; }
        public List<PrepedidoProdutoPrepedidoDados> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public string CorTotalFamiliaRA { get; set; }
        public float? PercRT { get; set; }
        public decimal Vl_total_NF { get; set; }
        public decimal Vl_total { get; set; }
        public DetalhesPrepedidoDados DetalhesPrepedido { get; set; }
        public List<string> FormaPagto { get; set; }
        public FormaPagtoCriacaoDados FormaPagtoCriacao { get; set; }
        public bool St_Orc_Virou_Pedido { get; set; }//se virou pedido retornar esse campo
        public string NumeroPedido { get; set; }//se virou pedido retornar esse campo
    }
}
