@ignore
Feature: Pedido

Scenario: Lista de dependências
	Given Nome deste item "Especificacao.Prepedido.Prepedido"
	And Especificado em "Especificacao.Pedido.Pedido"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.ListaExecucao"
