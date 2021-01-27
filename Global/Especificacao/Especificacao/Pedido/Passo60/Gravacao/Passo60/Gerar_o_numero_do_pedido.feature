@ignore
Feature: Gerar_o_numero_do_pedido

Background: Background
	Given Reiniciar banco ao terminar cenário
	Given Pedido base sem itens
	And Usando fabricante = "001", produto = "001000"
	And Zerar todo o estoque

Scenario: Gerar_o_numero_do_pedido
	#Gerar o número do pedido: caso maior que 1, colocar letras como sufixo
	#		'	Controla a quantidade de pedidos no auto-split
	#		'	pedido-base: indice_pedido=1
	#		'	pedido-filhote 'A' => indice_pedido=2
	#		'	pedido-filhote 'B' => indice_pedido=3
	#		'	etc
	Given Criar novo item com quantidade = "20"
	And Definir estoque id_nfe_emitente = "4003", saldo de estoque = "5"
	And Definir estoque id_nfe_emitente = "4903", saldo de estoque = "40"
	When Cadastrar pedido
	Then Gerado 2 pedido
	And Pedido gerado "0", verificar pedido = "numero pedido pai retornado"
	And Pedido gerado "1", verificar pedido = "numero pedido pai retornado mais traço A"
	