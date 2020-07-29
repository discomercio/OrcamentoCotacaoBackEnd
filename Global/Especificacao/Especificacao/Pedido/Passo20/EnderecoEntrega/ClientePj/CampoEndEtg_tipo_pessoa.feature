@ignore
@Especificacao/Pedido
@Ambiente/PrepedidoApi
@Ambiente/ApiUnis
Feature: Pedido de cliente PJ com endereço de entrega

Scenario: Validar tipo de pessoa 1
em loja/ClienteEdita.asp:
var EndEtg_tipo_pessoa = $('input[name="EndEtg_tipo_pessoa"]:checked').val();

	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = ""
	Then Erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 2
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "XX"
	Then Erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 3
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PJ"
	Then Sem erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 4
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PF"
	Then Sem erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

