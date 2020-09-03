@ignore
Feature: Produtos

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo40.Produtos"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração


Scenario: Sem quantidade zero
#loja/PedidoNovoConsiste.asp
#				if .qtde <= 0 then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " é inválida."
	When Lista de itens "0" informo "qtde" = 0
	Then Erro "Item está com quatidade inválida"
	When Lista de itens "0" informo "qtde" = -1
	Then Erro "Item está com quatidade inválida"

Scenario: Sem produtos compostos - t_EC_PRODUTO_COMPOSTO
	#'	VERIFICA SE É PRODUTO COMPOSTO
	#	strSql = "SELECT " & _
	#				"*" & _
	#			" FROM t_EC_PRODUTO_COMPOSTO t_EC_PC" & _
	#			" WHERE" & _
	#				" (fabricante_composto = '" & s_fabricante & "')" & _
	#				" AND (produto_composto = '" & s_produto & "')"
	When Fazer esta validação

Scenario: Sem produtos compostos 2 - t_EC_PRODUTO_COMPOSTO_ITEM
##loja/PedidoNovoConsiste.asp
#			s = "SELECT " & _
#					"*" & _
#				" FROM t_EC_PRODUTO_COMPOSTO_ITEM" & _
#				" WHERE" & _
#					" (fabricante_composto = '" & v_item(i).fabricante & "')" & _
#					" AND (produto_composto = '" & v_item(i).produto & "')" & _
#				" ORDER BY" & _
#					" fabricante_item," & _
#					" produto_item"
#			set rs = cn.execute(s)
#			if Not rs.Eof then
#				alerta=alerta & "O código de produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & " é somente um código auxiliar para agrupar os produtos " & s & " e não pode ser usado diretamente no pedido!!"
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

#também em loja/PedidoNovoConsiste.asp
#					alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está cadastrado."
	When Fazer esta validação

#em loja/PedidoNovoConsiste.asp
#					if Ucase(Trim("" & rs("vendavel"))) <> "S" then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está disponível para venda."
#					elseif .qtde > rs("qtde_max_venda") then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " excede o máximo permitido."
	When Fazer esta validação


Scenario: Máximo de itens por pedido
#alerta=alerta & "O número de itens que está sendo cadastrado (" & CStr(n) & ") excede o máximo permitido por pedido (" & CStr(MAX_ITENS) & ")!!"
	When Fazer esta validação


Scenario: Sem produtos repetidos
#loja/PedidoNovoConsiste.asp
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": linha " & renumera_com_base1(Lbound(v_item),i) & " repete o mesmo produto da linha " & renumera_com_base1(Lbound(v_item),j) & "."
	When Lista de itens copio item "1" para item "0"
	Then Erro regex ".*repete o mesmo produto da linha.*"


