@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Descontinuado.feature

Background:
	Given Reiniciar banco ao terminar cenário

Scenario: Descontinuado
	#loja/PedidoNovoConsiste.asp
	#						alerta=alerta & "Produto (" & v_item(i).fabricante & ")" & v_item(i).produto & " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada."
	Given Tabela "t_PRODUTO" com fabricante = "003" e produto = "003221" alterar campo "descontinuado" = "S"
	Given Zerar todo o estoque
	Given Pedido base
	Then Erro "regex .*consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada.*"

