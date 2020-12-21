@ignore
@Especificacao.Pedido.Passo40
@GerenciamentoBanco
Feature: FormaPagamentoPermitidas

#loja/PedidoNovoConsiste.asp linha 3104 e seguintes
#
#se rb_indicacao = "S" usa forma_pagto_liberada_da_prestacao_monta_itens_select
#caso contrário, usa forma_pagto_da_prestacao_monta_itens_select
#
#
#se aplica aos campos
#op_pce_prestacao_forma_pagto
#op_pse_prim_prest_forma_pagto
#op_pse_demais_prest_forma_pagto
#
#trecho do ASP:
#				<td align="left">
#				  <select id="op_pce_prestacao_forma_pagto" name="op_pce_prestacao_forma_pagto" onclick="fPED.rb_forma_pagto[<%=Cstr(intIdx)%>].click();" onchange="recalcula_RA_Liquido();">
#					<%	if rb_indicacao = "S" then
#							Response.Write forma_pagto_liberada_da_prestacao_monta_itens_select(Null, c_indicador, r_cliente.tipo)
#						else
#							Response.Write forma_pagto_da_prestacao_monta_itens_select(Null)
#							end if%>
#				  </select>
#				  <span style="width:10px;">&nbsp;</span>
#				  <input name="c_pce_prestacao_qtde" id="c_pce_prestacao_qtde" class="Cc" maxlength="2" style="width:30px;" onclick="fPED.rb_forma_pagto[<%=Cstr(intIdx)%>].click();" onkeypress="if (digitou_enter(true)&&tem_info(this.value)) fPED.c_pce_prestacao_valor.focus(); filtra_numerico();" onblur="recalcula_RA_Liquido();recalculaCustoFinanceiroPrecoLista();pce_calcula_valor_parcela();"
#					value='<%if (c_custoFinancFornecTipoParcelamento=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) then Response.Write c_custoFinancFornecQtdeParcelas%>'
#					>
#				  <span class="C" style="margin-right:0pt;">&nbsp;X&nbsp;&nbsp;&nbsp;<%=SIMBOLO_MONETARIO%></span
#				  ><input name="c_pce_prestacao_valor" id="c_pce_prestacao_valor" class="Cd" maxlength="18" style="width:90px;" onclick="fPED.rb_forma_pagto[<%=Cstr(intIdx)%>].click();" onkeypress="if (digitou_enter(true)&&tem_info(this.value)) fPED.c_pce_prestacao_periodo.focus(); filtra_moeda_positivo();" onblur="this.value=formata_moeda(this.value);recalcula_RA_Liquido();" value=''>
#				</td>


#function forma_pagto_liberada_da_prestacao_monta_itens_select(byval id_default, byval id_orcamentista_e_indicador, byval tipo_cliente)
#dim s, x, r, strResp, ha_default
#	id_default = Trim("" & id_default)
#	ha_default=False
#	s = "SELECT " & _
#			"*" & _
#		" FROM t_FORMA_PAGTO" & _
#		" WHERE" & _
#			" (hab_prestacao=1)" & _
#			" AND " & _
#			"(" & _
#				"id NOT IN " & _
#					"(" & _
#					"SELECT" & _
#						" id_forma_pagto" & _
#					" FROM t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO tOIRFP" & _
#					" WHERE" & _
#						"(" & _
#							"(id_orcamentista_e_indicador = '" & Trim(id_orcamentista_e_indicador) & "')" & _
#							" OR " & _
#							"(id_orcamentista_e_indicador = '" & ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS & "')" & _
#						")" & _
#						" AND (tipo_cliente = '" & Trim(tipo_cliente) & "')" & _
#						" AND (st_restricao_ativa <> 0)" & _
#					")" & _
#			")" & _
#		" ORDER BY" & _
#			" ordenacao"

Background: preparar o banco
	Given Reiniciar banco ao terminar cenário
	Given Limpar tabela "t_FORMA_PAGTO"

	#id 1 permitido
	Given Novo registro na tabela "t_FORMA_PAGTO"
	And Novo registro em "t_FORMA_PAGTO", campo "id" = "1"
	And Gravar registro em "t_FORMA_PAGTO"

	#id 2 negado
	Given Novo registro na tabela "t_FORMA_PAGTO"
	And Novo registro em "t_FORMA_PAGTO", campo "id" = "2"
	And Gravar registro em "t_FORMA_PAGTO"

	Given Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "id_forma_pagto" = "2"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "id_orcamentista_e_indicador" = "especial: id do orcamentista"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "st_restricao_ativa" = "1"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO"

	#id 3 negado
	Given Novo registro na tabela "t_FORMA_PAGTO"
	And Novo registro em "t_FORMA_PAGTO", campo "id" = "3"
	And Gravar registro em "t_FORMA_PAGTO"

	Given Novo registro na tabela "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "id_forma_pagto" = "3"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "id_orcamentista_e_indicador" = "especial: ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS"
	And Novo registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO", campo "st_restricao_ativa" = "1"
	And Gravar registro em "t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO"



Scenario Outline: validar op_pce_prestacao_forma_pagto "indicacao" = "S"
	Given Pedido base
	When Informo "indicacao" = "S"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "1"
	Then Sem erro "Forma de pagamento não aceita para esse indicador."
	Given Pedido base
	When Informo "indicacao" = "S"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "2"
	Then Erro "Forma de pagamento não aceita para esse indicador."
	Given Pedido base
	When Informo "indicacao" = "S"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "3"
	Then Erro "Forma de pagamento não aceita para esse indicador."
	Examples:
		| campo                           | rb_forma_pagto                        |
		| op_pce_prestacao_forma_pagto    | COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA |
		| op_pse_prim_prest_forma_pagto   | COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA |
		| op_pse_demais_prest_forma_pagto | COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA |


Scenario Outline: validar op_pce_prestacao_forma_pagto "indicacao" = "N"
	#este não aplica as restrições de t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO
	Given Pedido base
	When Informo "indicacao" = "N"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "1"
	Then Sem erro "Forma de pagamento não aceita para esse indicador."

	Given Pedido base
	When Informo "indicacao" = "N"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "2"
	Then Sem erro "Forma de pagamento não aceita para esse indicador."

	Given Pedido base
	When Informo "indicacao" = "N"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "3"
	Then Sem erro "Forma de pagamento não aceita para esse indicador."

	Given Pedido base
	When Informo "indicacao" = "N"
	When Informo "rb_forma_pagto" = "<rb_forma_pagto>"
	When Informo "<campo>" = "4"
	Then Erro "Forma de pagamento não aceita para esse indicador."

	Examples:
		| campo                           | rb_forma_pagto                        |
		| op_pce_prestacao_forma_pagto    | COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA |
		| op_pse_prim_prest_forma_pagto   | COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA |
		| op_pse_demais_prest_forma_pagto | COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA |

