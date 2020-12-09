@ignore
Feature: LimiteDesconto_perc_comissao_e_desconto

Background: Configurar descontos da loja
	Given Reinciar banco ao terminar cenário
	#todo mundo com 10% de máximo
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao" = "10"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "10"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "10"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_pj" = "10"
	#desabilita os meios preferenciais
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = ""

Scenario: Percentual de comissão excede o máximo permitido!!
#loja/PedidoNovoConsiste.asp
#if (perc_RT != 0) {
#	// RT excede limite máximo?
#	if (perc_RT > perc_max_RT) {
#		alert("Percentual de comissão excede o máximo permitido!!");
#		return;
#	}
#perc_max_RT = obtem_perc_max_comissao_e_desconto_por_loja(byval loja)
#	s = "SELECT" perc_max_comissao,
#	" FROM t_LOJA" & _
#	" WHERE" & _
#		" (CONVERT(smallint,loja) = " & loja & ")"

	Given Pedido base
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao" = "12"
	And Informo "perc_RT" = "13"
	Then Erro "Percentual de comissão excede o máximo permitido!!"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar
#vamos criar um pedido que exceda o limite padrão e verificar se está usando a configuração correta.

#loja/PedidoNovoConfirma.asp

#rCD = obtem_perc_max_comissao_e_desconto_por_loja(byval loja)
#	s = "SELECT" *,
#	" FROM t_LOJA" & _
#	" WHERE" & _
#		" (CONVERT(smallint,loja) = " & loja & ")"

#vMPN2
#set rP = get_registro_t_parametro(ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto)
#if Trim("" & rP.id) <> "" then
#	vMPN2 = Split(rP.campo_texto, ",")
#Const ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto = "PercMaxComissaoEDesconto_Nivel2_MeiosPagto"

#'	ANALISA O PERCENTUAL DE COMISSÃO+DESCONTO
#	dim perc_comissao_e_desconto_a_utilizar
#	dim s_pg, blnPreferencial
#	dim vlNivel1, vlNivel2
#	if tipo_cliente = ID_PJ then
#		perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_pj
#	else
#		perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto
#		end if

#primeiro, tem que dar o erro (no Background, colocamos todo mundo com 10% de limite)
Scenario: Verifica o perc_comissao_e_desconto_a_utilizar 1
	Given Pedido base
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"

#agora verifica que respeita o limite maior
Scenario: Verifica o perc_comissao_e_desconto_a_utilizar 2 PJ
	Given Pedido base PJ
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_pj" = "20"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar 2 PF
	Given Pedido base PF
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto" = "20"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_A_VISTA
	#
	#	if alerta="" then
	#		if rb_forma_pagto = COD_FORMA_PAGTO_A_VISTA then
	#			s_pg = Trim(op_av_forma_pagto)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						if tipo_cliente = ID_PJ then
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
	#						else
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
	#							end if
	#						exit for
	#						end if
	#					next
	#				end if
	Given Pedido base PJ a vista
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_A_VISTA PJ
	Given Pedido base PJ a vista
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_av_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_A_VISTA PF
	Given Pedido base PF a vista
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_av_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro



Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELA_UNICA
	#		elseif rb_forma_pagto = COD_FORMA_PAGTO_PARCELA_UNICA then
	#			s_pg = Trim(op_pu_forma_pagto)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						if tipo_cliente = ID_PJ then
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
	#						else
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
	#							end if
	#						exit for
	#						end if
	#					next
	#				end if
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELA_UNICA
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELA_UNICA PJ
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELA_UNICA
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pu_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELA_UNICA PF
	Given Pedido base PF COD_FORMA_PAGTO_PARCELA_UNICA
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pu_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro



Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO
	#		elseif rb_forma_pagto = COD_FORMA_PAGTO_PARCELADO_CARTAO then
	#			s_pg = Trim(ID_FORMA_PAGTO_CARTAO)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						if tipo_cliente = ID_PJ then
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
	#						else
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
	#							end if
	#						exit for
	#						end if
	#					next
	#				end if
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_CARTAO
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO PJ
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_CARTAO
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	#	Const ID_FORMA_PAGTO_CARTAO = "5"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "5"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO PF
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_CARTAO
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "5"
	Then Sem nenhum erro


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA
	#		elseif rb_forma_pagto = COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA then
	#			s_pg = Trim(ID_FORMA_PAGTO_CARTAO_MAQUINETA)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						if tipo_cliente = ID_PJ then
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
	#						else
	#							perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
	#							end if
	#						exit for
	#						end if
	#					next
	#				end if
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA PJ
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	#	Const ID_FORMA_PAGTO_CARTAO_MAQUINETA = "7"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "7"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA PF
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA
	Given Modificar pedido para "15" por cento de desconto
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "7"
	Then Sem nenhum erro


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA
	#		elseif rb_forma_pagto = COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA then
	#		'	Identifica e contabiliza o valor da entrada
	#			blnPreferencial = False
	#			s_pg = Trim(op_pce_entrada_forma_pagto)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						blnPreferencial = True
	#						exit for
	#						end if
	#					next
	#				end if
	#
	#			if blnPreferencial then
	#				vlNivel2 = converte_numero(c_pce_entrada_valor)
	#			else
	#				vlNivel1 = converte_numero(c_pce_entrada_valor)
	#				end if
	#
	#		'	Identifica e contabiliza o valor das parcelas
	#			blnPreferencial = False
	#			s_pg = Trim(op_pce_prestacao_forma_pagto)
	#			if s_pg <> "" then
	#				for i=Lbound(vMPN2) to Ubound(vMPN2)
	#				'	O meio de pagamento selecionado é um dos preferenciais
	#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
	#						blnPreferencial = True
	#						exit for
	#						end if
	#					next
	#				end if
	#
	#			if blnPreferencial then
	#				vlNivel2 = vlNivel2 + converte_numero(c_pce_prestacao_qtde) * converte_numero(c_pce_prestacao_valor)
	#			else
	#				vlNivel1 = vlNivel1 + converte_numero(c_pce_prestacao_qtde) * converte_numero(c_pce_prestacao_valor)
	#				end if
	#
	#		'	O montante a pagar por meio de pagamento preferencial é maior que 50% do total?
	#			if vlNivel2 > (vl_total/2) then
	#				if tipo_cliente = ID_PJ then
	#					perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
	#				else
	#					perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
	#					end if
	#				end if
	#
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PJ tudo preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pce_entrada_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PF tudo preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pu_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PJ entrada preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pce_entrada_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Erro "Desconto exede o permitido"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PF entrada preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pu_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Erro "Desconto exede o permitido"


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PJ prestacao preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pce_entrada_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "2"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA PF prestacao preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA com 10% de entrada
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pu_forma_pagto" = "1"
	And Informo "op_pce_prestacao_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "2"
	Then Sem nenhum erro


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA
#		elseif rb_forma_pagto = COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA then
#		'	Identifica e contabiliza o valor da 1ª parcela
#			blnPreferencial = False
#			s_pg = Trim(op_pse_prim_prest_forma_pagto)
#			if s_pg <> "" then
#				for i=Lbound(vMPN2) to Ubound(vMPN2)
#				'	O meio de pagamento selecionado é um dos preferenciais
#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
#						blnPreferencial = True
#						exit for
#						end if
#					next
#				end if
#
#			if blnPreferencial then
#				vlNivel2 = converte_numero(c_pse_prim_prest_valor)
#			else
#				vlNivel1 = converte_numero(c_pse_prim_prest_valor)
#				end if
#
#		'	Identifica e contabiliza o valor das parcelas
#			blnPreferencial = False
#			s_pg = Trim(op_pse_demais_prest_forma_pagto)
#			if s_pg <> "" then
#				for i=Lbound(vMPN2) to Ubound(vMPN2)
#				'	O meio de pagamento selecionado é um dos preferenciais
#					if Trim("" & s_pg) = Trim("" & vMPN2(i)) then
#						blnPreferencial = True
#						exit for
#						end if
#					next
#				end if
#
#			if blnPreferencial then
#				vlNivel2 = vlNivel2 + converte_numero(c_pse_demais_prest_qtde) * converte_numero(c_pse_demais_prest_valor)
#			else
#				vlNivel1 = vlNivel1 + converte_numero(c_pse_demais_prest_qtde) * converte_numero(c_pse_demais_prest_valor)
#				end if
#
#		'	O montante a pagar por meio de pagamento preferencial é maior que 50% do total?
#			if vlNivel2 > (vl_total/2) then
#				if tipo_cliente = ID_PJ then
#					perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2_pj
#				else
#					perc_comissao_e_desconto_a_utilizar = rCD.perc_max_comissao_e_desconto_nivel2
#					end if
#				end if
#			end if
#		end if

	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	Then Erro "Desconto exede o permitido"


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PJ tudo preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PF tudo preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "1"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Sem nenhum erro


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PJ entrada preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Erro "Desconto exede o permitido"

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PF entrada preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "1"
	Then Erro "Desconto exede o permitido"


Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PJ prestacao preferencial
	Given Pedido base PJ COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2_pj" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "2"
	Then Sem nenhum erro

Scenario: Verifica o perc_comissao_e_desconto_a_utilizar COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA PF prestacao preferencial
	Given Pedido base PF COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA com 10% de primeira prestação
	Given Modificar pedido para "15" por cento de desconto
	And Informo "op_pse_prim_prest_forma_pagto" = "1"
	And Informo "op_pse_demais_prest_forma_pagto" = "2"
	And Alterar registro em "t_LOJA", busca "loja" = "especial: loja atual", campo "perc_max_comissao_e_desconto_nivel2" = "20"
	And Alterar registro em "t_PARAMETRO", busca "id" = "especial: loja atual", campo "PercMaxComissaoEDesconto_Nivel2_MeiosPagto" = "2"
	Then Sem nenhum erro

