@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: BSellerInterest
	BSellerInterest: nos casos em que seja diferente de zero, o pedido deve ser processado manualmente.

Scenario: BSellerInterest - sucesso
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.BSellerInterest" = "0"
	Then Sem nenhum erro

Scenario: BSellerInterest - erro
	Given Pedido base
	And Informo "PedidoTotaisMagentoDto.BSellerInterest" = "10.00"
	Then afazer: verificar o que será feito nesse caso para poder validar da forma correta
