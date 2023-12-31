﻿using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UtilsGlobais.Usuario;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30 : PassoBase
    {
        public Passo30(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public async Task ExecutarAsync()
        {
            //esta rotina verifica o perc_rt infomrado pelo usuário. A rotina de baixo altera o valor, 
            //então a validação nesse campo precisa ser feita antes para validr somente o que o usuário informou,
            //e não o que o sistema alterou
            PermissaoPercRt();

            //esta rotina também determina o perc_Rt para o magento
            //protanto precisa ser chama antes das outras que usam o perc_rt
            await CamposMagentoExigidos(); 

            CamposMagentoNaoAceitos();
            await Cd();
            await Cod_site_assistencia_tecnica();
            await Indicador();
            Opcao_possui_RA();
        }

        private void PermissaoPercRt()
        {
            if (Pedido.Valor.Perc_RT == 0)
                return;


            bool erro = false;
            if (!Criacao.Execucao.UsuarioPermissao.Operacao_permitida(Constantes.OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO))
                erro = true;
            //NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca pode
            if (Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                erro = true;

            if (erro)
                Retorno.ListaErros.Add("Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)");


            ValidarPercentualRT(Pedido.Valor.Perc_RT, Criacao.Execucao.PercentualMaxDescEComissao.Perc_Max_Comissao, Retorno.ListaErros);
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
