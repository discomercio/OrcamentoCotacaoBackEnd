@ignore
Feature: Produtos

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo40.Produtos"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração


Scenario: Sem quantidade zero
	When Lista de itens "0" informo "qtde" = 0
	Then Erro "Item está com quatidade inválida"

Scenario: Sem produtos compostos
	#'	VERIFICA SE É PRODUTO COMPOSTO
	#	strSql = "SELECT " & _
	#				"*" & _
	#			" FROM t_EC_PRODUTO_COMPOSTO t_EC_PC" & _
	#			" WHERE" & _
	#				" (fabricante_composto = '" & s_fabricante & "')" & _
	#				" AND (produto_composto = '" & s_produto & "')"
	When Fazer esta validação

Scenario: Produto disponível para a loja
#strSql = "SELECT " & _
#			"*" & _
#		" FROM t_PRODUTO tP" & _
#			" INNER JOIN t_PRODUTO_LOJA tPL ON (tP.fabricante = tPL.fabricante) AND (tP.produto = tPL.produto)" & _
#		" WHERE" & _
#			" (tP.produto = '" & s_produto & "')" & _
#			" AND (loja = '" & loja & "')"
#if rs.State <> 0 then rs.Close
#rs.Open strSql, cn
#if rs.Eof then
#	alerta=texto_add_br(alerta)
#	alerta=alerta & "Produto '" & s_produto & "' não foi encontrado para a loja " & loja & "!!"
	When Fazer esta validação

Scenario: Máximo de itens por pedido
#alerta=alerta & "O número de itens que está sendo cadastrado (" & CStr(n) & ") excede o máximo permitido por pedido (" & CStr(MAX_ITENS) & ")!!"
	When Fazer esta validação

