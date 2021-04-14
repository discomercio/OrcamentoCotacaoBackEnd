@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: DiscountAmount
	DiscountAmount: Somar o campo DiscountAmount da lista de produtos incluindo serviços + o valor de desconto do frete.
	Ex: PedidoProdutoMagentoDto.DiscountAmount + PedidoTotaisMagentoDto.DescontoFrete = PedidoTotaisMagentoDto.DiscountAmount

Scenario: DiscountAmount - sucesso
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	Then Sem nenhum erro

Scenario:DiscountAmount - erro
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "50.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	Then Erro "ajustar msg"

Scenario: DiscountAmount - sucesso com desconto de frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	Then Sem nenhum erro

Scenario: DiscountAmount - erro com desconto de frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "300.00"
	Then Erro "ajusta msg"

#com serviço
Scenario: DiscountAmount - sucesso com serviço
	Given Pedido base
	When Lista de itens com "2" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	#serviço
	And Lista de itens "1" informo "Sku" = "900900"
	And Lista de itens "1" informo "Quantidade" = "2"
	And Lista de itens "1" informo "Subtotal" = "1300.00"
	And Lista de itens "1" informo "TaxAmount" = "1300.00"
	And Lista de itens "1" informo "DiscountAmount" = "100.00"
	And Lista de itens "1" informo "RowTotal" = "1200.00"
	Then Sem nenhum erro

Scenario: DiscountAmount - erro com serviço
	Given Pedido base
	When Lista de itens com "2" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	#serviço
	And Lista de itens "1" informo "Sku" = "900900"
	And Lista de itens "1" informo "Quantidade" = "2"
	And Lista de itens "1" informo "Subtotal" = "1300.00"
	And Lista de itens "1" informo "TaxAmount" = "1300.00"
	And Lista de itens "1" informo "DiscountAmount" = "50.00"
	And Lista de itens "1" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "300.00"
	Then Erro "ajustar msg"

Scenario: DiscountAmount - sucesso com serviço e desconto de frete
	Given Pedido base
	When Lista de itens com "2" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	#serviço
	And Lista de itens "1" informo "Sku" = "900900"
	And Lista de itens "1" informo "Quantidade" = "2"
	And Lista de itens "1" informo "Subtotal" = "1300.00"
	And Lista de itens "1" informo "TaxAmount" = "1300.00"
	And Lista de itens "1" informo "DiscountAmount" = "100.00"
	And Lista de itens "1" informo "RowTotal" = "1200.00"
	#frete
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	Then Sem nenhum erro

Scenario: DiscountAmount - erro com serviço e desconto de frete
	Given Pedido base
	When Lista de itens com "2" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	#serviço
	And Lista de itens "1" informo "Sku" = "900900"
	And Lista de itens "1" informo "Quantidade" = "2"
	And Lista de itens "1" informo "Subtotal" = "1300.00"
	And Lista de itens "1" informo "TaxAmount" = "1300.00"
	And Lista de itens "1" informo "DiscountAmount" = "100.00"
	And Lista de itens "1" informo "RowTotal" = "1200.00"
	#frete
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "300.00"
	Then Erro "ajustar msg"

Scenario: DiscountAmount - sem desconto nos produtos
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "0"
	And Lista de itens "0" informo "RowTotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	Then Erro "ajustar msg"

Scenario: DiscountAmount - sem desconto no total
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "0"
	Then Erro "ajustar msg"