using System;
using System.Collections.Generic;
using System.Text;

//todo: religar nullable
#nullable disable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoDados
    {
        public PedidoCriacaoDados()
        {
        }

        public InfraBanco.Constantes.Constantes.CodSistemaResponsavel SistemaResponsavelCadastro { get; set; }

        //Armazena a loja do usuário logado
        public string LojaUsuario { get; set; }

        //Armazena nome do usuário logado
        public string Usuario { get; set; }


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

        //Armazena se é venda externa
        public bool Venda_Externa { get; set; }

        //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
        public bool OpcaoVendaSemEstoque { get; set; }

        //Armazena o valor total do pedido
        public decimal Vl_total { get; set; }

        //Armazena o valor total de pedido com RA
        //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
        public decimal Vl_total_NF { get; set; }

        public short PermiteRAStatus { get; set; }

        public string Pedido_bs_x_ac { get; set; }
        public string Marketplace_codigo_origem { get; set; }
        public string Pedido_bs_x_marketplace { get; set; }

        public PedidoCriacaoConfiguracaoDados PedidoCriacaoConfiguracao { get; set; }

        //Armazena os dados cadastrados do cliente
        public PedidoCriacaoClienteDados DadosCliente { get; set; }

        //Armazena os dados do cliente para o Pedido
        public Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralCliente { get; set; }

        //Armazena os dados de endereço de entrega
        public Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntrega { get; set; }

        //Armazena os dados dos produtos selecionados
        public List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> ListaProdutos { get; set; }

        //Armazena os dados da forma de pagto selecionado
        public Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacao { get; set; }

        //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
        public Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPedido { get; set; }


        public static Prepedido.Dados.DetalhesPrepedido.PrePedidoDados PrePedidoDadosDePedidoCriacaoDados(PedidoCriacaoDados pedido)
        {
            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prepedido = new Prepedido.Dados.DetalhesPrepedido.PrePedidoDados();
            prepedido.DadosCliente = Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(pedido.EnderecoCadastralCliente,
                pedido.DadosCliente.Indicador_Orcamentista, pedido.DadosCliente.Loja, "", null, pedido.DadosCliente.Id_cliente);
            prepedido.ListaProdutos = Pedido.Dados.Criacao.PedidoCriacaoProdutoDados.PrepedidoProdutoPrepedidoDados_De_PedidoCriacaoProdutoDados(pedido.ListaProdutos);
            prepedido.FormaPagtoCriacao = pedido.FormaPagtoCriacao;
            prepedido.EnderecoEntrega = pedido.EnderecoEntrega;
            prepedido.EnderecoCadastroClientePrepedido = pedido.EnderecoCadastralCliente;
            prepedido.DetalhesPrepedido = pedido.DetalhesPedido;
            prepedido.Vl_total = pedido.Vl_total;
            prepedido.Vl_total_NF = pedido.Vl_total_NF;
            prepedido.PermiteRAStatus = pedido.PermiteRAStatus;

            return prepedido;
        }
    }
}
