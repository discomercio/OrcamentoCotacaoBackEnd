@ignore
Feature: Estoque

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
	#					alerta=alerta & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não possui regra de consumo do estoque associada"
	#				elseif vProdRegra(iRegra).regra.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está desativada"
	#				elseif vProdRegra(iRegra).regra.regraUF.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para a UF '" & r_cliente.uf & "'"
	#				elseif vProdRegra(iRegra).regra.regraUF.regraPessoa.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	#				elseif converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.spe_id_nfe_emitente) = 0 then
	#					alerta=texto_add_br(alerta)
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
	When Fazer esta validação

Scenario: Validar estoque 2
	#loja/PedidoNovoConsiste.asp
	#loja/PedidoNovoConfirma.asp
	#exatamente o mesmo código nas duas 

	#		if alerta="" then
	#		'OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE
	#		for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
	#			if Trim(vProdRegra(iRegra).produto) <> "" then
	#				for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
	#					if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
	#						( (id_nfe_emitente_selecao_manual = 0) Or (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
	#						'VERIFICA SE O CD ESTÁ HABILITADO
	#						'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
	#						if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0) Or (id_nfe_emitente_selecao_manual <> 0) then
	#							idxItem = Lbound(v_item) - 1
	#							for iItem=Lbound(v_item) to Ubound(v_item)
	#								if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) then
	#									idxItem = iItem
	#									exit for
	#									end if
	#								next
	#							if idxItem < Lbound(v_item) then
	#								alerta=texto_add_br(alerta)
	#								alerta=alerta & "Falha ao localizar o produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " na lista de produtos a ser processada"
	#							else
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.fabricante = v_item(idxItem).fabricante
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.produto = v_item(idxItem).produto
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao = v_item(idxItem).descricao
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao_html = v_item(idxItem).descricao_html
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = v_item(idxItem).qtde
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque = 0
	#								if Not estoque_verifica_disponibilidade_integral_v2(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente, vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque) then
	#									alerta=texto_add_br(alerta)
	#									alerta=alerta & "Falha ao tentar consultar disponibilidade no estoque do produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto
	#									end if
	#								end if
	#							end if
	#						end if
	#
	#					if alerta <> "" then exit for
	#					next
	#				end if
	#
	#			if alerta <> "" then exit for
	#			next
	#		end if 'if alerta=""
	When Fazer esta validação

Scenario: Validar estoque - não implementado
	#loja/PedidoNovoConsiste.asp
	#loja/PedidoNovoConfirma.asp
	#exatamente o mesmo código nas duas, exceto:
	#	loja/PedidoNovoConfirma.asp tem um bloco a mais para avisar de estoque que está desabilitado

	#Desde:
	#'	HÁ PRODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS)
	#	erro_produto_indisponivel = False
	#	if alerta="" then
	#.....até.....
	#'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA
	#	dim qtde_empresa_selecionada, lista_empresa_selecionada
	#(inclusive esse bloco
	#Não implementamos porque isso é só para avisar na tela. Atualmente, sempre permite a venda sem estoque
	When Nada a fazer


Scenario: descontinuado
	#loja/PedidoNovoConsiste.asp
#						alerta=alerta & "Produto (" & v_item(i).fabricante & ")" & v_item(i).produto & " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada."
	When Fazer esta validação

