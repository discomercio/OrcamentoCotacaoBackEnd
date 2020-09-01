@ListaDependencias
Feature: ListaDependencias

Scenario: Lista de dependências
	Given Nome deste item "Especificacao.Pedido.Pedido.PedidoListaDependencias"
	#And Especificado em "Especificacao.Pedido.Passo10.*"
	#And Especificado em "Especificacao.Pedido.Passo20.*"
	#And Especificado em "Especificacao.Pedido.Passo25.*"
	#And Especificado em "Especificacao.Pedido.Passo30.*"
	#And Especificado em "Especificacao.Pedido.Passo40.*"
	And Especificado em "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias"
	And Implementado em "Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias"
	And Fim da configuração
