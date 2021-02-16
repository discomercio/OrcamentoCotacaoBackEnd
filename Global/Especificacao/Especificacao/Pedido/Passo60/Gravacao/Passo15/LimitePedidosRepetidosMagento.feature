@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: LimitePedidoRepetidosMagento

Background:
	Given Reiniciar appsettings

Scenario: LimitePedidosRepetidos iguais Api Magento - sucesso
	When Informo "limitePedidos.pedidoIgual" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro

@ignore
Scenario: LimitePedidosRepetidos iguais Api Magento - erro
	#falta implementar a validação
	When Informo "limitePedidos.pedidoIgual" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido igual"

Scenario: LimitePedidosRepetidos por cpf Api Magento - sucesso
	When Informo "limitePedidos.porCpf" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro

@ignore
Scenario: LimitePedidosRepetidos por cpf Api Magento - erro
	#falta implementar a validação
	When Informo "limitePedidos.porCpf" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido por cpf"