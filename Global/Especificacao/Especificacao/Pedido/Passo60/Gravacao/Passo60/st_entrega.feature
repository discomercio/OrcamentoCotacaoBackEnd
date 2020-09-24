@ignore
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

#Se nenhum produto saiu do estoque (tabela t_ESTOQUE_ITEM)
#	st_entrega = ST_ENTREGA_ESPERAR
#Se algum produto saiu do estoque e nenhum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SEPARAR
#Se algum produto saiu do estoque e algum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SPLIT_POSSIVEL

	When Fazer esta validação
