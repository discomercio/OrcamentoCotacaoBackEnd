@ListaDependencias
Feature: ListaDependencias

Scenario: Lista de dependências
	Given Nome deste item "Especificacao.Pedido.Pedido.PedidoListaDependencias"
	And Especificado em "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias"
	And Especificado em "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias"
	#And Especificado em "Especificacao.Pedido.Passo10.*"
	#And Especificado em "Especificacao.Pedido.Passo20.*"
	#And Especificado em "Especificacao.Pedido.Passo25.*"
	#And Especificado em "Especificacao.Pedido.Passo30.*"
	#And Especificado em "Especificacao.Pedido.Passo40.*"
	And Especificado em "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias"
	And Implementado em "Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias"
	And Implementado em "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias"
	
	#na loja
	And Implementado em "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedidoListaDependencias"
