@ignore
Feature: Endereco
Background: Validações do PedidoNovo

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

