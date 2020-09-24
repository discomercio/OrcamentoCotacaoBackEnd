@ignore
Feature: Especificacao\Pedido\Passo60\Gravacao\Passo60\Itens\Log

Scenario: Log dos itens
#loja/PedidoNovoConfirma.asp
#'	LOG
#	if s_log_item_autosplit <> "" then s_log_item_autosplit = s_log_item_autosplit & chr(13)
#	s_log_item_autosplit = s_log_item_autosplit & "(" & .fabricante & ")" & .produto & ":" & _
#				" Qtde Solicitada = " & vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada & "," & _
#				" Qtde Sem Presença Autorizada = " & Cstr(qtde_spe) & "," & _
#				" Qtde Estoque Vendido = " & Cstr(qtde_estoque_vendido_aux) & "," & _
#				" Qtde Sem Presença = " & Cstr(qtde_estoque_sem_presenca_aux)

	When Fazer esta validação
