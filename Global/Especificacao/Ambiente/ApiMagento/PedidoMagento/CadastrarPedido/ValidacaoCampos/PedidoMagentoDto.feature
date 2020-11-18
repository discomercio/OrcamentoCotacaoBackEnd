﻿@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos
Feature: PedidoMagentoDto
#testar a validação campo a campo

@ignore
Scenario: PedidoMagentoDto
	When Fazer esta validação

Scenario: InfCriacaoPedido Pedido_bs_x_ac
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
	Then Erro "Favor informar o número do pedido Magento(Pedido_bs_x_ac)!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "12345678"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567890"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	Then Sem nenhum erro
	#salvamos várias vezes, por nenhum bom motivo, só para testar mais...
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456799"
	Then Sem nenhum erro
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456799"
	Then Sem nenhum erro

Scenario: InfCriacaoPedido Marketplace_codigo_origem
#/// Número do pedido no marketplace (opcional, se o pedido é do magento este campo não existe)
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	Then Erro "Informe o Marketplace_codigo_origem."

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = ""
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "123"
	Then Erro "Informe o Pedido_bs_x_marketplace."


@ignore
Scenario: EnderecoCadastralCliente
	When Fazer esta validação

Scenario: EnderecoCadastralCliente CPF diferente do principal
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_cnpj_cpf" = "1"
	And Informo "pedidoMagentoDto.Cnpj_Cpf" = "2"
	Then Erro "Cnpj_Cpf está diferente de EnderecoCadastralCliente.Endereco_cnpj_cpf."

@ignore
Scenario: DetalhesPedidoMagentoDto
	#//nao existe o DetalhesPedidoMagentoDto. Os valores a usar são:
	#//St_Entrega_Imediata: se for PF, sim. Se for PJ, não
	#// PrevisaoEntregaData = null
	#// BemDeUso_Consumo = COD_ST_BEM_USO_CONSUMO_SIM
	#//InstaladorInstala = COD_INSTALADOR_INSTALA_NAO
	When Fazer esta validação