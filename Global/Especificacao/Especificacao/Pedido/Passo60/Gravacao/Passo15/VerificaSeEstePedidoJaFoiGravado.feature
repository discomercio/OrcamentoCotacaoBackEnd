@ignore
Feature: VerificaSeEstePedidoJaFoiGravado

Scenario: VerificaSeEstePedidoJaFoiGravado
#loja/PedidoNovoConfirma.asp
#'	VERIFICA SE ESTE PEDIDO JÁ FOI GRAVADO!!
#	dim pedido_a, vjg
#	if Cstr(loja) <> NUMERO_LOJA_OLD03 then
#		s = "SELECT t_PEDIDO.pedido, fabricante, produto, qtde, preco_venda FROM t_PEDIDO INNER JOIN t_PEDIDO_ITEM ON (t_PEDIDO.pedido=t_PEDIDO_ITEM.pedido)" & _
#			" WHERE (id_cliente='" & cliente_selecionado & "') AND (data=" & bd_formata_data(Date) & ")" & _
#			" AND (loja='" & loja & "') AND (vendedor='" & usuario & "')" & _
#			" AND (data >= " & bd_monta_data(Date) & ")" & _
#			" AND (hora >= '" & formata_hora_hhnnss(Now-converte_min_to_dec(10))& "')" & _
#			" AND (st_entrega<>'" & ST_ENTREGA_CANCELADO & "')" & _
#			" ORDER BY t_PEDIDO_ITEM.pedido, sequencia"
#		set rs = cn.execute(s)
#		redim vjg(0)
#		set vjg(ubound(vjg)) = New cl_DUAS_COLUNAS
#		vjg(ubound(vjg)).c1=""
#		pedido_a="--XX--"
#		do while Not rs.EOF 
#			if pedido_a<>Trim("" & rs("pedido")) then
#				pedido_a=Trim("" & rs("pedido"))
#				if vjg(ubound(vjg)).c1 <> "" then 
#					redim preserve vjg(ubound(vjg)+1)
#					set vjg(ubound(vjg)) = New cl_DUAS_COLUNAS
#					vjg(ubound(vjg)).c1=""
#					end if
#				vjg(ubound(vjg)).c2=pedido_a
#				end if
#		
#			vjg(ubound(vjg)).c1=vjg(ubound(vjg)).c1 & Trim("" & rs("fabricante")) & "|" & Trim("" & rs("produto")) & "|" & Trim("" & rs("qtde")) & "|" & formata_moeda(rs("preco_venda")) & "|"
#			rs.MoveNext 
#			Loop
#
#		if rs.State <> 0 then rs.Close
#	
#		s=""
#		for i=Lbound(v_item) to Ubound(v_item)
#			with v_item(i)
#				if .produto<>"" then
#					s=s & .fabricante & "|" & .produto & "|" & Cstr(.qtde) & "|" & formata_moeda(.preco_venda) & "|"
#					end if
#				end with
#			next
#
#		for i=Lbound(vjg) to Ubound(vjg)
#			if s=vjg(i).c1 then
#				alerta="Este pedido já foi gravado com o número " & vjg(i).c2
#				exit for
#				end if
#			next
#		end if
		When Fazer esta validação

