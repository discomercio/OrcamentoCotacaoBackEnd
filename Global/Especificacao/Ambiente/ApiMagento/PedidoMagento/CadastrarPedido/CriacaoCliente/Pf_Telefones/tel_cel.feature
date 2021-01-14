@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: tel_cel

Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

Scenario: validar telefone - tel_cel
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = ""
	Then Erro "PREENCHA O TELEFONE CELULAR."

Scenario: validar telefone - tel_cel 2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = ""
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "PREENCHA O DDD CELULAR."

Scenario: validar telefone - tel_cel 3
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = ""
	When Informo "EndEtg_tel_cel" = ""
	Then Sem nenhum erro 

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Sem nenhum erro 

@ignore
Scenario: validar telefone - tel_cel 4
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "1"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "EndEtg_ddd_cel muito curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "1234"
	Then Erro "EndEtg_tel_cel esta muito curto"

	Given Pedido base
	When Informo "OurtroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "123"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "EndEtg_ddd_cel esta muito longo"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "123456789"
	Then Erro "EndEtg_tel_cel esta muito longo"
