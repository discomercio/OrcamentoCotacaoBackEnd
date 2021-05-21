using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Validacao
{
    class ValidacaoCampos
    {
        public ValidacaoCampos()
        {
        }

        public void Executar()
        {
            /*
             * Especificacao\Pedido\Passo60\Validacao\DadosCadastrais\DadosCadastrais.feature
             * tratado no código em \arclube\Global\Pedido\Criacao\Passo10\Passo10.cs ValidarCliente
             * 
             * Especificacao\Pedido\Passo60\Validacao\blnPedidoECommerceCreditoOkAutomatico.feature
             * tratado no código em \arclube\Global\Pedido\Criacao\Passo30\CamposMagentoExigidos.cs:ConfigurarBlnPedidoECommerceCreditoOkAutomatico()
             * 
             * Especificacao\Pedido\Passo60\Validacao\vl_aprov_auto_analise_credito.feature
             * tratado no código em \arclube\Global\Pedido\Criacao\Passo60\Validacao\ConfigurarVariaveis.cs:ConfigurarVariaveisExecutar() Criacao.Execucao.Vl_aprov_auto_analise_credito
             * 
            */

            Validar_CustoFinancFornec();
            Validar_origem_pedido();
            Validar_RaIndicador();

        }

            //Especificacao\Pedido\Passo60\Validacao\CustoFinancFornec.feature
        private void Validar_CustoFinancFornec()
        {
            //feito em Pedido.Criacao.Passo40.Passo40.ListaProdutosFormaPagamento
            //na chamada UtilsGlobais.Util.ValidarTipoCustoFinanceiroFornecedor
        }

        //Especificacao\Pedido\Passo60\Validacao\origem_pedido.feature
        private void Validar_origem_pedido()
        {
            //campo c_origem_pedido do ASP é Pedido.Marketplace.Marketplace_codigo_origem

            //feito em Pedido.Criacao.Passo30.Passo30.ConfigurarBlnPedidoECommerceCreditoOkAutomatico
        }

        //Especificacao\Pedido\Passo60\Validacao\RaIndicador.feature
        private void Validar_RaIndicador()
        {
            //feito em Pedido.Criacao.Passo30.Passo30.Indicador()
            //em \arclube\Global\Pedido\Criacao\Passo30\Indicador.cs
        }

    }
}
