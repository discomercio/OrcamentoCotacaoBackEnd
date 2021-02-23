@ignore
@GerenciamentoBanco
Feature: 100-Split_com_um_cd
#Vamos verificar se está gerando o pedido corretamente: im único produto com um único CD. Na verdade, nunca vai gerar um filhote, somente um split possível

Background: Background
	Given Reiniciar banco ao terminar cenário
	And Regra de consumo para CD1
	And Zerar o estoque do produto e colocar estoque de outros produtos

Scenario: Pedido totalmente atendido e sobrando
	When Estoque CD1 de 40
	And Criar pedido com 20 itens
	And Gerar 1 pedido pai sem filhotes
	And Pedido pai status = SEP
	And Pedido pai id_nfe_emitente = CD1
	And Pedido pai t_pedido_item quantidade = 40
	And Saldo de estoque na t_ESTOQUE_ITEM no CD1 = (40 - 20)
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 20, estoque = VDO, operacao = VDA


Scenario: Pedido totalmente atendido e zerando estoque
	When Estoque CD1 de 45
	And Criar pedido com 45 itens
	And Gerar 1 pedido pai sem filhotes
	And Pedido pai status = SEP
	And Pedido pai id_nfe_emitente = CD1
	And Pedido pai t_pedido_item quantidade = 45
	And Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 45, estoque = VDO, operacao = VDA


Scenario: Pedido faltando produto
	When Estoque CD1 de 50
	And Criar pedido com 77 itens
	And Gerar 1 pedido pai sem filhotes
	And Pedido pai status = SPL
	And Pedido pai id_nfe_emitente = CD1
	And Pedido pai t_pedido_item quantidade = 77
	And Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 2 registros
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 50, estoque = VDO, operacao = VDA
	And t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 27, estoque = SPE, operacao = VDA

