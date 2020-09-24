@ignore
Feature: Validações do PedidoNovo

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo30.PedidoNovo"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração


Scenario: Validar CEP
	if Trim("" & r_cliente.cep) <> "" then
		if Len(retorna_so_digitos(Trim("" & r_cliente.cep))) < 8 then
			alerta=texto_add_br(alerta)
			alerta=alerta & "O CEP do cadastro do cliente está incompleto (CEP: " & Trim("" & r_cliente.cep) & ")"
			end if
		end if

	if rb_end_entrega = "S" then
		if EndEtg_cep <> "" then
			if Len(retorna_so_digitos(EndEtg_cep)) < 8 then
				alerta=texto_add_br(alerta)
				alerta=alerta & "O CEP do endereço de entrega está incompleto (CEP: " & EndEtg_cep & ")"
				end if
			end if
		end if
Given fazer esta validação


Scenario: ddd_res
	'	DDD VÁLIDO?
		if Not ddd_ok(r_cliente.ddd_res) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone residencial é inválido!!"
			end if
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
		if Not ddd_ok(r_cliente.ddd_com) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone comercial é inválido!!"
			end if
			
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
		if (r_cliente.contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) then
			if Not isInscricaoEstadualValida(r_cliente.ie, r_cliente.uf) then
				if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
				alerta=alerta & "Corrija a IE (Inscrição Estadual) com um número válido!!" & _
						"<br>" & "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE."
				end if
			end if
	When Fazer esta validação

Scenario: MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
		if Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_sugerida_municipios, msg_erro) then
	When Fazer esta validação


Scenario: MUNICÍPIO DE ENTREGA DE ACORDO C/ TABELA DO IBGE?
		if rb_end_entrega = "S" then
		'	MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
			if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
	When Fazer esta validação

Scenario: 		'VERIFICA SE O MESMO CÓDIGO FOI DIGITADO REPETIDO EM VÁRIAS LINHAS
alerta=alerta & "Produto " & vDuplic(i).produto & " do fabricante " & vDuplic(i).fabricante & ": linha " & renumera_com_base1(LBound(vDuplic),i) & " repete o mesmo produto da linha " & renumera_com_base1(LBound(vDuplic),j)
	When Fazer esta validação

Scenario: 		alert("Não há produtos na lista!!");
	When Fazer esta validação

Scenario: 		Especifique a loja que fez a indicação
		#if (f.vendedor_externo.value=="S") {
		#if (trim(f.loja_indicou.options[f.loja_indicou.selectedIndex].value)=="") {
		#	alert('Especifique a loja que fez a indicação!!');
		#	return;
		#	}
		#}
	When Fazer esta validação

Scenario: 		Selecione o "indicador"
	When Fazer esta validação
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


Scenario: 	Somente na tela: avisar O indicador selecionado é diferente do indicador que consta no cadastro deste cliente
	When Fazer esta validação

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
	#if (strMsgErro!="") {
	#	strMsgErro="A forma de pagamento " + KEY_ASPAS + f.c_custoFinancFornecParcelamentoDescricao.value.toLowerCase() + KEY_ASPAS + " não está disponível para o(s) produto(s):"+strMsgErro;
	#	alert(strMsgErro);
	#	return;
	#	}
	When Fazer esta validação
