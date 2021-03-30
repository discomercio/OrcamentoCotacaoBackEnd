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
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "qtde" = "20"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "estoque" = "VDO"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "operacao" = "VDA"

Scenario: Pedido totalmente atendido e zerando estoque
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Zerar todo o estoque
	Given Pedido base
	When Lista de itens com "1" itens
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	When Lista de itens "0" informo "Qtde" = "50"
	Given Definir saldo de estoque = "90" para produto "um"
	When Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SEP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "id_nfe_emitente" = "4903"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "50"
	And Tabela "t_ESTOQUE_ITEM" registro pai e produto = "003220", verificar campo "qtde_utilizada" = "50"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "qtde" = "50"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "estoque" = "VDO"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "operacao" = "VDA"

Scenario: Pedido faltando produto
	#Vamos ignorar na Loja para que podemos gerar um split possível
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Zerar todo o estoque
	Given Pedido base
	When Lista de itens com "1" itens
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	When Lista de itens "0" informo "Qtde" = "77"
	Given Definir saldo de estoque = "50" para produto "um"
	When Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SPL"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "id_nfe_emitente" = "4903"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "77"
	And Tabela "t_ESTOQUE_ITEM" registro pai e produto = "003220", verificar campo "qtde_utilizada" = "50"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "qtde" = "50"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "estoque" = "VDO"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "VDO", verificar campo "operacao" = "VDA"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "SPE", verificar campo "qtde" = "27"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "SPE", verificar campo "estoque" = "SPE"
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "003220" e estoque = "SPE", verificar campo "operacao" = "VDA"