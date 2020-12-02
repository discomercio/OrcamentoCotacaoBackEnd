@ignore
Feature: ValidacaoEstoque.feature

Scenario: Validar estoque
#loja/PedidoNovoConsiste.asp
#loja/PedidoNovoConfirma.asp
#exatamente o mesmo código nas duas (só tem uma mensagem comentada de diferença)

#	if alerta="" then
#		'PREPARA O VETOR PARA RECUPERAR AS REGRAS DE CONSUMO DO ESTOQUE ASSOCIADAS AOS PRODUTOS
#		redim vProdRegra(0)
#		inicializa_cl_PEDIDO_SELECAO_PRODUTO_REGRA vProdRegra(UBound(vProdRegra))
#		for i=LBound(v_item) to UBound(v_item)
#			if vProdRegra(UBound(vProdRegra)).produto <> "" then
#				redim preserve vProdRegra(UBound(vProdRegra)+1)
#				inicializa_cl_PEDIDO_SELECAO_PRODUTO_REGRA vProdRegra(UBound(vProdRegra))
#				end if
#			vProdRegra(UBound(vProdRegra)).fabricante = v_item(i).fabricante
#			vProdRegra(UBound(vProdRegra)).produto =v_item(i).produto
#			next
#
#		'RECUPERA AS REGRAS DE CONSUMO DO ESTOQUE ASSOCIADAS AOS PRODUTOS
#		if Not obtemCtrlEstoqueProdutoRegra(r_cliente.uf, r_cliente.tipo, r_cliente.contribuinte_icms_status, r_cliente.produtor_rural_status, vProdRegra, msg_erro) then
#			alerta = "Falha ao tentar obter a(s) regra(s) de consumo do estoque"
#			if msg_erro <> "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & msg_erro
#				end if
#			end if
#		end if 'if alerta=""
#
################### 'até aqui foi testado em obtemCtrlEstoqueProdutoRegra.feature
#
#	if alerta="" then
#		'VERIFICA SE HOUVE ERRO NA LEITURA DAS REGRAS DE CONSUMO DO ESTOQUE ASSOCIADAS AOS PRODUTOS
#		for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
#			if Trim(vProdRegra(iRegra).produto) <> "" then
#				if Not vProdRegra(iRegra).st_regra_ok then
#					if Trim(vProdRegra(iRegra).msg_erro) <> "" then
#						alerta=texto_add_br(alerta)
#						alerta=alerta & vProdRegra(iRegra).msg_erro
#					else
#						alerta=texto_add_br(alerta)
#						alerta=alerta & "Falha desconhecida na leitura da regra de consumo do estoque para o produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " (UF: '" & r_cliente.uf & "', tipo de pessoa: '" & descricao_tipo_pessoa & "')"
################### 'testado nos cenários: Scenario: Validar estoque - erro leitura - XX
#						end if
#					end if
#				end if
#			next
#		end if 'if alerta=""
#
#	if alerta="" then
#		'VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK
#		for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
#			if Trim(vProdRegra(iRegra).produto) <> "" then
#				if converte_numero(vProdRegra(iRegra).regra.id) = 0 then
#					alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - não possui regra de consumo do estoque associada
#					alerta=alerta & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não possui regra de consumo do estoque associada"
#				elseif vProdRegra(iRegra).regra.st_inativo = 1 then
#					alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - Regra de consumo do estoque  - está desativada
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está desativada"
#				elseif vProdRegra(iRegra).regra.regraUF.st_inativo = 1 then
#					alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - Regra de consumo do estoque  - está bloqueada para a UF 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para a UF '" & r_cliente.uf & "'"
#				elseif vProdRegra(iRegra).regra.regraUF.regraPessoa.st_inativo = 1 then
#					alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - Regra de consumo do estoque  - está bloqueada para clientes 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
#				elseif converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.spe_id_nfe_emitente) = 0 then
#					alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - Regra de consumo do estoque  - não especifica nenhum CD para aguardar produtos sem presença no estoque 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
#				else
#					qtde_CD_ativo = 0
#					for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
#						if converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente) > 0 then
#							if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0 then
#								qtde_CD_ativo = qtde_CD_ativo + 1
#								end if
#							end if
#						next
#					'A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
#					if (qtde_CD_ativo = 0) And (id_nfe_emitente_selecao_manual = 0) then
#						alerta=texto_add_br(alerta)
################### 'testado no cenário: Scenario: Validar estoque - Regra de consumo do estoque  - não especifica nenhum CD ativo para clientes 
#						alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD ativo para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
#						end if
#					end if
#				end if
#			next
#		end if 'if alerta=""
#
#	'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS
#	if alerta="" then
#		if id_nfe_emitente_selecao_manual <> 0 then
#			alerta_aux = ""
#			alerta_informativo_aux = ""
#			for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
#				blnAchou = False
#				blnDesativado = False
#				if Trim(vProdRegra(iRegra).produto) <> "" then
#					for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
#						if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual then
#							blnAchou = True
#							if vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 1 then blnDesativado = True
#							exit for
#							end if
#						next
#					end if
#
#				if Not blnAchou then
#					alerta_aux=texto_add_br(alerta_aux)
#					alerta_aux=alerta_aux & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & ": regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") não permite o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "'"
#				elseif blnDesativado then
#					'16/09/2017: FOI REALIZADA UMA ALTERAÇÃO P/ QUE A SELEÇÃO MANUAL DE CD PERMITA O USO DE CD DESATIVADO
################### 'não é testado porque é somente uma informação, mas é salvo da mesma forma
#					alerta_informativo_aux = "Regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") define o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "' como 'desativado'"
#					if Instr(alerta_informativo, alerta_informativo_aux) = 0 then
#						alerta_informativo=texto_add_br(alerta_informativo)
#						alerta_informativo=alerta_informativo & alerta_informativo_aux
#						end if
#					end if
#				next
#
#			if alerta_aux <> "" then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:"
#				alerta=texto_add_br(alerta)
#				alerta=alerta & alerta_aux
#				end if
#			end if
#		end if

Scenario: Validar estoque - erro leitura - 1
#Erros possíveis:
#msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "': não foi possível determinar o tipo de pessoa (tipo_cliente=" & tipo_cliente & ", contribuinte_icms_status=" & contribuinte_icms_status & ", produtor_rural_status=" & produtor_rural_status & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não possui regra associada"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não está associado a nenhuma regra"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não foi localizada no banco de dados (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não está cadastrada para a UF '" & uf & "' (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não está cadastrada para '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "' (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " não especifica nenhum CD para consumo do estoque (Id=" & id_wms_regra_cd & ")"
#vProdRegra(iProd).msg_erro = "Falha na leitura da regra de consumo do estoque para a UF '" & uf & "' e '" & descricao_multi_CD_regra_tipo_pessoa(tipo_pessoa) & "': regra associada ao produto (" & vProdRegra(iProd).fabricante & ")" & vProdRegra(iProd).produto & " especifica o CD '" & obtem_apelido_empresa_NFe_emitente(vProdRegra(iProd).regra.regraUF.regraPessoa.spe_id_nfe_emitente) & "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para processar produtos disponíveis (Id=" & id_wms_regra_cd & ")"

	When Afazer esta validação

Scenario: Validar estoque - não possui regra de consumo do estoque associada
#					alerta=alerta & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não possui regra de consumo do estoque associada"
	When Afazer esta validação

Scenario: Validar estoque - Regra de consumo do estoque  - está desativada
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está desativada"
	When Afazer esta validação

	Scenario: Validar estoque - Regra de consumo do estoque  - está bloqueada para a UF 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para a UF '" & r_cliente.uf & "'"
	When Afazer esta validação

	Scenario: Validar estoque - Regra de consumo do estoque  - está bloqueada para clientes 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	When Afazer esta validação

	Scenario: Validar estoque - Regra de consumo do estoque  - não especifica nenhum CD para aguardar produtos sem presença no estoque 
#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	When Afazer esta validação

	Scenario: Validar estoque - Regra de consumo do estoque  - não especifica nenhum CD ativo para clientes 
#						alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD ativo para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	When Afazer esta validação


