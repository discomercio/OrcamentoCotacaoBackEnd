using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Validacao
{
    class ValidacaoCampos
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public ValidacaoCampos(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task Executar()
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

        // Especificacao\Pedido\Passo60\Validacao\CustoFinancFornec.feature
        private void Validar_CustoFinancFornec()
        {
            //todo: fazer Especificacao\Pedido\Passo60\Validacao\CustoFinancFornec.feature
            /*
            Scenario: A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
                #loja/PedidoNovoConfirma.asp
                #if (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) And _
                #   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) And _
                #   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
                #	alerta = "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
                Given Pedido base
                When Informo "CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO inválido"
                Then Erro "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."

            Scenario: Não foi informada a quantidade de parcelas para a forma de pagamento selecionada
                #loja/PedidoNovoConfirma.asp
                #if (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) Or _
                #   (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
                #	if converte_numero(c_custoFinancFornecQtdeParcelas) <= 0 then
                #		alerta = "Não foi informada a quantidade de parcelas para a forma de pagamento selecionada (" & descricaoCustoFinancFornecTipoParcelamento(c_custoFinancFornecTipoParcelamento) &  ")"
                Given Pedido base
                When Informo "CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
                When Informo "CustoFinancFornecQtdeParcelas" = "0"
                Then Erro "regex Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

                Given Pedido base
                When Informo "CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
                When Informo "CustoFinancFornecQtdeParcelas" = "0"
                Then Erro "regex Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

            */
        }

        //Especificacao\Pedido\Passo60\Validacao\origem_pedido.feature
        private void Validar_origem_pedido()
        {
            //todo: ásso60 Especificacao\Pedido\Passo60\Validacao\origem_pedido.feature
            /*
            #ignorando porque naõ se aplica ao magento e na loja vamos verificar quando implementar
            @Especificacao.Pedido.Passo60
            Feature: origem_pedido


            Background: não executar no prepedido
                #ignoramos no prepedido inteiro
                Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
                #no magento é testado separadamente
                Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

            Scenario: somente para resolver a lista de dependencias
                Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

            @ignore
            Scenario: Validar origem_pedido
                #loja/PedidoNovoConsiste.asp
                #	set r = cn.Execute("SELECT * FROM t_CODIGO_DESCRICAO WHERE (grupo='PedidoECommerce_Origem') AND (st_inativo=0) ORDER BY ordenacao")
                #	if ($("#c_loja").val()==NUMERO_LOJA_ECOMMERCE_AR_CLUBE){
                #		if ($("#c_origem_pedido").val() == ""){
                #			alert("Selecione a origem do pedido (marketplace)!");
                #			$("#c_origem_pedido").focus();
                #			return;
                #		}
                #o campo origem_pedido precisa ser um desses (se loja NUMERO_LOJA_ECOMMERCE_AR_CLUBE, ele é exigido)
                Given Pedido base
                When Informo "DadosCliente.Loja" = "201"
                When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
                Then Erro "Selecione a origem do pedido (marketplace)!"
                Given Pedido base
                When Informo "DadosCliente.Loja" = "201"
                #"OrigemPedido" = "001" => Arclube (e-commerce)
                #"OrigemPedido" = "002" => Arclube (televendas)
                #"OrigemPedido" = "003" => Americanas
                #"OrigemPedido" = "004" => Submarino
                # inclui esses códigos caso tenha que ser alterado
                When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
                Then Sem nenhum erro

            @ignore
            Scenario: Validar origem_pedido2
                #		if ($("#c_pedido_ac").val() != "") {
                #		    if(retorna_so_digitos($("#c_pedido_ac").val()) != $("#c_pedido_ac").val()) {
                #		        alert("O número Magento deve conter apenas dígitos!");
                #		        $("#c_pedido_ac").focus();
                #		        return;
                #		    }
                #		}
                #	}
                Given Pedido base
                When Informo "DadosCliente.Loja" = "201"
                When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
                When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
                Then Erro "O número Magento deve conter apenas dígitos!"

            @ignore
            Scenario: Validar origem_pedido3
                #	if (FLAG_MAGENTO_PEDIDO_COM_INDICADOR)
                #	{
                #		if ($("#c_pedido_ac").val() != "") {
                #			if(retorna_so_digitos($("#c_pedido_ac").val()) != $("#c_pedido_ac").val()) {
                #				alert("O número Magento deve conter apenas dígitos!");
                #				$("#c_pedido_ac").focus();
                #				return;
                #			}
                #		}
                #	}
                #
                #verificar se o indicador é inserido de forma automática no magento
                Given Pedido base
                When Informo "DadosCliente.Loja" = "201"
                When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
                When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
                Then Erro "O número Magento deve conter apenas dígitos!"

            @ignore
            Scenario: Validar origem_pedido4
                Given Pedido base
                When Informo "DadosCliente.Loja" = "201"
                When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
                When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
                Then Erro "ver qual o erro"

            */
        }

        //Especificacao\Pedido\Passo60\Validacao\RaIndicador.feature
        private void Validar_RaIndicador()
        {
            //feito em Pedido.Criacao.Passo30.Passo30.Indicador()
            //em \arclube\Global\Pedido\Criacao\Passo30\Indicador.cs
        }

    }
}
