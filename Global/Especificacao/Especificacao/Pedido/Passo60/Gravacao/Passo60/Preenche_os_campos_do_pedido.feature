@ignore
Feature: Preenche_os_campos_do_pedido
#a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
#loja/PedidoNovoConfirma.asp
#de linha 1788
#s = "SELECT * FROM t_PEDIDO WHERE pedido='X'"
#até linha 2057
#rs("id_nfe_emitente") = vEmpresaAutoSplit(iv)
#rs.Update


Scenario: Preenche_os_campos_do_pedido
#Campos que existem em todos os pedidos: pedido, loja, data, hora
#Campos que existem somente no pedido base, não nos filhotes:
#	st_auto_split se tiver filhotes
#	Campos transferidos: de linha 1800 rs("dt_st_pagto") = Date até 1887 rs("perc_limite_RA_sem_desagio") = perc_limite_RA_sem_desagio
#Campos que existem somente nos pedidos filhotes, não no base:
#	linha 1892 rs("st_auto_split") = 1 até 1903 rs("forma_pagto")=""
#Transfere mais campos: linha 1907 até 2055

	When Fazer esta validação
