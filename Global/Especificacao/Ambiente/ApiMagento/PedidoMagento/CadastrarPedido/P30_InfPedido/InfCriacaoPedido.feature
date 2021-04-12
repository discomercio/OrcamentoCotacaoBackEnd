@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos
@GerenciamentoBanco
Feature: InfCriacaoPedido

Background: Configuracoes
	Given Reiniciar banco imediatamente

Scenario: InfCriacaoPedido - Pedido_bs_x_ac
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
	Then Erro "Favor informar o número do pedido Magento(Pedido_bs_x_ac)!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "22345678"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "2234567890"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	Then Sem nenhum erro
	#salvamos várias vezes, por nenhum bom motivo, só para testar mais...
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456799"
	Then Sem nenhum erro
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456790"
	Then Sem nenhum erro

Scenario: InfCriacaoPedido - Pedido_bs_x_ac somente digitos
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
	Then Erro "O número Magento deve conter apenas dígitos!"

Scenario: InfCriacaoPedido - Marketplace_codigo_origem
	# Número do pedido no marketplace (opcional, se o pedido é do magento este campo não existe)
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "126"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	Then Erro "Informe o Marketplace_codigo_origem."
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "126"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "123"
	Then Erro "Código Marketplace não encontrado."

Scenario: InfCriacaoPedido - Pedido_bs_x_marketplace e Marketplace_codigo_origem já existem
#2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
#Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
#caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).
#Testado em Especificacao\Pedido\Passo60\Gravacao\Passo15\PedidoMagentoRepetido.feature