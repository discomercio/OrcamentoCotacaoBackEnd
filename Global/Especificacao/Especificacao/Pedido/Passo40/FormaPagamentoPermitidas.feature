@ignore
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


@mytag
Scenario: validar op_pce_prestacao_forma_pagto
	When Fazer esta validação
	Then Erro "Forma de pagamento não aceita para esse indicador."

