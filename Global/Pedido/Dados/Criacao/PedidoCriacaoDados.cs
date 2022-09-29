using Cliente.Dados;
using FormaPagamento.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;

#nullable enable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoDados
    {
        public PedidoCriacaoDados(
            PedidoCriacaoAmbienteDados ambiente, PedidoCriacaoConfiguracaoDados configuracao,
            PedidoCriacaoExtraDados extra, PedidoCriacaoClienteDados cliente,
            EnderecoCadastralClientePrepedidoDados enderecoCadastralCliente,
            EnderecoEntregaClienteCadastroDados enderecoEntrega, List<PedidoCriacaoProdutoDados> listaProdutos,
            PedidoCriacaoValorDados valor, FormaPagtoCriacaoDados formaPagtoCriacao,
            DetalhesPrepedidoDados detalhesPedido, PedidoCriacaoMarketplaceDados marketplace)
        {
            Ambiente = ambiente ?? throw new ArgumentNullException(nameof(ambiente));
            Configuracao = configuracao ?? throw new ArgumentNullException(nameof(configuracao));
            Extra = extra ?? throw new ArgumentNullException(nameof(extra));
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            EnderecoCadastralCliente = enderecoCadastralCliente ?? throw new ArgumentNullException(nameof(enderecoCadastralCliente));
            EnderecoEntrega = enderecoEntrega ?? throw new ArgumentNullException(nameof(enderecoEntrega));
            ListaProdutos = listaProdutos ?? throw new ArgumentNullException(nameof(listaProdutos));
            Valor = valor ?? throw new ArgumentNullException(nameof(valor));
            FormaPagtoCriacao = formaPagtoCriacao ?? throw new ArgumentNullException(nameof(formaPagtoCriacao));
            DetalhesPedido = detalhesPedido ?? throw new ArgumentNullException(nameof(detalhesPedido));
            Marketplace = marketplace ?? throw new ArgumentNullException(nameof(marketplace));
        }

        public PedidoCriacaoAmbienteDados Ambiente { get; }

        public PedidoCriacaoConfiguracaoDados Configuracao { get; }

        public PedidoCriacaoExtraDados Extra { get; }

        //Armazena os dados cadastrados do cliente
        public PedidoCriacaoClienteDados Cliente { get; }

        //Armazena os dados do cliente para o Pedido
        public Cliente.Dados.EnderecoCadastralClientePrepedidoDados EnderecoCadastralCliente { get; }

        //Armazena os dados de endereço de entrega
        public Cliente.Dados.EnderecoEntregaClienteCadastroDados EnderecoEntrega { get; }

        //Armazena os dados dos produtos selecionados
        public List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> ListaProdutos { get; }

        //totais e detalhes do RA
        public PedidoCriacaoValorDados Valor { get; }

        //Armazena os dados da forma de pagto selecionado
        public FormaPagtoCriacaoDados FormaPagtoCriacao { get; }

        //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
        public Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPedido { get; }

        //campos do marketplace
        public PedidoCriacaoMarketplaceDados Marketplace { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Estilo de código")]
        public static Prepedido.Dados.DetalhesPrepedido.PrePedidoDados PrePedidoDadosDePedidoCriacaoDados(PedidoCriacaoDados pedido, string id_cliente)
        {
            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prepedido = new Prepedido.Dados.DetalhesPrepedido.PrePedidoDados();
            prepedido.DadosCliente = global::Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(pedido.EnderecoCadastralCliente,
                pedido.Ambiente.Indicador, pedido.Ambiente.Loja, "", null, id_cliente, null);
            prepedido.ListaProdutos = Pedido.Dados.Criacao.PedidoCriacaoProdutoDados.PrepedidoProdutoPrepedidoDados_De_PedidoCriacaoProdutoDados(pedido.ListaProdutos);
            prepedido.FormaPagtoCriacao = pedido.FormaPagtoCriacao;
            prepedido.EnderecoEntrega = pedido.EnderecoEntrega;
            prepedido.EnderecoCadastroClientePrepedido = pedido.EnderecoCadastralCliente;
            prepedido.DetalhesPrepedido = pedido.DetalhesPedido;
            prepedido.Vl_total = pedido.Valor.Vl_total;
            prepedido.Vl_total_NF = pedido.Valor.Vl_total_NF;
            prepedido.PermiteRAStatus = (short)(pedido.Valor.PedidoPossuiRa() ? 1 : 0);

            return prepedido;
        }
    }
}
