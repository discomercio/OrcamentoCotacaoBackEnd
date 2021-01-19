@ignore
Feature: CamposMagentoNaoAceitos
#sel loja != NUMERO_LOJA_ECOMMERCE_AR_CLUBE then %>
#nao pode ter campos c_numero_mktplace, c_origem_pedido, c_numero_magento
#Pedido_bs_x_ac Pedido_bs_x_marketplace Marketplace_codigo_origem

Scenario: InfCriacaoPedido.Pedido_bs_x_ac
	Given Pedido base
	And Loja diferente de NUMERO_LOJA_ECOMMERCE_AR_CLUBE
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456799"
	Then Erro "Pedido_bs_x_ac somente pode ser informado para a loja NUMERO_LOJA_ECOMMERCE_AR_CLUBE"

Scenario: InfCriacaoPedido.Pedido_bs_x_marketplace
	Given Fazer esta validação

Scenario: InfCriacaoPedido.Marketplace_codigo_origem
	Given Fazer esta validação

