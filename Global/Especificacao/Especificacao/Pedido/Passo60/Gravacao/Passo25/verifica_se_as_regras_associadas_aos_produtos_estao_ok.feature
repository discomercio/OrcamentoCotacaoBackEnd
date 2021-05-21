@Especificacao.Pedido.Passo60.Gravacao.Passo25
@GerenciamentoBanco
Feature: verifica_se_as_regras_associadas_aos_produtos_estao_ok

Background: Configuracao
	Given Reiniciar banco ao terminar cenário
	Given Chamar ObtemCtrlEstoqueProdutoRegra

#loja/PedidoNovoConfirma.asp
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
#					'alerta_aux=texto_add_br(alerta_aux)
#					'alerta_aux=alerta_aux & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & ": regra '" & vProdRegra(iRegra).regra.apelido & "' (Id=" & vProdRegra(iRegra).regra.id & ") define o CD '" & obtem_apelido_empresa_NFe_emitente(id_nfe_emitente_selecao_manual) & "' como 'desativado'"
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
#
Scenario: Teste 1 - TwmsRegraCd.Id = 0
	# if converte_numero(vProdRegra(iRegra).regra.id) = 0 then
	#		alerta=texto_add_br(alerta)
	#		alerta=alerta & "Produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto &
	#                " não possui regra de consumo do estoque associada"
	#
	#if (r.TwmsRegraCd.Id == 0)
	Given Lista de CtrlRegra alterar registro de TwmsRegraCd do produto = "003220", campo "id" = "0"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Produto (003)003220 não possui regra de consumo do estoque associada"

Scenario: Teste 2 - TwmsRegraCd.St_inativo = 1
	#				elseif vProdRegra(iRegra).regra.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está desativada"
	Given Lista de CtrlRegra alterar registro de TwmsRegraCd do produto = "003220", campo "st_inativo" = "1"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Regra de consumo do estoque 'Regra 05' associada ao produto(003)003220 está desativada"

Scenario: Teste 3 - TwmsRegraCdUf.St_inativo = 1
	#				elseif vProdRegra(iRegra).regra.regraUF.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para a UF '" & r_cliente.uf & "'"
	#else if (r.TwmsRegraCdXUf.St_inativo == 1)
	Given Lista de CtrlRegra alterar registro de TwmsRegraCdUf do produto = "003220", campo "st_inativo" = "1"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Regra de consumo do estoque 'Regra 05' associada ao produto (003)003220 está bloqueada para a UF 'SP'"

Scenario: Teste 4 - TwmsRegraCdXUfXPessoa.St_inativo = 1
	#				elseif vProdRegra(iRegra).regra.regraUF.regraPessoa.st_inativo = 1 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " está bloqueada para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	#TwmsRegraCdXUfXPessoa.St_inativo == 1
	Given Lista de CtrlRegra alterar registro de TwmsRegraCdXUfXPessoa do produto = "003220", campo "st_inativo" = "1"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Regra de consumo do estoque 'Regra 05' associada ao produto (003)003220 está bloqueada para clientes 'PF' da UF 'SP'"

Scenario: Teste 5 - TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente = 0
	#				elseif converte_numero(vProdRegra(iRegra).regra.regraUF.regraPessoa.spe_id_nfe_emitente) = 0 then
	#					alerta=texto_add_br(alerta)
	#					alerta=alerta & "Regra de consumo do estoque '" & vProdRegra(iRegra).regra.apelido & "' associada ao produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" & descricao_tipo_pessoa & "' da UF '" & r_cliente.uf & "'"
	#else if (r.TwmsRegraCdXUfXPessoa != null && r.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
	Given Lista de CtrlRegra alterar registro de TwmsRegraCdXUfXPessoa do produto = "003220", campo "spe_id_nfe_emitente" = "0"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Regra de consumo do estoque 'Regra 05' associada ao produto (003)003220 não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes 'PF' da UF 'SP'"

Scenario: Teste 6 - TwmsCdXUfXPessoaXCd.St_inativo = 0
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
	#if (verificaErros == 0 && id_nfe_emitente_selecao_manual == 0)
	Given Lista de CtrlRegra alterar registro de TwmsCdXUfXPessoaXCd do produto = "003220" e id_nfe_emitente = "4903", campo "st_inativo" = "1"
	Given Chamar VerificarRegrasAssociadasAosProdutos
	Then Erro "Regra de consumo do estoque 'Regra 05' associada ao produto (003)003220 não especifica nenhum CD ativo para clientes 'PF' da UF 'SP'"