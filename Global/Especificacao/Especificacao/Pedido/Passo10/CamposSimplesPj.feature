@Especificacao.Pedido.Passo10.CamposSimples
Feature: Validar campos simples Pj

Background: Configuração
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	#por enquanto, ignoramos no prepedido inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#também ignoramos na API magneto porque só aceita pedidos de PF
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

@ListaDependencias
Scenario: CamposSimples ListaDependencias Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias"
	Given Implementado em "Especificacao.Pedido.Pedido.PedidoListaDependencias"


Scenario: Validar quer CPF já esteja cadastrado
	#está aqui porque a ApiMagento sempre cadastra o cliente, caso ele não exista
	When Pedido base cliente PF
	And  Informo "CPF/CNPJ" = "687.307.550-77"
	Then Erro "O cliente não está cadastrado: 68730755077"
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-51"
	Then Erro "O cliente não está cadastrado: 12584718000151"


Scenario: Validar CNPJ
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-5"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-99"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-xx"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-11"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-53"
	Then Erro "CNPJ INVÁLIDO."

	When Pedido base cliente PJ
	And  Informo "CPF/CNPJ" = "12.584.718/0001-51"
	Then Sem Erro "CNPJ INVÁLIDO."

Scenario: Validar EnderecoCadastralCliente.Endereco_tipo_pessoa
	When Pedido base
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "XX"
	Then Erro "Tipo de cliente não é PF nem PJ."

