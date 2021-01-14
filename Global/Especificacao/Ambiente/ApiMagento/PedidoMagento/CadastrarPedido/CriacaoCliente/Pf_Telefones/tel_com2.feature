@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: tel_com2

Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

@ignore
Scenario: validar telefone - tel_com_2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = ""
	Then Erro "EndEtg_tel_com2 faltando"

@ignore
Scenario: validar telefone - tel_com_2 2
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = ""
	When Informo "EndEtg_tel_com2" = "12345678"
	Then Erro "EndEtg_ddd_com2 faltando"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = ""
	When Informo "EndEtg_tel_com2" = ""
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = "12"
	When Informo "EndEtg_tel_com2" = "12345678"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = "1"
	When Informo "EndEtg_tel_com2" = "12345678"
	Then Erro "EndEtg_ddd_com2 muito curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = "12"
	When Informo "EndEtg_tel_com2" = "1234"
	Then Erro "EndEtg_tel_com2 muito curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = "123"
	When Informo "EndEtg_tel_com2" = "12345678"
	Then Erro "EndEtg_ddd_com2 muito grande"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com2" = "12"
	When Informo "EndEtg_tel_com2" = "1234567890"
	Then Erro "EndEtg_tel_com2 muito grande"

@ignore
Scenario: EndEtg_ramal_com_2
	Given Falta validar este campo
