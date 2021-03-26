@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: 100-Split_com_um_cd

#Vamos verificar se está gerando o pedido corretamente: im único produto com um único CD. Na verdade, nunca vai gerar um filhote, somente um split possível
Background: Background
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário

Scenario: Pedido totalmente atendido e sobrando
	#A quantidade utilizada na t_ESTOQUE_ITEM é 40 mesmo, porque rodamos para Magento e Loja
	Given Zerar todo o estoque
	Given Pedido base
	When Lista de itens com "1" itens
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	When Lista de itens "0" informo "Qtde" = "20"
	Given Definir saldo de estoque = "40" para produto "um"
	When Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SEP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "id_nfe_emitente" = "4903"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "20"
	And Tabela "t_ESTOQUE_ITEM" registro pai e produto = "003220", verificar campo "qtde_utilizada" = "40"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220", verificar campo "qtde" = "20"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220", verificar campo "estoque" = "VDO"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220", verificar campo "operacao" = "VDA"

@ignore
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

@ignore
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