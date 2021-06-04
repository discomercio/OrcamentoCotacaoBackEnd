@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: CamposMagentoNaoAceitos

#se loja != NUMERO_LOJA_ECOMMERCE_AR_CLUBE then %>
#nao pode ter campos c_numero_mktplace, c_origem_pedido, c_numero_magento
#Pedido_bs_x_ac Pedido_bs_x_marketplace Marketplace_codigo_origem
Background:
Para fazer esse teste precisamos incluir os campos de InfCriacaoPedido do magento para o módulo loja
na condição Pedido/Criacao/Passo30/CamposMagentoNaoAceitos.cs se estiver vindo do magento ele não irá fazer
essa validação
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: Configurações da loja 201
	When Fazer esta validação
#Ao invés de loja != 201, verificar t_loja.unidade_negocio != "AC"
#Altear nos testes e no código

Scenario: InfCriacaoPedido.Pedido_bs_x_ac
	Given Pedido base
	When Informo "appsettings.Loja" = "212"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456799"
	Then Erro "O campo Pedido_bs_x_ac não pode ser informado se loja != 201"

Scenario: InfCriacaoPedido.Pedido_bs_x_marketplace
	Given Pedido base
	When Informo "appsettings.Loja" = "212"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "212456799"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "846"
	Then Erro "O campo Pedido_bs_x_marketplace não pode ser informado se loja != 201"

Scenario: InfCriacaoPedido.Marketplace_codigo_origem
	Given Pedido base
	When Informo "appsettings.Loja" = "212"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "987654123"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "946"
	Then Erro "O campo Marketplace_codigo_origem não pode ser informado se loja != 201"