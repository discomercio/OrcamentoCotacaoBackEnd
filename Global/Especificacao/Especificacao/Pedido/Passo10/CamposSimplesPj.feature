@Especificacao.Pedido.Passo10.CamposSimples
Feature: Validar campos simples Pj

Background: Configuração
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	#por enquanto, ignoramos no prepedido inteiro
	Given Ignorar feature no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#também ignoramos na API magneto porque só aceita pedidos de PF
	Given Ignorar feature no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

@ListaDependencias
Scenario: CamposSimples ListaDependencias Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias"
	Given Implementado em "Especificacao.Pedido.Pedido.PedidoListaDependencias"


Scenario: Validar CNPJ
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-5"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-99"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-xx"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-11"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "CNPJ INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-53"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "CNPJ INVÁLIDO."

	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-51"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Sem Erro "CNPJ INVÁLIDO."

Scenario: Validar EnderecoCadastralCliente.Endereco_tipo_pessoa
	When Pedido base
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "XX"
	Then Erro "Tipo de cliente não é PF nem PJ."

