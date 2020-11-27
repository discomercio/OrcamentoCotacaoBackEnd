@ignore
@Especificacao.Pedido.Passo30
Feature: Validações do PedidoNovo

#todas as validações que no ASP são feitas na r_cliente passamos a fazer nos campos do endereço de cobrança.
#No ASP, o pedido sempre é criado com os dados da t_cliente, por isso a t_cliente é validada
#Exceto quando o orçamento vira pedido, aí sim são usados os campos de memorização de endereço (endereço de cobrança) do orçamento
#mas este levantamento foi feito a partir da criação do pedido

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo30.PedidoNovo"
	Given Implementado em "Especificacao.Pedido.Pedido"


Scenario: Validar CEP
	#	if Trim("" & r_cliente.cep) <> "" then
	#		if Len(retorna_so_digitos(Trim("" & r_cliente.cep))) < 8 then
	#			alerta=texto_add_br(alerta)
	#			alerta=alerta & "O CEP do cadastro do cliente está incompleto (CEP: " & Trim("" & r_cliente.cep) & ")"
	#			end if
	#		end if
	#
	#	if rb_end_entrega = "S" then
	#		if EndEtg_cep <> "" then
	#			if Len(retorna_so_digitos(EndEtg_cep)) < 8 then
	#				alerta=texto_add_br(alerta)
	#				alerta=alerta & "O CEP do endereço de entrega está incompleto (CEP: " & EndEtg_cep & ")"
	#				end if
	#			end if
	#		end if
	When Pedido base
	And Informo "cep" = "1234567"
	Then Erro "regex O CEP do cadastro do cliente está incompleto.*"
	When Pedido base
	And Informo "cep" = ""
	Then Erro "regex O CEP do cadastro do cliente está incompleto.*"

Scenario: Validar CEP EndEtg_cep
	When Pedido base com endereço de entrega
	And Informo "cep" = "1234567"
	Then Erro "regex O CEP do endereço de entrega está incompleto.*"
	When Pedido base
	And Informo "cep" = ""
	Then Erro "regex O CEP do endereço de entrega está incompleto.*"



Scenario: ddd_res
	#'	DDD VÁLIDO?
	#	if Not ddd_ok(r_cliente.ddd_res) then
	#		if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
	#		alerta = alerta & "DDD do telefone residencial é inválido!!"
	#		end if
	When Pedido base
	And Informo "ddd_res" = "1"
	Then Erro "DDD do telefone residencial é inválido!!"

	When Pedido base
	And Informo "ddd_res" = "123"
	Then Erro "DDD do telefone residencial é inválido!!"

	When Pedido base
	And Informo "ddd_res" = "12"
	Then Sem erro "DDD do telefone residencial é inválido!!"

Scenario: ddd_com
#if Not ddd_ok(r_cliente.ddd_com) then
#	if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
#	alerta = alerta & "DDD do telefone comercial é inválido!!"
#	end if

	When Pedido base
	And Informo "ddd_com" = "1"
	Then Erro "DDD do telefone comercial é inválido!!"

	When Pedido base
	And Informo "ddd_com" = "123"
	Then Erro "DDD do telefone comercial é inválido!!"

	When Pedido base
	And Informo "ddd_com" = "12"
	Then Sem erro "DDD do telefone comercial é inválido!!"

Scenario: I.E. É VÁLIDA?
	#if (r_cliente.contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) then
	#	if Not isInscricaoEstadualValida(r_cliente.ie, r_cliente.uf) then
	#		if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
	#		alerta=alerta & "Corrija a IE (Inscrição Estadual) com um número válido!!" & _
	#				"<br>" & "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE."
	#		end if
	#	end if
	Given Pedido base cliente PF
	When Informo "EnderecoCadastralCliente.ie" = "123"
	When Informo "EnderecoCadastralCliente.icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EnderecoCadastralCliente.produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Erro "regex .*Corrija a IE (Inscrição Estadual) com um número válido.*"

Scenario: Validar IE  erro 2 - com outro estado
	Given Pedido base cliente PF
	When Endereço cadastral do estado "BA"
	When Informo "EnderecoCadastralCliente.ie" = "51.313.629"
	When Informo "EnderecoCadastralCliente.icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EnderecoCadastralCliente.produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Sem erro "regex .*Corrija a IE (Inscrição Estadual) com um número válido.*"

Scenario: Validar IE  erro 2 - com outro estado 2
	Given Pedido base cliente PF
	When Endereço cadastral  do estado "SP"
	When Informo "EnderecoCadastralCliente.ie" = "304480484"
	When Informo "EnderecoCadastralCliente.contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EnderecoCadastralCliente.produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Sem erro "regex .*Corrija a IE (Inscrição Estadual) com um número válido.*"

	Given Pedido base cliente PF
	When Endereço cadastral  do estado "SP"
	#este está errado
	When Informo "EnderecoCadastralCliente.ie" = "30448048499"
	When Informo "EnderecoCadastralCliente.contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EnderecoCadastralCliente.produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Erro "regex .*Corrija a IE (Inscrição Estadual) com um número válido.*"


Scenario: MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
		if Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_sugerida_municipios, msg_erro) then
	When Fazer esta validação


Scenario: MUNICÍPIO DE ENTREGA DE ACORDO C/ TABELA DO IBGE?
		if rb_end_entrega = "S" then
		'	MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
			if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
	When Fazer esta validação

Scenario: 		'VERIFICA SE O MESMO CÓDIGO FOI DIGITADO REPETIDO EM VÁRIAS LINHAS
	#alerta=alerta & "Produto " & vDuplic(i).produto & " do fabricante " & vDuplic(i).fabricante & ": linha " & renumera_com_base1(LBound(vDuplic),i) & " repete o mesmo produto da linha " & renumera_com_base1(LBound(vDuplic),j)
	Given Pedido base
	And Duplicar um produto
	Then Erro "regex *.repete o mesmo produto da linha.*"

Scenario: 		alert("Não há produtos na lista!!");
	Given Pedido base
	And Limpar lista de produtos
	Then Erro "Não há produtos na lista!!"

Scenario: 		Especifique a loja que fez a indicação
	#if (f.vendedor_externo.value=="S") {
	#if (trim(f.loja_indicou.options[f.loja_indicou.selectedIndex].value)=="") {
	#	alert('Especifique a loja que fez a indicação!!');
	#	return;
	#	}
	#}
	Given Pedido base
	When Informo "vendedor_externo" = "S"
	And Informo "loja_indicou" = ""
	Then Erro "Especifique a loja que fez a indicação!!"


Scenario: Selecione o "indicador" - lista completa
#este é o código todo que vamos testar, mais para baixo testamos por partes

#	if (f.c_ExibirCamposComSemIndicacao.value!="S") {
#		blnIndicacaoOk=true;
#	}
#	else if (f.c_MagentoPedidoComIndicador.value=="S") {
#		blnIndicacaoOk=true;
#	}
#	else {
#		idx=-1;
#		blnIndicacaoOk=false;
#		//  Sem Indicação
#		idx++;
#		if (f.rb_indicacao[idx].checked) {
#			blnIndicacaoOk=true;
#		}
#
#		//	Com Indicação
#		idx++;
#		if (f.rb_indicacao[idx].checked) {
#			if (trim(f.c_indicador.value)=='') {
#				alert('Selecione o "indicador"!!');
#				f.c_indicador.focus();
#				return;
#			}
#			if ((!f.rb_RA[0].checked)&&(!f.rb_RA[1].checked)) {
#				alert('Informe se o pedido possui RA ou não!!');
#				return;
#			}
#			blnIndicacaoOk=true;
#			//  O indicador informado agora é diferente do indicador original no cadastro do cliente?
#			if (loja != "<%=NUMERO_LOJA_ECOMMERCE_AR_CLUBE%>") {
#				if (trim(f.c_indicador_original.value)!="") {
#					if (trim(f.c_indicador.value)!=trim(f.c_indicador_original.value)) {
#						s="O indicador selecionado é diferente do indicador que consta no cadastro deste cliente.\n\n##################################################\nFAVOR COMUNICAR AO GERENTE!!\n##################################################\n\nContinua mesmo assim?";
#						if (!confirm(s)) return;
#					}
#				}
#			}
#		}
#
#		if (!blnIndicacaoOk) {
#			alert('Informe se o pedido é com indicação ou não!!');
#			return;
#		}
#	}

Scenario: Selecione o "indicador" 2
#	if (f.c_ExibirCamposComSemIndicacao.value!="S") {
#		blnIndicacaoOk=true;
#	}
#<%	if operacao_permitida(OP_LJA_EXIBIR_CAMPOS_COM_SEM_INDICACAO_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then
#		strAux="S"

	Given Pedido base
	When Usuário sem permissão "OP_LJA_EXIBIR_CAMPOS_COM_SEM_INDICACAO_AO_CADASTRAR_NOVO_PEDIDO"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Outro 205"
	Then Erro "Sem permissão para especificar o indicador"


Scenario: Selecione o "indicador" 3
	#		if (f.rb_indicacao[idx].checked) {
	#			if (trim(f.c_indicador.value)=='') {
	#				alert('Selecione o "indicador"!!');
	#				f.c_indicador.focus();
	#				return;
	#			}
	Given Pedido base
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = ""
	Then Erro "Selecione o "indicador"!!"

Scenario: Selecione o "indicador" 4
	#			if ((!f.rb_RA[0].checked)&&(!f.rb_RA[1].checked)) {
	#				alert('Informe se o pedido possui RA ou não!!');
	#				return;
	#			}
	Given Pedido base
	And Informo "rb_RA" = ""
	Then Erro "Informe se o pedido possui RA ou não!!"


Scenario: Selecione o "indicador" 5
	#		if (!blnIndicacaoOk) {
	#			alert('Informe se o pedido é com indicação ou não!!');
	#			return;
	#		}
	Given Pedido base
	And Informo "rb_indicacao" = "XX"
	Then Erro "Informe se o pedido é com indicação ou não!!"



Scenario: 	Somente na tela: avisar O indicador selecionado é diferente do indicador que consta no cadastro deste cliente
#não fazemos esta validação porque este teste está sendo feito somente no servidor
#quando fizermos o teste da tela, aí sim devemos fazer este teste

Scenario: 	Não foi informada a forma de pagamento!
	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = ""
	Then Erro "Não foi informada a forma de pagamento!"

	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	Then Sem Erro "Não foi informada a forma de pagamento!"

Scenario: 	Não foi informada a quantidade de parcelas da forma de pagamento!
	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "0"
	Then Erro "Não foi informada a quantidade de parcelas da forma de pagamento!"

	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "1"
	Then Sem Erro "Não foi informada a quantidade de parcelas da forma de pagamento!"

	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "0"
	Then Erro "Não foi informada a quantidade de parcelas da forma de pagamento!"

	When Pedido base
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "1"
	Then Sem Erro "Não foi informada a quantidade de parcelas da forma de pagamento!"

Scenario: A forma de pagamento não está disponível para o(s) produto(s)
#strMsgErro="";
#for (i=0; i < f.c_produto.length; i++) {
#	if (trim(f.c_produto[i].value)!="") {
#		if (f.c_preco_lista[i].style.color.toLowerCase()==COR_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__INEXISTENTE.toLowerCase()) {
#			strMsgErro+="\n" + f.c_produto[i].value + " - " + f.c_descricao[i].value;
#			}
#		}
#	}
#if (strMsgErro!="") {
#	strMsgErro="A forma de pagamento " + KEY_ASPAS + f.c_custoFinancFornecParcelamentoDescricao.value.toLowerCase() + KEY_ASPAS + " não está disponível para o(s) produto(s):"+strMsgErro;
#	alert(strMsgErro);
#	return;
#	}

#COR_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__INEXISTENTE é determinado pela página AjaxCustoFinancFornecConsultaPrecoBD.asp
#Condições:
#strSql = _
#	"SELECT " & _
#		"*" & _
#	" FROM t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR" & _
#	" WHERE" & _
#		" (fabricante = '" & vResp(intCounter).fabricante & "')" & _
#		" AND (tipo_parcelamento = '" & strTipoParcelamento & "')" & _
#		" AND (qtde_parcelas = " & strQtdeParcelas & ")"
#
#		e
#						"SELECT " & _
#	"*" & _
#" FROM t_PRODUTO" & _
#	" INNER JOIN t_PRODUTO_LOJA" & _
#		" ON (t_PRODUTO.fabricante=t_PRODUTO_LOJA.fabricante) AND (t_PRODUTO.produto=t_PRODUTO_LOJA.produto)" & _
#" WHERE" & _
#	" (t_PRODUTO.fabricante = '" & vResp(intCounter).fabricante & "')" & _
#	" AND (t_PRODUTO.produto = '" & vResp(intCounter).produto & "')" & _
#	" AND (CONVERT(smallint,loja) = " & strLoja & ")"

Scenario: A forma de pagamento não está disponível para o(s) produto(s) - t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR
	When Pedido base
	#passar o pedido com esse tipo_parcelamento
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "2"
	Then Sem nenhum erro 
	#se passou, vamos estragar para forçar o erro
	Given Reiniciar banco no fim do cenário
	And Pedido base
	And Limpar tabela "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR"
	Then Erro "regex .*não está disponível para o(s) produto(s).*"


Scenario: A forma de pagamento não está disponível para o(s) produto(s) - t_PRODUTO_LOJA
	When Pedido base
	#passar o pedido com esse tipo_parcelamento
	And Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "2"
	Then Sem nenhum erro 
	#se passou, vamos estragar para forçar o erro
	Given Reiniciar banco no fim do cenário
	And Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	Then Erro "regex .*não está disponível para o(s) produto(s).*"


