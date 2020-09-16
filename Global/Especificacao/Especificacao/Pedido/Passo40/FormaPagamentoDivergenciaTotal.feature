@ignore
Feature: FormaPagamentoDivergenciaTotal

Scenario: COD_FORMA_PAGTO_A_VISTA - divergência entre o valor total do pedido
	#loja/PedidoNovoConfirma.asp
		#if rb_forma_pagto = COD_FORMA_PAGTO_A_VISTA then vlTotalFormaPagto = vl_total_NF
		#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
		#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - divergência entre o valor total do pedido
#loja/PedidoNovoConsiste.asp
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
#		f.c_pu_valor.focus();

	#loja/PedidoNovoConfirma.asp
	#	if alerta = "" then vlTotalFormaPagto = converte_numero(c_pu_valor)
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - Parcelado no cartão (internet) divergência
#loja/PedidoNovoConsiste.asp
#vtFP=n*vp;
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');

	#loja/PedidoNovoConfirma.asp
	#if alerta = "" then vlTotalFormaPagto = converte_numero(c_pc_qtde) * converte_numero(c_pc_valor)
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA - Parcelado no cartão (maquineta) divergência
#loja/PedidoNovoConsiste.asp
#vtFP=n*vp;
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
#		f.c_pc_maquineta_valor.focus();

	#loja/PedidoNovoConfirma.asp
	#if alerta = "" then vlTotalFormaPagto = converte_numero(c_pc_maquineta_qtde) * converte_numero(c_pc_maquineta_valor)
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA - Parcelado com entrada divergência
	#loja/PedidoNovoConsiste.asp
	#vtFP=ve+(n*vp);
	#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
	#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
	#		f.c_pce_prestacao_valor.focus();
	#loja/PedidoNovoConfirma.asp
	#vlTotalFormaPagto = converte_numero(c_pce_entrada_valor) + (converte_numero(c_pce_prestacao_qtde) * converte_numero(c_pce_prestacao_valor))
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA - Parcelado sem entrada divergência
	#loja/PedidoNovoConsiste.asp
	#vtFP=ve+(n*vp);
	#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
	#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
	#		f.c_pse_demais_prest_valor.focus();
	#loja/PedidoNovoConfirma.asp
	#vlTotalFormaPagto = converte_numero(c_pse_prim_prest_valor) + (converte_numero(c_pse_demais_prest_qtde) * converte_numero(c_pse_demais_prest_valor))
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	When Fazer esta validação

Scenario: c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia
	#loja/PedidoNovoConfirma.asp
	#	dim c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas, coeficiente
	#	dim c_custoFinancFornecTipoParcelamentoConferencia, c_custoFinancFornecQtdeParcelasConferencia
	#	c_custoFinancFornecTipoParcelamento = Trim(Request.Form("c_custoFinancFornecTipoParcelamento"))
	#	c_custoFinancFornecQtdeParcelas = Trim(Request.Form("c_custoFinancFornecQtdeParcelas"))
	#	if rb_forma_pagto=COD_FORMA_PAGTO_A_VISTA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA
	#		c_custoFinancFornecQtdeParcelasConferencia="0"
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELA_UNICA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia="1"
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_CARTAO then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pc_qtde
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pc_maquineta_qtde
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pce_prestacao_qtde
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=Cstr(converte_numero(c_pse_demais_prest_qtde)+1)
	#	else
	#		c_custoFinancFornecTipoParcelamentoConferencia=""
	#		c_custoFinancFornecQtdeParcelasConferencia="0"
	#		end if
	#
	#	if alerta = "" then
	#		if c_custoFinancFornecTipoParcelamentoConferencia<>c_custoFinancFornecTipoParcelamento then
	#			alerta="Foi detectada uma inconsistência no tipo de parcelamento do pagamento (código esperado=" & c_custoFinancFornecTipoParcelamentoConferencia & ", código lido=" & c_custoFinancFornecTipoParcelamento & ")"
	#		elseif converte_numero(c_custoFinancFornecQtdeParcelasConferencia)<>converte_numero(c_custoFinancFornecQtdeParcelas) then
	#			alerta="Foi detectada uma inconsistência na quantidade de parcelas de pagamento (qtde esperada=" & c_custoFinancFornecQtdeParcelasConferencia & ", qtde lida=" & c_custoFinancFornecQtdeParcelas & ")"
	#			end if
	#		end if
	When Fazer esta validação
