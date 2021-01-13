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
	Then Erro "EndEtg_tel_res faltando"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = ""
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "EndEtg_tel_res faltando"

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

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "1"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "EndEtg_ddd_res curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "123"
	Then Erro "EndEtg_tel_res curto"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "123"
	When Informo "EndEtg_tel_res" = "12345678"
	Then Erro "EndEtg_ddd_res muito grande"

	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "1234567890"
	Then Erro "EndEtg_tel_res muito grande"


