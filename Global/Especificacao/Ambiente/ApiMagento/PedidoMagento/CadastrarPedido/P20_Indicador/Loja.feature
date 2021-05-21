@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: Loja
	Verifica se a loja do indicador existe, essa informação é lida do appsettings.

Scenario: Loja - "201" (vamos ler do appsettings)
	Given Pedido base
	And Informo "appsettings.Loja" = "201"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "loja" = "201"

Scenario: Loja - "202" (vamos ler do appsettings)
	Given Pedido base
	And Informo "appsettings.Loja" = "202"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456789"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "loja" = "202"
	
Scenario: Loja - diferente de "201"
	Given Pedido base
	And Informo "appsettings.Loja" = "001"
	Then Erro "Loja não existe!"