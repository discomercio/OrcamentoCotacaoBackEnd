@Especificacao.Pedido.Passo40
Feature: Produtos


Scenario: Sem quantidade zero
	#loja/PedidoNovoConsiste.asp
	#				if .qtde <= 0 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " é inválida."
	Given Pedido base
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "qtde" = "0"
	Then Erro "regex .* com Qtde menor ou igual a zero!"

Scenario: Sem quantidade zero 2
	Given Pedido base
	When Lista de itens "0" informo "qtde" = "-1"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com Qtde menor ou igual a zero!"

#
#Scenario: Sem produtos compostos - t_EC_PRODUTO_COMPOSTO_ITEM
#
###loja/PedidoNovoConsiste.asp
##			s = "SELECT " & _
##					"*" & _
##				" FROM t_EC_PRODUTO_COMPOSTO_ITEM" & _
##				" WHERE" & _
##					" (fabricante_composto = '" & v_item(i).fabricante & "')" & _
##					" AND (produto_composto = '" & v_item(i).produto & "')" & _
##				" ORDER BY" & _
##					" fabricante_item," & _
##					" produto_item"
##			set rs = cn.execute(s)
##			if Not rs.Eof then
##				alerta=alerta & "O código de produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & " é somente um código auxiliar para agrupar os produtos " & s & " e não pode ser usado diretamente no pedido!!"
Scenario: produtos: sempre virão divididos, nunca vai vir um produto composto.
	#Implementado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\Passo40\Produtos.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001090"
	When Lista de itens "0" informo "Preco_Venda" = "1648.00"
	When Lista de itens "0" informo "Preco_NF" = "1648.00"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "Produto cód.(001090) do fabricante cód.(001) não existe!"

#Scenario: Produto disponível para a loja

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
#loja/PedidoNovoConfirma.asp
#alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está cadastrado para a loja " & loja
#validação feita em
#Especificacao\Pedido\Passo40\PedidoNovoProdCompostoMask_t_PRODUTO_LOJA.feature
#em loja/PedidoNovoConsiste.asp
#					if Ucase(Trim("" & rs("vendavel"))) <> "S" then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está disponível para venda."
#					elseif .qtde > rs("qtde_max_venda") then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " excede o máximo permitido."
# Validação feita em outro arquivo: "Especificacao\Pedido\Passo40\PedidoNovoProdCompostoMask_t_PRODUTO_LOJA.feature"
Scenario: Máximo de itens por pedido
	#alerta=alerta & "O número de itens que está sendo cadastrado (" & CStr(n) & ") excede o máximo permitido por pedido (" & CStr(MAX_ITENS) & ")!!"
	
	#Implementado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\Passo40\Produtos.feature
	#para o magento precisamos mudar a loja do appsettings
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Lista de itens com "13" itens
	#item 1
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "338.85"
	When Lista de itens "0" informo "Preco_NF" = "340.00"
	#item 2
	When Lista de itens "1" informo "Fabricante" = "001"
	When Lista de itens "1" informo "Produto" = "001001"
	When Lista de itens "1" informo "Qtde" = "1"
	When Lista de itens "1" informo "Preco_Venda" = "790.63"
	When Lista de itens "1" informo "Preco_NF" = "792.63"
	#item 3
	When Lista de itens "2" informo "Fabricante" = "001"
	When Lista de itens "2" informo "Produto" = "001002"
	When Lista de itens "2" informo "Qtde" = "1"
	When Lista de itens "2" informo "Preco_Venda" = "370.75"
	When Lista de itens "2" informo "Preco_NF" = "372.75"
	#item 4
	When Lista de itens "3" informo "Fabricante" = "001"
	When Lista de itens "3" informo "Produto" = "001003"
	When Lista de itens "3" informo "Qtde" = "1"
	When Lista de itens "3" informo "Preco_Venda" = "825.22"
	When Lista de itens "3" informo "Preco_NF" = "827.22"
	#item 5
	When Lista de itens "4" informo "Fabricante" = "001"
	When Lista de itens "4" informo "Produto" = "001004"
	When Lista de itens "4" informo "Qtde" = "1"
	When Lista de itens "4" informo "Preco_Venda" = "384.44"
	When Lista de itens "4" informo "Preco_NF" = "386.44"
	#item 6
	When Lista de itens "5" informo "Fabricante" = "001"
	When Lista de itens "5" informo "Produto" = "001005"
	When Lista de itens "5" informo "Qtde" = "1"
	When Lista de itens "5" informo "Preco_Venda" = "897.03"
	When Lista de itens "5" informo "Preco_NF" = "899.03"
	#item 7
	When Lista de itens "6" informo "Fabricante" = "001"
	When Lista de itens "6" informo "Produto" = "001006"
	When Lista de itens "6" informo "Qtde" = "1"
	When Lista de itens "6" informo "Preco_Venda" = "469.91"
	When Lista de itens "6" informo "Preco_NF" = "471.91"
	#item 8
	When Lista de itens "7" informo "Fabricante" = "001"
	When Lista de itens "7" informo "Produto" = "001007"
	When Lista de itens "7" informo "Qtde" = "1"
	When Lista de itens "7" informo "Preco_Venda" = "954.05"
	When Lista de itens "7" informo "Preco_NF" = "956.05"
	#item 9
	When Lista de itens "8" informo "Fabricante" = "001"
	When Lista de itens "8" informo "Produto" = "001008"
	When Lista de itens "8" informo "Qtde" = "1"
	When Lista de itens "8" informo "Preco_Venda" = "595.30"
	When Lista de itens "8" informo "Preco_NF" = "597.30"
	#item 10
	When Lista de itens "9" informo "Fabricante" = "001"
	When Lista de itens "9" informo "Produto" = "001009"
	When Lista de itens "9" informo "Qtde" = "1"
	When Lista de itens "9" informo "Preco_Venda" = "1208.63"
	When Lista de itens "9" informo "Preco_NF" = "1210.63"
	#item 11
	When Lista de itens "12" informo "Fabricante" = "001"
	When Lista de itens "12" informo "Produto" = "001012"
	When Lista de itens "12" informo "Qtde" = "1"
	When Lista de itens "12" informo "Preco_Venda" = "833.54"
	When Lista de itens "12" informo "Preco_NF" = "835.54"
	#item 12
	When Lista de itens "11" informo "Fabricante" = "001"
	When Lista de itens "11" informo "Produto" = "001013"
	When Lista de itens "11" informo "Qtde" = "1"
	When Lista de itens "11" informo "Preco_Venda" = "1916.50"
	When Lista de itens "11" informo "Preco_NF" = "1918.50"
	#item 13
	When Lista de itens "10" informo "Fabricante" = "002"
	When Lista de itens "10" informo "Produto" = "002000"
	When Lista de itens "10" informo "Qtde" = "1"
	When Lista de itens "10" informo "Preco_Venda" = "581.04"
	When Lista de itens "10" informo "Preco_NF" = "583.04"
	#
	Then Erro "regex .*12 itens*"

@ignore
Scenario: Produto repetidos
	#loja/PedidoNovoConsiste.asp
	#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": linha " & renumera_com_base1(Lbound(v_item),i) & " repete o mesmo produto da linha " & renumera_com_base1(Lbound(v_item),j) & "."
	Given Pedido base
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "1" informo "Fabricante" = "001"
	When Lista de itens "1" informo "Produto" = "001000"
	When Lista de itens "1" informo "Qtde" = "1"	
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .*repete o mesmo produto da linha.*"

Scenario: Sem produtos
	Given Pedido base
	When Lista de itens com "0" itens
	Then Erro "Não há itens na lista de produtos!"

Scenario: Consistência para valor zerado
	#loja/PedidoNovoConsiste.asp
	#// CONSISTÊNCIA PARA VALOR ZERADO
	#   strMsgErro = "";
	#   for (i = 0; i < f.c_produto.length; i++) {
	#       if (trim(f.c_produto[i].value) != "") {
	#           vl_preco_venda = converte_numero(f.c_vl_unitario[i].value);
	#           if (vl_preco_venda <= 0) {
	#               if (strMsgErro != "") strMsgErro += "\n";
	#               strMsgErro += "O produto '" + f.c_descricao[i].value + "' está com valor de venda zerado!";
	#           }
	#           else if ((f.c_permite_RA_status.value == '1') && (f.rb_RA.value == 'S')) {
	#               vl_NF = converte_numero(f.c_vl_NF[i].value);
	#               if (vl_NF <= 0) {
	#                   if (strMsgErro != "") strMsgErro += "\n";
	#                   strMsgErro += "O produto '" + f.c_descricao[i].value + "' está com o preço zerado!";
	#               }
	#           }
	#       }
	#   }
	#loja/PedidoNovoConfirma.asp
	#'	CONSISTÊNCIA PARA VALOR ZERADO
	#	if alerta="" then
	#		for i=Lbound(v_item) to Ubound(v_item)
	#			with v_item(i)
	#				if .preco_venda <= 0 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Produto '" & .produto & "' está com valor de venda zerado!"
	#				elseif ((rb_RA = "S") And (permite_RA_status = 1)) And (.preco_NF <= 0) then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Produto '" & .produto & "' está com preço zerado!"
	#					end if
	#				end with
	#			next
	#		end if
	Given Pedido base
	When Lista de itens "0" informo "preco_venda" = "-1"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com preco_venda menor ou igual a zero!"

Scenario: Consistência para valor zerado 2
	Given Pedido base
	When Lista de itens "0" informo "preco_venda" = "0"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com preco_venda menor ou igual a zero!"

#nao testamos o elseif ((rb_RA = "S") And (permite_RA_status = 1)) And (.preco_NF <= 0) then
#porque sempre testamos, mais abaixo

Scenario: Consistência para valor negativos
	#loja/PedidoNovoConsiste.asp
	#<input name="c_vl_NF" id="c_vl_NF" class="PLLd" style="width:62px;"
	#	onkeypress="if (digitou_enter(true)) fPED.c_vl_unitario[<%=Cstr(i-1)%>].focus(); filtra_moeda_positivo();" onblur="this.value=formata_moeda(this.value); trata_edicao_RA(<%=Cstr(i-1)%>); recalcula_RA(); recalcula_RA_Liquido(); recalcula_parcelas();"
	#Quer dizer, os preços devem ser positivos
	Given Pedido base
	When Lista de itens "0" informo "preco_NF" = "0"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com preco_NF menor ou igual a zero!"
	Given Pedido base
	When Lista de itens "1" informo "preco_NF" = "0"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com preco_NF menor ou igual a zero!"
	Given Pedido base
	When Lista de itens "1" informo "preco_NF" = "-1"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "regex .* com preco_NF menor ou igual a zero!"