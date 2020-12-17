@Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
Feature: Pedido de cliente PJ com endereço de entrega

Background: Api MAgento somente aceita pedidos PF
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"


Scenario: Validar tipo de pessoa 1
	#em loja/ClienteEdita.asp:
	#var EndEtg_tipo_pessoa = $('input[name="EndEtg_tipo_pessoa"]:checked').val();
	#loja/PedidoNovoConsiste.asp
	#if EndEtg_tipo_pessoa <> "PJ" and EndEtg_tipo_pessoa <> "PF" then
	#    alerta = "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = ""
	Then Erro "regex .*Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega.*"

Scenario: Validar tipo de pessoa 2
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "XX"
	Then Erro "regex .*Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega.*"

Scenario: Validar tipo de pessoa 3
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PJ"
	Then Sem erro "regex .*Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega.*"

Scenario: Validar tipo de pessoa 4
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PF"
	Then Sem erro "regex .*Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega.*"

