@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: tel_com_2

Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

Scenario: validar telefone - tel_com_2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = ""
	Then Erro "PREENCHA O TELEFONE COMERCIAL2."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "null"
	Then Erro "PREENCHA O TELEFONE COMERCIAL2."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "1234"
	Then Erro "TELEFONE COMERCIAL2 INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "123456789012"
	Then Erro "TELEFONE COMERCIAL2 INVÁLIDO."

Scenario: validar telefone - tel_ddd_com_2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = "12345678"
	Then Erro "PREENCHA O DDD DO TELEFONE COMERCIAL2."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "null"
	When Informo "EndEtg_tel_com_2" = "12345678"
	Then Erro "PREENCHA O DDD DO TELEFONE COMERCIAL2."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "1"
	When Informo "EndEtg_tel_com_2" = "12345678"
	Then Erro "DDD DO TELEFONE COMERCIAL2 INVÁLIDO."

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "123"
	When Informo "EndEtg_tel_com_2" = "12345678"
	Then Erro "DDD DO TELEFONE COMERCIAL2 INVÁLIDO."
	
Scenario: validar telefone - tel_ramal_com_2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = ""
	When Informo "EndEtg_ramal_com_2" = "21"
	Then Erro "Ramal comercial 2 preenchido sem telefone!"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = "12345678"
	When Informo "EndEtg_ramal_com_2" = "21"
	Then Erro "Ramal comercial 2 preenchido sem telefone!"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = ""
	When Informo "EndEtg_ramal_com_2" = "21"
	Then Erro "Ramal comercial 2 preenchido sem telefone!"

Scenario: validar telefone - tel_com_2 sucesso
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "12345678"
	When Informo "EndEtg_ramal_com_2" = ""
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = ""
	When Informo "EndEtg_ramal_com_2" = ""
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "null"
	When Informo "EndEtg_tel_com_2" = "null"
	When Informo "EndEtg_ramal_com_2" = "null"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "12345678"
	When Informo "EndEtg_ramal_com_2" = "21"
	Then Sem nenhum erro