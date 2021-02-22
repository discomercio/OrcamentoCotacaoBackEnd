@ignore
@Especificacao.Pedido.Passo40
@GerenciamentoBanco
Feature: LimiteDesconto_t_DESCONTO

Background: Configurar descontos da loja
	Given Pedido base
	When Lista de itens "0" informo "Fabricante" = "003"
	When Lista de itens "0" informo "Produto" = "003220"
	When Lista de itens "0" informo "Desc_Dado" = "20"
	#
	Given Reiniciar banco ao terminar cenário
	#todo mundo com 10% de máximo
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao" = "10"
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao_e_desconto_nivel2" = "10"
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao_e_desconto_nivel2_pj" = "10"
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao_e_desconto_pj" = "10"
	#desabilita os meios preferenciais
	And Tabela "t_PARAMETRO" com id = "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" alterar campo "campo_texto" = ""
	# e tira os descontos
	And Limpar tabela "t_DESCONTO"
	# e prepara o desconto que vamos usar para testar
	And Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "003220"
	And Novo registro em "t_DESCONTO", campo "loja" = "202"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	#e prepara o pedido
	Given Pedido base
	When Lista de itens "0" informo "Fabricante" = "003"
	When Lista de itens "0" informo "Produto" = "003220"
	When Lista de itens "0" informo "Desc_Dado" = "20"

#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
Scenario: Verifica se todos os produtos cujo desconto excedem o máximo permitido possuem senha de desconto disponível

#loja/PedidoNovoConsiste.asp
#usa a consulta em JsonConsultaSenhaDescontoBD.asp
#	// Verifica se todos os produtos cujo desconto excedem o máximo permitido possuem senha de desconto disponível
#	// Laço p/ produtos
#	strMsgErro = "";
#	for (i = 0; i < f.c_produto.length; i++) {
#		if (trim(f.c_produto[i].value) != "") {
#			perc_senha_desconto = 0;
#			vl_preco_lista = converte_numero(f.c_preco_lista[i].value);
#			vl_preco_venda = converte_numero(f.c_vl_unitario[i].value);
#			if (vl_preco_lista == 0) {
#				perc_desc = 0;
#			}
#			else {
#				perc_desc = 100 * (vl_preco_lista - vl_preco_venda) / vl_preco_lista;
#			}
#
#			// Tem desconto: sim
#			if (perc_desc != 0) {
#				// Desconto excede limite máximo: sim
#				if (perc_desc > perc_max_comissao_e_desconto_a_utilizar) {
#					// Tem senha de desconto?
#					if (objSenhaDesconto == null) {
#						executa_consulta_senha_desconto(f.cliente_selecionado.value, f.c_loja.value);
#					}
#					for (j = 0; j < objSenhaDesconto.item.length; j++) {
#						if ((objSenhaDesconto.item[j].fabricante == f.c_fabricante[i].value) && (objSenhaDesconto.item[j].produto == f.c_produto[i].value)) {
#							perc_senha_desconto = converte_numero(objSenhaDesconto.item[j].desc_max);
#							break;
#						}
#					}
#					// Tem senha de desconto: sim
#					if (perc_senha_desconto != 0) {
#						// Senha de desconto NÃO cobre desconto
#						if (perc_senha_desconto < perc_desc) {
#							if (strMsgErro != "") strMsgErro += "\n";
#							strMsgErro += "O desconto do produto '" + f.c_descricao[i].value + "' (" + formata_numero(perc_desc, 2) + "%) excede o máximo autorizado!!";
#						}
#					}
#					// Não tem senha de desconto
#					else {
#						if (strMsgErro != "") strMsgErro += "\n";
#						strMsgErro += "O desconto do produto '" + f.c_descricao[i].value + "' (" + formata_numero(perc_desc, 2) + "%) excede o máximo permitido!!";
#					}
#				} // if (perc_desc > perc_max_comissao_e_desconto_a_utilizar)
#			} // if (perc_desc != 0)
#		} // if (trim(f.c_produto[i].value) != "")
#	} // for (laço produtos)
#loja/PedidoNovoConfirma.asp
#if .preco_lista = 0 then
#	.desc_dado = 0
#	desc_dado_arredondado = 0
#else
#	.desc_dado = 100*(.preco_lista-.preco_venda)/.preco_lista
#	desc_dado_arredondado = converte_numero(formata_perc_desc(.desc_dado))
#	end if
#if desc_dado_arredondado > perc_comissao_e_desconto_a_utilizar then
#	if rs.State <> 0 then rs.Close
#	s = "SELECT " & _
#			"*" & _
#		" FROM t_DESCONTO" & _
#		" WHERE" & _
#			" (usado_status=0)" & _
#			" AND (cancelado_status=0)" & _
#			" AND (id_cliente='" & cliente_selecionado & "')" & _
#			" AND (fabricante='" & .fabricante & "')" & _
#			" AND (produto='" & .produto & "')" & _
#			" AND (loja='" & loja & "')" & _
#			" AND (data >= " & bd_formata_data_hora(Now-converte_min_to_dec(TIMEOUT_DESCONTO_EM_MIN)) & ")" & _
#		" ORDER BY" & _
#			" data DESC"
#	set rs=cn.execute(s)
#	if rs.Eof then
#		alerta=texto_add_br(alerta)
#		alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": desconto de " & formata_perc_desc(.desc_dado) & "% excede o máximo permitido."
#	else
#		if .desc_dado > rs("desc_max") then
#			alerta=texto_add_br(alerta)
#			alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": desconto de " & formata_perc_desc(.desc_dado) & "% excede o máximo autorizado."
Scenario: Sem t_DESCONTO
	Given Limpar tabela "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO certo
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	#And Gravar registro em "t_DESCONTO"
	Then Sem nenhum erro

Scenario: Com t_DESCONTO usado_status errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "003220"
	And Novo registro em "t_DESCONTO", campo "loja" = "202"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO cancelado_status errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "003220"
	And Novo registro em "t_DESCONTO", campo "loja" = "202"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO fabricante errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "004"
	And Novo registro em "t_DESCONTO", campo "produto" = "003220"
	And Novo registro em "t_DESCONTO", campo "loja" = "202"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO produto errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "903220"
	And Novo registro em "t_DESCONTO", campo "loja" = "202"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO loja errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "903220"
	And Novo registro em "t_DESCONTO", campo "loja" = "987"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"

Scenario: Com t_DESCONTO data errado
	#Given Modificar item do pedido, fabricante "003", produto "003220", colocar "20" por cento de desconto
	#Const TIMEOUT_DESCONTO_EM_MIN = 30
	Given Limpar tabela "t_DESCONTO"
	Given Novo registro na tabela "t_DESCONTO"
	And Novo registro em "t_DESCONTO", campo "usado_status" = "0"
	And Novo registro em "t_DESCONTO", campo "cancelado_status" = "1"
	And Novo registro em "t_DESCONTO", campo "fabricante" = "003"
	And Novo registro em "t_DESCONTO", campo "produto" = "903220"
	And Novo registro em "t_DESCONTO", campo "loja" = "987"
	And Novo registro em "t_DESCONTO", campo "data" = "especial: data atual menos 1 hora"
	And Gravar registro em "t_DESCONTO"
	Then Erro "desconto excedido"