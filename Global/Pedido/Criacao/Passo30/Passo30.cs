﻿using InfraBanco.Constantes;
using Pedido.Criacao.UtilsLoja;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UtilsGlobais.Usuario;

namespace Pedido.Criacao.Passo30
{
    class Passo30
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo30(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Executar()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            PermissaoPercRt();
        }

        private void PermissaoPercRt()
        {
            if (pedido.Valor.PercRT == 0)
                return;


            bool erro = false;
            if (!pedidoCriacao.UsuarioPermissao.Permitido(Constantes.OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO))
                erro = true;
            //NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca pode
            if (pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                erro = true;

            if (erro)
                retorno.ListaErros.Add("Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)");


            ValidarPercentualRT(pedido.Valor.PercRT, pedidoCriacao.PercentualMaxDescEComissao.PercMaxComissao, retorno.ListaErros);
        }

        private void ValidarPercentualRT(float percComissao, float percentualMax, List<string> lstErros)
        {
            if (percComissao < 0 || percComissao > 100)
            {
                lstErros.Add("Percentual de comissão inválido.");
            }
            if (percComissao > percentualMax)
            {
                lstErros.Add("O percentual de comissão excede o máximo permitido.");
            }
        }

    }
}
