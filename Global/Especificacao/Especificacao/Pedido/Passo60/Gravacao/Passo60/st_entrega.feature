@ignore
@Especificacao.Pedido.Passo60
Feature: st_entrega

Scenario: st_entrega
	#loja/PedidoNovoConfirma.asp
	#'		STATUS DE ENTREGA
	#		if total_estoque_vendido = 0 then
	#			s = ST_ENTREGA_ESPERAR
	#		elseif total_estoque_sem_presenca = 0 then
	#			s = ST_ENTREGA_SEPARAR
	#		else
	#			s = ST_ENTREGA_SPLIT_POSSIVEL
	#			end if
	#Se algum produto saiu do estoque e algum produto foi vendido sem presença no estoque
	#	st_entrega = ST_ENTREGA_SPLIT_POSSIVEL
	When Fazer esta validação

#Se algum produto saiu do estoque e nenhum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SEPARAR
Scenario: st_entrega - separar
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "st_entrega" = "SEP"

#Se nenhum produto saiu do estoque (tabela t_ESTOQUE_ITEM)
#	st_entrega = ST_ENTREGA_ESPERAR
Scenario: st_entrega - esperar
	Given Pedido base
	And Alterar a qtde de estoque vendido para ser 0
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "st_entrega" = "ESP"

#Se algum produto saiu do estoque e algum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SPLIT_POSSIVEL
Scenario: st_entregar - split possivel
	#criamos um pedido com split possivel no verdinho com esses dados
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	When Informo "ValorTotalDestePedidoComRA" = "173512.00"
	When Informo "VlTotalDestePedido" = ""
	When Informo "FormaPagtoCriacao.C_pc_valor" = "173512.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "st_entrega" = "SPL"