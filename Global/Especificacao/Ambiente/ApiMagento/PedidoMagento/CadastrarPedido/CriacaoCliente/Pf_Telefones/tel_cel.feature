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

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "null"
	Then Erro "PREENCHA O TELEFONE CELULAR."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "1234"
	Then Erro "TELEFONE CELULAR INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "12"
	When Informo "EndEtg_tel_cel" = "123456789012"
	Then Erro "TELEFONE CELULAR INVÁLIDO."

Scenario: validar telefone - tel_ddd_cel
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = ""
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "PREENCHA O DDD CELULAR."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "null"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "PREENCHA O DDD CELULAR."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "1"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "DDD CELULAR INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_cel" = "123"
	When Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "DDD CELULAR INVÁLIDO."

Scenario: validar telefone - tel_cel sucesso
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

	

	

	
