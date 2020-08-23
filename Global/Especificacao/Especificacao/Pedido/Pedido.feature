@ignore
Feature: Pedido

Scenario: Lista de dependências
	Given Nome deste item "Especificacao.Pedido.Pedido"
	And Especificado em "Especificacao.Pedido.Passo10.*"
	And Especificado em "Especificacao.Pedido.Passo20.*"
	And Especificado em "Especificacao.Pedido.Passo25.*"
	And Especificado em "Especificacao.Pedido.Passo30.*"
	And Especificado em "Especificacao.Pedido.Passo40.*"
	And Implementado em "Especificacao.Prepedido.Prepedido"
	And Fim da configuração
