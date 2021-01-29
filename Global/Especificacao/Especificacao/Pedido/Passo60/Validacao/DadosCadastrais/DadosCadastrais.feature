Feature: DadosCadastrais
#loja/PedidoNovoConfirma.asp
#Validaçãoo de dados cadastrais


#Os cmapos EndCOb são os que são gavados no pedido, tipo:
#				rs("endereco_uf") = EndCob_uf
#				rs("endereco_cep") = EndCob_cep

#No magento, a validação do endereço é diferente: somente validamos a cidade no IBGE e a UF. 
#Exigimos CEP mas não precisa existir na nossa base.

#'	CONSISTÊNCIAS
#	if alerta = "" then
#		if c_FlagCadSemiAutoPedMagento_FluxoOtimizado = "1" then
#			'CPF/CNPJ
#			if Not cnpj_cpf_ok(EndCob_cnpj_cpf) then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "CPF/CNPJ informado é inválido!"
#				end if
#			'NOME
#			if EndCob_nome = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Nome do cliente não informado!"
#				end if
#			'TIPO DE PESSOA
#			if EndCob_tipo_pessoa = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Tipo de pessoa não informado!"
#				end if
#			'INSCRIÇÃO ESTADUAL
#			if ((EndCob_tipo_pessoa = ID_PJ) And (EndCob_contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) _
#				Or _
#				((EndCob_tipo_pessoa = ID_PJ) And ((EndCob_contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO) And (EndCob_ie <> ""))) _
#				Or _
#				((EndCob_tipo_pessoa = ID_PF) And (EndCob_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)) then
#				if Not isInscricaoEstadualValida(EndCob_ie, EndCob_uf) then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Número de IE (inscrição estadual) informado é inválido (IE: " & EndCob_ie & ", UF: " & EndCob_uf & ")!" & _
#									"<br />" & "Certifique-se de que a UF do endereço corresponde à UF responsável pelo registro da IE."
#					end if
#				end if
#			'ENDEREÇO
#			if EndCob_endereco = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Endereço (logradouro) não informado!"
#				end if
#			if Len(EndCob_endereco) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Endereço (logradouro) excede o tamanho máximo permitido:<br />Tamanho atual: " & Cstr(Len(EndCob_endereco)) & " caracteres<br />Tamanho máximo: " & Cstr(MAX_TAMANHO_CAMPO_ENDERECO) & " caracteres"
#				end if
#			'NÚMERO DO ENDEREÇO
#			if EndCob_endereco_numero = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Número do endereço não informado!"
#				end if
#			'CIDADE
#			if EndCob_cidade = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Cidade do endereço não informada!"
#				end if
#			'UF
#			if EndCob_uf = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "UF do endereço não informada!"
#				end if
#'	CEP
#			if EndCob_cep = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "CEP não informado!"
#				end if
#		else 'bloco else: if c_FlagCadSemiAutoPedMagento_FluxoOtimizado = "1"
#			'CPF/CNPJ
#			if Not cnpj_cpf_ok(EndCob_cnpj_cpf) then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "CPF/CNPJ do cadastro do cliente é inválido!"
#				end if
#			'NOME
#			if EndCob_nome = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher o nome no cadastro do cliente!"
#				end if
#			'TIPO DE PESSOA
#			if EndCob_tipo_pessoa = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher o tipo de pessoa no cadastro do cliente!"
#				end if
#			'INSCRIÇÃO ESTADUAL
#			if ((EndCob_tipo_pessoa = ID_PJ) And (EndCob_contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) _
#				Or _
#				((EndCob_tipo_pessoa = ID_PJ) And ((EndCob_contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO) And (EndCob_ie <> ""))) _
#				Or _
#				((EndCob_tipo_pessoa = ID_PF) And (EndCob_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)) then
#				if Not isInscricaoEstadualValida(EndCob_ie, EndCob_uf) then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Número de IE (inscrição estadual) no cadastro do cliente é inválido (IE: " & EndCob_ie & ", UF: " & EndCob_uf & ")!" & _
#									"<br />" & "Certifique-se de que a UF do endereço corresponde à UF responsável pelo registro da IE."
#					end if
#				end if
#			'ENDEREÇO
#			if EndCob_endereco = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher o endereço (logradouro) no cadastro do cliente!"
#				end if
#			if Len(EndCob_endereco) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Endereço (logradouro) do cadastro do cliente excede o tamanho máximo permitido:<br />Tamanho atual: " & Cstr(Len(EndCob_endereco)) & " caracteres<br />Tamanho máximo: " & Cstr(MAX_TAMANHO_CAMPO_ENDERECO) & " caracteres"
#				end if
#			'NÚMERO DO ENDEREÇO
#			if EndCob_endereco_numero = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher o número do endereço no cadastro do cliente!"
#				end if
#			'CIDADE
#			if EndCob_cidade = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher a cidade no cadastro do cliente!"
#				end if
#			'UF
#			if EndCob_uf = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher a UF no cadastro do cliente!"
#				end if
#			'CEP
#			if EndCob_cep = "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "É necessário preencher o CEP no cadastro do cliente!"
#				end if
#			end if 'if c_FlagCadSemiAutoPedMagento_FluxoOtimizado = "1"
#		end if
@ignore
Scenario: Validar
	Given Fazer a validação acima
