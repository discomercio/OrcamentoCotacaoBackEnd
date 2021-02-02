@ignore
@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Gerar_o_numero_do_pedido

Background: Background
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	When Lista de itens "0" informo "Qtde" = "20"
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	And Zerar todo o estoque

Scenario: Gerar_o_numero_do_pedido
	#Gerar o número do pedido: caso maior que 1, colocar letras como sufixo
	#		'	Controla a quantidade de pedidos no auto-split
	#		'	pedido-base: indice_pedido=1
	#		'	pedido-filhote 'A' => indice_pedido=2
	#		'	pedido-filhote 'B' => indice_pedido=3
	#		'	etc
	Given Pedido base
	And Definir estoque id_nfe_emitente = "4003", saldo de estoque = "5"
	And Definir estoque id_nfe_emitente = "4903", saldo de estoque = "40"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido" = "contém a letra A"
