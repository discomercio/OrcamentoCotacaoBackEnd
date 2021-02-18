@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: tel_com

Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

Scenario: validar telefones - tel_com
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = ""
	Then Erro "PREENCHA O TELEFONE COMERCIAL."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "null"
	Then Erro "PREENCHA O TELEFONE COMERCIAL."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "123"
	Then Erro "TELEFONE COMERCIAL INVÁLIDO."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "123456789012"
	Then Erro "TELEFONE COMERCIAL INVÁLIDO."

Scenario: validar telefone - tel_ddd_com
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "PREENCHA O DDD COMERCIAL."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "null"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "PREENCHA O DDD COMERCIAL."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "1"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "DDD DO TELEFONE COMERCIAL INVÁLIDO."
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "123"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "DDD DO TELEFONE COMERCIAL INVÁLIDO."

Scenario: validar telefone - tel_ramal_com
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = ""
	When Informo "EndEtg_ramal_com" = "21"
	Then Erro "Ramal comercial preenchido sem telefone!"
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = "12345678"
	When Informo "EndEtg_ramal_com" = "21"
	Then Erro "Ramal comercial preenchido sem telefone!"
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = ""
	When Informo "EndEtg_ramal_com" = "21"
	Then Erro "Ramal comercial preenchido sem telefone!"

Scenario: validar telefone - tel_com sucesso
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = ""
	When Informo "EndEtg_ramal_com" = ""
	Then Sem nenhum erro
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "null"
	When Informo "EndEtg_tel_com" = "null"
	When Informo "EndEtg_ramal_com" = "null"
	Then Sem nenhum erro
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "12345678"
	When Informo "EndEtg_ramal_com" = ""
	Then Sem nenhum erro
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "12345678"
	When Informo "EndEtg_ramal_com" = "null"
	Then Sem nenhum erro
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "12345678"
	When Informo "EndEtg_ramal_com" = "21"
	Then Sem nenhum erro

@ignore
Scenario: validar telefone - tel_com com simbolos
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "1234-5678"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "Endereco_tel_com" = "12345678"
