using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoDados
    {
        //Armazena a loja do usuário logado
        public string LojaUsuario { get; set; }

        //Armazena nome do usuário logado
        public string Usuario { get; set; }

        //Armazena os dados cadastrados do cliente
        public Cliente.Dados.DadosClienteCadastroDados DadosCliente { get; set; }

        //Armazena os dados do cliente para o Pedido
        public Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralCliente { get; set; }

        //Armazena os dados de endereço de entrega
        public Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntrega { get; set; }

        //Armazena os dados dos produtos selecionados
        public List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> ListaProdutos { get; set; }

        //Armazena os dados da forma de pagto selecionado
        public Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacao { get; set; }

        //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
        public Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPedido { get; set; }

        //Flag para saber se tem indicador selecionado 
        public bool ComIndicador { get; set; }

        //Armazena o nome do indicador selecionado
        public string NomeIndicador { get; set; }

        //Armazena o percentual de comissão para o indicador selecionado
        public float PercRT { get; set; }

        //Armazena "S" ou "N" para caso de o indicador selecionado permita RA
        public bool OpcaoPossuiRa { get; set; }

        //Armazena o id do centro de distribuição selecionado manualmente
        //Obs: armazena "0" caso seja automático
        public int IdNfeSelecionadoManual { get; set; }

        //Armazena o nome do vendedor externo
        //obs: analisar melhor quando esse campos será preenchido
        public string VendedorExterno { get; set; }

        //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
        public bool OpcaoVendaSemEstoque { get; set; }

        //Armazena o valor total do pedido
        public decimal Vl_total { get; set; }

        //Armazena o valor total de pedido com RA
        //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
        public decimal Vl_total_NF { get; set; }

        public short PermiteRAStatus { get; set; }

    }
}
