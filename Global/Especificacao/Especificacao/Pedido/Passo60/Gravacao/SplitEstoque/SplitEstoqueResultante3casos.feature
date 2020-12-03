@ignore
Feature: SplitEstoqueResultante3casos
Vmaos verificar se está fazendo o slipt corretamente:
- com um pedido totalmente atendido por um CD
- com um pedido totalmente atendido por dois CDs
- com um pedido atendido por dois CDs e ainda com quantidade sem presença no estoque

Background: Background
	Given Pedido base sem itens
	And Usando fabricante = "001", produto = "001000"
	And Zerar todo o estoque

Scenario: Pedido sem slipt
	Given Criar novo item com quantidade = "20"
	And Definir estoque id_nfe_emitente = "4003", saldo de estoque = "40"
	When Cadastrar pedido
	Then Gerado 1 pedido
	And Pedido gerado "0", verificar id_nfe_emitente = "4003", qtde = "20"
	And Pedido gerado "0", verificar st_entrega = "ST_ENTREGA_SEPARAR"
	And Verificar estoque id_nfe_emitente = "4003", saldo de estoque = "20"


Scenario: Pedido com slipt totalmente atendido
	Given Criar novo item com quantidade = "20"
	And Definir estoque id_nfe_emitente = "4003", saldo de estoque = "5"
	And Definir estoque id_nfe_emitente = "4903", saldo de estoque = "40"
	When Cadastrar pedido

	Then Gerado 2 pedido
	And Pedido gerado "0", verificar id_nfe_emitente = "4003", qtde = "5"
	And Pedido gerado "0", verificar st_entrega = "ST_ENTREGA_SEPARAR"
	And Pedido gerado "1", verificar id_nfe_emitente = "4903", qtde = "15"
	And Pedido gerado "1", verificar st_entrega = "ST_ENTREGA_SEPARAR"

	#zerou este estoque
	And Verificar estoque id_nfe_emitente = "4003", saldo de estoque = "0"
	And Verificar estoque id_nfe_emitente = "4003", saldo de ID_ESTOQUE_SEM_PRESENCA = "0"
	# do 4903 usou 20 - 5 = 15, tinha 40, ficou 25
	And Verificar estoque id_nfe_emitente = "4903", saldo de estoque = "25"
	And Verificar estoque id_nfe_emitente = "4903", saldo de ID_ESTOQUE_SEM_PRESENCA = "0"


Scenario: Pedido com slipt faltando produto
	Given Criar novo item com quantidade = "20"
	And Definir estoque id_nfe_emitente = "4003", saldo de estoque = "5"
	And Definir estoque id_nfe_emitente = "4903", saldo de estoque = "7"
	When Cadastrar pedido

	Then Gerado 2 pedido
	#pedido 0 existente = 5, pedido 1 existente = 7, sem presença no estoque = 8
	#tem que ser os 5 atendidos + os 8 sem presença
	And Pedido gerado "0", verificar id_nfe_emitente = "4003", qtde = "13"
	And Pedido gerado "0", verificar st_entrega = "ST_ENTREGA_SPLIT_POSSIVEL"
	And Pedido gerado "1", verificar id_nfe_emitente = "4903", qtde = "7"
	And Pedido gerado "1", verificar st_entrega = "ST_ENTREGA_SEPARAR"

	#zerou este estoque
	And Verificar estoque id_nfe_emitente = "4003", saldo de estoque = "0"
	And Verificar estoque id_nfe_emitente = "4903", saldo de estoque = "0"
	#e no CD 4003 deixou 8 pendentes
	And Verificar estoque id_nfe_emitente = "4003", saldo de ID_ESTOQUE_SEM_PRESENCA = "8"
	And Verificar estoque id_nfe_emitente = "4903", saldo de ID_ESTOQUE_SEM_PRESENCA = "0"


