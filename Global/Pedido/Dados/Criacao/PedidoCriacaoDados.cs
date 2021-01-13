﻿using Cliente.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

//todo: religar nullable
#nullable disable

namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoDados
    {
        public PedidoCriacaoDados(PedidoCriacaoAmbienteDados ambiente, PedidoCriacaoConfiguracaoDados configuracao, PedidoCriacaoClienteDados cliente, EnderecoCadastralClientePrepedidoDados enderecoCadastralCliente, EnderecoEntregaClienteCadastroDados enderecoEntrega, List<PedidoCriacaoProdutoDados> listaProdutos, PedidoCriacaoValorDados valor, FormaPagtoCriacaoDados formaPagtoCriacao, DetalhesPrepedidoDados detalhesPedido, PedidoCriacaoMarketplaceDados marketplace)
        {
            Ambiente = ambiente ?? throw new ArgumentNullException(nameof(ambiente));
            Configuracao = configuracao ?? throw new ArgumentNullException(nameof(configuracao));
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
        public Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados FormaPagtoCriacao { get; }

        //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
        public Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados DetalhesPedido { get; }

        //campos do marketplace
        public PedidoCriacaoMarketplaceDados Marketplace { get; }

        public static Prepedido.Dados.DetalhesPrepedido.PrePedidoDados PrePedidoDadosDePedidoCriacaoDados(PedidoCriacaoDados pedido)
        {
            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prepedido = new Prepedido.Dados.DetalhesPrepedido.PrePedidoDados();
            prepedido.DadosCliente = global::Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(pedido.EnderecoCadastralCliente,
                pedido.Ambiente.Indicador, pedido.Ambiente.Loja, "", null, pedido.Cliente.Id_cliente);
            prepedido.ListaProdutos = Pedido.Dados.Criacao.PedidoCriacaoProdutoDados.PrepedidoProdutoPrepedidoDados_De_PedidoCriacaoProdutoDados(pedido.ListaProdutos);
            prepedido.FormaPagtoCriacao = pedido.FormaPagtoCriacao;
            prepedido.EnderecoEntrega = pedido.EnderecoEntrega;
            prepedido.EnderecoCadastroClientePrepedido = pedido.EnderecoCadastralCliente;
            prepedido.DetalhesPrepedido = pedido.DetalhesPedido;
            prepedido.Vl_total = pedido.Valor.Vl_total;
            prepedido.Vl_total_NF = pedido.Valor.Vl_total_NF;
            prepedido.PermiteRAStatus = pedido.Valor.PermiteRAStatus;

            return prepedido;
        }
    }
}
