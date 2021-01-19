﻿using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo20
{
    class Passo20
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo20(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task ValidarEnderecoEntrega()
        {
            /* valida endereço de entrega */
            await pedidoCriacao.validacoesPrepedidoBll.ValidarEnderecoEntrega(pedido.EnderecoEntrega, retorno.ListaErros,
                pedido.Ambiente.Indicador, pedido.Cliente.Tipo.ParaString(), false, pedido.Ambiente.Loja);
        }
    }
}
