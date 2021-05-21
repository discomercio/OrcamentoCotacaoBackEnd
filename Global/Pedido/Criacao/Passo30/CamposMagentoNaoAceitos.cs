using InfraBanco.Constantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private void CamposMagentoNaoAceitos()
        {
            /*
                #se loja != NUMERO_LOJA_ECOMMERCE_AR_CLUBE then %>
                #nao pode ter campos c_numero_mktplace, c_origem_pedido, c_numero_magento
                #Pedido_bs_x_ac Pedido_bs_x_marketplace Marketplace_codigo_origem
             * */

            if (Pedido.Ambiente.Loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                return;
            //se ambiente = api magento, pode ter
            if (Pedido.Configuracao.SistemaResponsavelCadastro == Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
                return;

            if (!string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_ac))
                Retorno.ListaErros.Add("O campo Pedido_bs_x_ac não pode ser informado se loja != 201");
            if (!string.IsNullOrWhiteSpace(Pedido.Marketplace.Pedido_bs_x_marketplace))
                Retorno.ListaErros.Add("O campo Pedido_bs_x_marketplace não pode ser informado se loja != 201");
            if (!string.IsNullOrWhiteSpace(Pedido.Marketplace.Marketplace_codigo_origem))
                Retorno.ListaErros.Add("O campo Marketplace_codigo_origem não pode ser informado se loja != 201");
        }
    }
}
