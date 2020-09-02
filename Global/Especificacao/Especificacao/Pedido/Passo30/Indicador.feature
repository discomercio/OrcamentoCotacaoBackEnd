@ignore
Feature: Validações do Indicador

Background: Configuração
	#na ApiMagento, o valor é lido do appsettings. Verificado no "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"
	Given Ignorar feature no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"


Scenario: indicador preenchido
#loja/PedidoNovoConsiste.asp
#		if rb_indicacao = "" then
#			alerta = "Informe se o pedido é com indicação ou não."
#		elseif rb_indicacao = "S" then
#			if c_indicador = "" then
#				alerta = "Informe quem é o indicador."
#			elseif rb_RA = "" then
#				alerta = "Informe se o pedido possui RA ou não."
#				end if
	Given Pedido base
	When Informo "rb_indicacao" = ""
	Then Erro "Informe se o pedido é com indicação ou não."

	Given Pedido base
	When Informo "rb_indicacao" = "S"
	When Informo "c_indicador" = ""
	Then Erro "Informe quem é o indicador."

	Given Pedido base
	When Informo "rb_RA" = ""
	Then Erro "Informe se o pedido possui RA ou não."


#todo: afazer: fazr esta validação

#' ___________________________________________________________________________
#' INDICADORES MONTA ITENS SELECT
#' LEMBRE-SE: O ORÇAMENTISTA É CONSIDERADO AUTOMATICAMENTE UM INDICADOR!!
#function indicadores_monta_itens_select(byval id_default, byref strResp, byref strJsScript)
#dim x, r, ha_default, strSql
#	id_default = Trim("" & id_default)
#	ha_default=False
#
#	strJsScript = "<script language='JavaScript'>" & chr(13) & _
#					"var vIndicador = new Array();" & chr(13) & _
#					"vIndicador[0] = new oIndicador('', 0);" & chr(13)
#
#	if ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA then
#		strSql = "SELECT " & _
#					"*" & _
#				" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#				" WHERE" & _
#					" (status = 'A')" & _
#				" ORDER BY" & _
#					" apelido"
#	else
#		'10/01/2020 - Unis - Desativação do acesso dos vendedores a todos os parceiros da Unis
#		if (False And isLojaVrf(loja)) Or (loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE) then
#		'	TODOS OS VENDEDORES COMPARTILHAM OS MESMOS INDICADORES
#			strSql = "SELECT " & _
#						"*" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#					" WHERE" & _
#						" (status = 'A')" & _
#						" AND (loja = '" & loja & "')" & _
#					" ORDER BY" & _
#						" apelido"
#		elseif (loja = NUMERO_LOJA_OLD03) Or (loja = NUMERO_LOJA_OLD03_BONIFICACAO) Or (operacao_permitida(OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO, s_lista_operacoes_permitidas)) then
#		'	OLD03: LISTA COMPLETA DOS INDICADORES LIBERADA
#			strSql = "SELECT " & _
#						"*" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#					" WHERE" & _
#						" (status = 'A')" & _
#					" ORDER BY" & _
#						" apelido"
#		else
#			strSql = "SELECT " & _
#						"*" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#					" WHERE" & _
#						" (status = 'A')" & _
#						" AND (vendedor = '" & usuario & "')" & _
#					" ORDER BY" & _
#						" apelido"
#			end if
#		end if
#	
#	set r = cn.Execute(strSql)
#	strResp = ""
#	do while Not r.eof
#		intQtdeIndicadores = intQtdeIndicadores + 1
#		x = Trim("" & r("apelido"))
#		if (id_default<>"") And (id_default=x) then
#			strResp = strResp & "<OPTION SELECTED"
#			ha_default=True
#		else
#			strResp = strResp & "<OPTION"
#			end if
#		strResp = strResp & " VALUE='" & x & "'>"
#		strResp = strResp & x & " - " & Trim("" & r("razao_social_nome"))
#		strResp = strResp & "</OPTION>" & chr(13)
#		
#		strJsScript = strJsScript & _
#						"vIndicador[vIndicador.length] = new oIndicador('" & QuotedStr(Trim("" & r("apelido"))) & "', " & Trim("" & r("permite_RA_status")) & ");" & chr(13)
#		r.MoveNext
#		loop
#
#	if Not ha_default then
#		strResp = "<OPTION SELECTED VALUE=''>&nbsp;</OPTION>" & chr(13) & strResp
#		end if
#	
#	strJsScript = strJsScript & "</script>" & chr(13)
#	
#	r.close
#	set r=nothing
#end function

#todo: afazer: testar se  o indicador pode informar permite_RA_status
