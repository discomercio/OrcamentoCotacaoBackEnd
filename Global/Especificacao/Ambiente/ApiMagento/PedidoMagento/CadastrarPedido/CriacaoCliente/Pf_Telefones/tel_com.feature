@ignore
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
	Then Erro "EndEtg_tel_com faltando"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "EndEtg_ddd_res faltando"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = ""
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "1"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "EndEtg_ddd_res curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "123"
	Then Erro "EndEtg_tel_res curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "123"
	When Informo "EndEtg_tel_com" = "12345678"
	Then Erro "EndEtg_ddd_res muito grande"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_com" = "12"
	When Informo "EndEtg_tel_com" = "1234567890"
	Then Erro "EndEtg_tel_res muito grande"

@ignore
Scenario: EndEtg_ramal_com
	Given Falta validar este campo

