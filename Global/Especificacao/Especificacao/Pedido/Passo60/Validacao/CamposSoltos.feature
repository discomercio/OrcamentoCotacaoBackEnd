@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: Pedido Passo 60 validação CamposSoltos

Scenario: campos soltos 1 - loja que indicou
	#loja/PedidoNovoConfirma.asp
	#'	OBTÉM O PERCENTUAL DA COMISSÃO
	#	if alerta = "" then
	#		if s_loja_indicou<>"" then
	#			s = "SELECT loja, comissao_indicacao FROM t_LOJA WHERE (loja='" & s_loja_indicou & "')"
	#			set rs = cn.execute(s)
	#			if Not rs.Eof then
	#				comissao_loja_indicou = rs("comissao_indicacao")
	#			else
	#				alerta = "Loja " & s_loja_indicou & " não está cadastrada."
	#				end if
	#			end if
	#		end if
	Given Pedido base
	When Informo "DadosCliente.Vendedor" = "S"
	When Informo "Loja_Indicou" = "203"
	Then Erro "Loja 202 não está cadastrada."

Scenario: campos soltos 2 - nome do indicador
	#loja/PedidoNovoConfirma.asp
	#	if alerta="" then
	#		if rb_indicacao = "" then
	#			alerta = "Informe se o pedido é com indicação ou não."
	#		elseif rb_indicacao = "S" then
	#			if c_indicador = "" then
	#				alerta = "Informe quem é o indicador."
	#			elseif rb_RA = "" then
	#				alerta = "Informe se o pedido possui RA ou não."
	#		'	POR SOLICITAÇÃO DO ROGÉRIO, A CONSISTÊNCIA DO LIMITE DE COMPRAS FOI DESATIVADA (NOV/2008)
	#		'	elseif (vl_limite_mensal_disponivel - vl_total) <= 0 then
	#		'		alerta = "Não é possível cadastrar este pedido porque excede o valor do limite mensal estabelecido para o indicador (" & c_indicador & ")"
	#			elseif rb_garantia_indicador = "" then
	#				alerta = "Informe se o pedido é garantido pelo indicador ou não."
	#				end if
	#			end if
	#		end if
	#
	#if rb_indicacao = "" then
	#		alerta = "Informe se o pedido é com indicação ou não."
	# O campo do dto que especifica se é com ou sem indicador "ComIndicador" é bool, então por definição é false
	#
	#elseif rb_RA = "" then
	#		alerta = "Informe se o pedido possui RA ou não."
	#O campo do dto PermiteRAStatus é um short que passa a ser um bool na criação do pedido, então por definição é false
	#
	#		elseif rb_indicacao = "S" then
	#			if c_indicador = "" then
	#				alerta = "Informe quem é o indicador."
	Given Pedido base
	When Informo "ComIndicador" = "1"
	When Informo "NomeIndicador" = ""
	Then Erro "Informe quem é o indicador."

Scenario: campos soltos 3
	#loja/PedidoNovoConfirma.asp
	#		if s_etg_imediata = "" then
	#			alerta = "É necessário selecionar uma opção para o campo 'Entrega Imediata'."
	#			end if
	#
	#		if CLng(s_etg_imediata) = CLng(COD_ETG_IMEDIATA_NAO) then
	#			if c_data_previsao_entrega = "" then
	#				alerta=texto_add_br(alerta)
	#				alerta=alerta & "É necessário informar a data de previsão de entrega"
	#			elseif Not IsDate(c_data_previsao_entrega) then
	#				alerta=texto_add_br(alerta)
	#				alerta=alerta & "Data de previsão de entrega informada é inválida"
	#			elseif StrToDate(c_data_previsao_entrega) <= Date then
	#				alerta=texto_add_br(alerta)
	#				alerta=alerta & "Data de previsão de entrega deve ser uma data futura"
	#				end if
	#			end if
	#		end if
	#

	#When Implementado em "Especificacao.Especificacao.Pedido.Passo50.CamposSoltos

Scenario: campos soltos 4
	#loja/PedidoNovoConfirma.asp
	#if s_bem_uso_consumo = "" then
	#	alerta = "É necessário informar se é 'Bem de Uso/Consumo'."
	#When Implementado em "Especificacao.Especificacao.Pedido.Passo50.CamposSoltos

Scenario: campos soltos 5
	#loja/PedidoNovoConfirma.asp
	#if c_exibir_campo_instalador_instala = "S" then
	#	if s_instalador_instala = "" then
	#		alerta = "É necessário preencher o campo 'Instalador Instala'."
	#When Implementado em "Especificacao.Especificacao.Pedido.Passo50.CamposSoltos