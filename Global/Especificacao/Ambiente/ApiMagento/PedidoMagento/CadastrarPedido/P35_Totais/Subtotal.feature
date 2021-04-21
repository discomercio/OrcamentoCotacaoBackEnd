@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: Subtotal
	Subtotal: Somar o campo Subtotal da lista de produtos (incluindo os serviços)
	PedidoTotaisMagentoDto.Subtotal = 
		soma de PedidoProdutoMagentoDto.Subtotal
		+ soma de PedidoServicoMagentoDto.Subtotal

Scenario: Subtotal - sucesso
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1300.00"
	Then Sem nenhum erro

Scenario: Subtotal - erro
	Given Pedido base
	When Lista de itens com "1" itens
	And Lista de itens "0" informo "Quantidade" = "2"
	And Lista de itens "0" informo "Subtotal" = "1300.00"
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "1100.00"
	Then Erro "ajustar msg"

Scenario: Subtotal - vazio
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.Subtotal" = ""
	Then Erro "ajustar msg"

Scenario: Subtotal - zerado
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.Subtotal" = "0"
	Then Erro "ajustar msg"