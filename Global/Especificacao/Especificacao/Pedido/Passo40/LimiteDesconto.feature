@ignore
Feature: LimiteDesconto

Scenario: Percentual de comissão excede o máximo permitido!!
	#loja/PedidoNovoConsiste.asp
	#if (perc_RT != 0) {
	#	// RT excede limite máximo?
	#	if (perc_RT > perc_max_RT) {
	#		alert("Percentual de comissão excede o máximo permitido!!");
	#		return;
	#	}
	#perc_max_RT = obtem_perc_max_comissao_e_desconto_por_loja(byval loja)
	#	s = "SELECT" perc_max_comissao,
	#	" FROM t_LOJA" & _
	#	" WHERE" & _
	#		" (CONVERT(smallint,loja) = " & loja & ")"
	Given Pedido base
	When Fazer esta validação

Scenario: Verifica se todos os produtos cujo desconto excedem o máximo permitido possuem senha de desconto disponível
	#loja/PedidoNovoConsiste.asp
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
	Given Pedido base
	When Fazer esta validação
