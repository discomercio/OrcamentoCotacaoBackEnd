@ignore
@Especificacao.Pedido.Passo30
Feature: Validações do Indicador

Background: Configuração
	#na ApiMagento, o valor é lido do appsettings. Verificado no "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"
	Given Ignorar scenario no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"

	Given Reiniciar banco ao terminar cenário

	#setup da tabela com indicadores permitidos e não permitidos
	Given Limpar tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "apelido" = "Válido 201"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "status" = "A"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "loja" = "201"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR"

	And Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "apelido" = "Inativo 201"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "status" = "I"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "loja" = "201"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR"

	And Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "apelido" = "Válido 202"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "status" = "A"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "loja" = "202"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR"

	And Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "apelido" = "Válido 205"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "status" = "A"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "loja" = "205"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "vendedor" = "usuario 205"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR"

	And Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "apelido" = "Outro 205"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "status" = "A"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "loja" = "205"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR", campo "vendedor" = "Outro 205"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR"


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

#Const NUMERO_LOJA_ECOMMERCE_AR_CLUBE = "201"
#Const NUMERO_LOJA_OLD03 = "300"
#Const NUMERO_LOJA_OLD03_BONIFICACAO = "301"
#Const NUMERO_LOJA_OLD03_ASSISTENCIA = "302"
#Const NUMERO_LOJA_MARCELO_ARTVEN = "305"
#Const NUMERO_LOJA_TRANSFERENCIA = "01"
#Const NUMERO_LOJA_KITS = "02"


Scenario: indicador válido - ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA

	#somente a loja tem a configuracao de ID_PARAM_SITE, ignoramos nos outros ambientes
	Given Ignorar scenario no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Ignorar scenario no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

	#	if ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA then
	#		strSql = "SELECT " & _
	#					"*" & _
	#				" FROM t_ORCAMENTISTA_E_INDICADOR" & _
	#				" WHERE" & _
	#					" (status = 'A')" & _
	#				" ORDER BY" & _
	#					" apelido"
	Given Pedido base
	Given Reiniciar appsettings ao terminar cenário
	When Informo "ID_PARAM_SITE" = "COD_SITE_ASSISTENCIA_TECNICA"
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 201"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "ID_PARAM_SITE" = "COD_SITE_ASSISTENCIA_TECNICA"
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Inativo 201"
	Then Erro "Indicador inválido"

	#aceita de outras lojas também
	Given Pedido base
	When Informo "ID_PARAM_SITE" = "COD_SITE_ASSISTENCIA_TECNICA"
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 202"
	Then Sem nenhum erro



Scenario: indicador válido - NUMERO_LOJA_ECOMMERCE_AR_CLUBE
#		'	TODOS OS VENDEDORES COMPARTILHAM OS MESMOS INDICADORES
#			strSql = "SELECT " & _
#						"*" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#					" WHERE" & _
#						" (status = 'A')" & _
#						" AND (loja = '" & loja & "')" & _
#					" ORDER BY" & _
#						" apelido"

	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 201"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Inativo 201"
	Then Erro "Indicador inválido"

	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 202"
	Then Erro "Indicador inválido"

Scenario: indicador válido - NUMERO_LOJA_OLD03
#		elseif (loja = NUMERO_LOJA_OLD03) Or (loja = NUMERO_LOJA_OLD03_BONIFICACAO) Or (operacao_permitida(OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO, s_lista_operacoes_permitidas)) then
#		'	OLD03: LISTA COMPLETA DOS INDICADORES LIBERADA
#			strSql = "SELECT " & _
#						"*" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR" & _
#					" WHERE" & _
#						" (status = 'A')" & _
#					" ORDER BY" & _
#						" apelido"

	Given Pedido base
	When Informo "loja" = "300"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 201"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "loja" = "300"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Inativo 201"
	Then Erro "Indicador inválido"

	#aceita de outras lojas também
	Given Pedido base
	When Informo "loja" = "300"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 202"
	Then Sem nenhum erro

Scenario: indicador válido - NUMERO_LOJA_OLD03_BONIFICACAO
	Given Pedido base
	When Informo "loja" = "301"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 201"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "loja" = "301"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Inativo 201"
	Then Erro "Indicador inválido"

	#aceita de outras lojas também
	Given Pedido base
	When Informo "loja" = "301"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 202"
	Then Sem nenhum erro


Scenario: indicador válido - OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO
	Given Reiniciar permissões do usuário quando terminar o cenário
	And Usuário com permissão OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO

	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 201"
	Then Sem nenhum erro

	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Inativo 201"
	Then Erro "Indicador inválido"

	#aceita de outras lojas também
	Given Pedido base
	When Informo "loja" = "201"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 202"
	Then Sem nenhum erro


Scenario: indicador válido - outras lojas

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

	Given Reiniciar permissões do usuário quando terminar o cenário
	And Usuário sem permissão OP_LJA_SELECIONAR_QUALQUER_INDICADOR_EM_PEDIDO_NOVO

	#usamos a 205
	Given Pedido base
	When Informo "loja" = "205"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Válido 205"
	And Informo "usuario" = "usuario 205"
	Then Sem nenhum erro

	#neste caso o vendedor é outro
	Given Pedido base
	When Informo "loja" = "205"
	And Informo "rb_indicacao" = "S"
	And Informo "c_indicador" = "Outro 205"
	And Informo "usuario" = "usuario 205"
	Then Erro "Indicador inválido"



Scenario: indicador pode informar permite_RA_status?
#implementado em Especificacao\Pedido\Passo60\Validacao\RaIndicador.feature


Scenario: indicador e perc_rt
	#if (f.c_loja.value != NUMERO_LOJA_ECOMMERCE_AR_CLUBE) {
	#    if (f.c_indicador.value == "") {
	#        if(f.c_perc_RT.value != "") {
	#            if (parseFloat(f.c_perc_RT.value.replace(',','.')) > 0) {
	#                alert('Não é possível gravar o pedido com o campo "Indicador" vazio e "COM(%)" maior do que zero!!');
	#                f.c_perc_RT.focus();
	#                return;
	Given fazer esta validação
