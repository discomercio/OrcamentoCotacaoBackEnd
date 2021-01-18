@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: tel_res


Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

Scenario: validar telefones - tel_res
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = ""
	Then Erro "PREENCHA O TELEFONE RESIDENCIAL."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "null"
	Then Erro "PREENCHA O TELEFONE RESIDENCIAL."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "123"
	Then Erro "TELEFONE RESIDENCIAL INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "123456789012"
	Then Erro "TELEFONE RESIDENCIAL INVÁLIDO."

Scenario: validar telefones - tel_ddd_res 2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = ""
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "PREENCHA O DDD RESIDENCIAL."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "null"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "PREENCHA O DDD RESIDENCIAL."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "1"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "DDD RESIDENCIAL INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "123"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "DDD RESIDENCIAL INVÁLIDO."

Scenario: validar telefones - tel_res sucesso
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = ""
	When Informo "EndEtg_tel_res" = ""
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Sem nenhum erro

	

	

	

	


