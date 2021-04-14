@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: GrandTotal
	GrandTotal: 
	Iremos fazer 2 validações
	1) PedidoTotaisMagentoDto.Subtotal - PedidoTotaisMagentoDto.DiscountAmount + PedidoTotaisMagentoDto.FreteBruto = PedidoTotaisMagentoDto.GrandTotal
	2) Somar o campo PedidoProdutoMagentoDto.RowTotal da lista de produtos incluindo serviços + PedidoTotaisMagentoDto.FreteBruto = PedidoTotaisMagentoDto.GrandTotal

Scenario: GrandTotal - sucesso
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "0"
	And Lista de itens "0" informo "RowTotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "0"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "0"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1300.00"
	Then Sem nenhum erro

Scenario: GrandTotal - erro
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "0"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1300.00"
	Then Erro "ajustar msg"

Scenario: GrandTotal - Sucesso com Frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "0"
	And Lista de itens "0" informo "RowTotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "0"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1400.00"
	Then Sem nenhum erro

Scenario: GrandTotal - erro com Frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "0"
	And Lista de itens "0" informo "RowTotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "0"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1300.00"
	Then Erro "ajustar msg"

Scenario: GrandTotal - sucesso com desconto
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "0"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1200.00"
	Then Sem nenhum erro

Scenario: GrandTotal - erro com desconto
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "100.00"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1500.00"
	Then Erro "ajustar msg"

Scenario: GrandTotal - sucesso com desconto e frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "200.00"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1400.00"
	Then Sem nenhum erro

Scenario: GrandTotal - erro com desconto e frete
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Lista de itens "0" informo "DiscountAmount" = "100.00"
	And Lista de itens "0" informo "RowTotal" = "1200.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.DiscountAmount" = "100.00"
	And Informo "PedidoTotaisMagentoDto.FreteBruto" = "200.00"
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "1600.00"
	Then Erro "ajustar msg"

Scenario: GrandTotal - vazio
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = ""
	Then Erro "ajustar msg"

Scenario: GrandTotal - zerado
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.GrandTotal" = "0"
	Then Erro "ajustar msg"