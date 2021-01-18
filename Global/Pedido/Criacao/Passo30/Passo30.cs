using InfraBanco.Constantes;
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
        private readonly PedidoCriacaoRetornoDados pedidoRetorno;
        private readonly UtilsGlobais.Usuario.UsuarioPermissao usuarioPermissao;
        private readonly Pedido.Criacao.UtilsLoja.PercentualMaxDescEComissao percentualMaxDescEComissao;
        public Passo30(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados pedidoRetorno, UsuarioPermissao usuarioPermissao, PercentualMaxDescEComissao percentualMaxDescEComissao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.pedidoRetorno = pedidoRetorno ?? throw new ArgumentNullException(nameof(pedidoRetorno));
            this.usuarioPermissao = usuarioPermissao ?? throw new ArgumentNullException(nameof(usuarioPermissao));
            this.percentualMaxDescEComissao = percentualMaxDescEComissao ?? throw new ArgumentNullException(nameof(percentualMaxDescEComissao));
        }

        public void Executar()
        {
            PermissaoPercRt();
        }

        private void PermissaoPercRt()
        {
            if (pedido.Valor.PercRT == 0)
                return;


            bool erro = false;
            if (!usuarioPermissao.Permitido(Constantes.OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO))
                erro = true;
            //NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca pode
            if (pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                erro = true;

            if (erro)
                pedidoRetorno.ListaErros.Add("Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)");


            ValidarPercentualRT(pedido.Valor.PercRT, percentualMaxDescEComissao.PercMaxComissao, pedidoRetorno.ListaErros);
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
