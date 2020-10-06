@ignore
Feature: FormaPagamento

#CUIDADO COM O NOME DA VARIÁVEL!!
#nota em loja/PedidoNovoConsiste.asp e loja/PedidoNovoConfirma.asp
#rb_forma_pagto = Trim(Request.Form("rb_forma_pagto"))
#rs("tipo_parcelamento")=CLng(rb_forma_pagto)
#
#s_forma_pagto=Trim(request("c_forma_pagto"))
#rs("forma_pagto")=s_forma_pagto

Scenario: "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
	#loja/PedidoNovoConsiste.asp
	#		if (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) And _
	#		   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) And _
	#		   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
	#			alerta = "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
	Given Pedido base
	When Informo "custoFinancFornecTipoParcelamento" = "XX"
	Then Erro "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."

Scenario: Não foi informada a quantidade de parcelas
	#loja/PedidoNovoConsiste.asp
	#		if (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) Or _
	#		   (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
	#			if converte_numero(c_custoFinancFornecQtdeParcelas) <= 0 then
	#				alerta = "Não foi informada a quantidade de parcelas para a forma de pagamento selecionada (" & descricaoCustoFinancFornecTipoParcelamento(c_custoFinancFornecTipoParcelamento) &  ")"
	Given Pedido base
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "0"
	Then Erro regex ""Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

	Given Pedido base
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "-1"
	Then Erro regex ""Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

	Given Pedido base
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "0"
	Then Erro regex ""Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

	Given Pedido base
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	And Informo "custoFinancFornecQtdeParcelas" = "-1"
	Then Erro regex ""Não foi informada a quantidade de parcelas para a forma de pagamento selecionada.*"

Scenario: Opção de parcelamento não disponível para fornecedor
##loja/PedidoNovoConsiste.asp
#				if c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA then
#					coeficiente = 1
#				else
#					s = "SELECT " & _
#							"*" & _
#						" FROM t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR" & _
#						" WHERE" & _
#							" (fabricante = '" & .fabricante & "')" & _
#							" AND (tipo_parcelamento = '" & c_custoFinancFornecTipoParcelamento & "')" & _
#							" AND (qtde_parcelas = " & c_custoFinancFornecQtdeParcelas & ")"
#					if rs.Eof then
#						alerta=alerta & "Opção de parcelamento não disponível para fornecedor " & .fabricante & ": " & decodificaCustoFinancFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas) & " parcela(s)"

#loja/PedidoNovoConfirma.asp
#mesma validação, mensagem:
#alerta=alerta & "Opção de parcelamento não disponível para fornecedor " & .fabricante & ": " & decodificaCustoFinancFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas) & " parcela(s)"

	When Fazer esta validação


#//	À Vista

Scenario: op_av_forma_pagto
#loja/PedidoNovoConsiste.asp
#		if (trim(f.op_av_forma_pagto.value)=='') {
#				alert('Indique a forma de pagamento!!');
#loja/PedidoNovoConfirma.asp
#if rb_forma_pagto = COD_FORMA_PAGTO_A_VISTA then
#if op_av_forma_pagto = "" then alerta = "Indique a forma de pagamento (à vista)."

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "op_av_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento!!" ou "Indique a forma de pagamento (à vista)."


#//	Parcela Única
Scenario: op_pu_forma_pagto
	#loja/PedidoNovoConfirma.asp
	#if op_pu_forma_pagto = "" then
	#	alerta = "Indique a forma de pagamento da parcela única."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "op_pu_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento da parcela única."

Scenario: c_pu_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pu_valor.value)=='') {
	#		alert('Indique o valor da parcela única!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pu_valor = "" then
	#	alerta = "Indique o valor da parcela única."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_valor" = ""
	Then Erro "Indique o valor da parcela única!!"

Scenario: c_pu_valor inválido
	#loja/PedidoNovoConsiste.asp
	#ve=converte_numero(f.c_pu_valor.value);
	#if (ve<=0) {
	#		alert('Valor da parcela única é inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pu_valor) <= 0 then
	#	alerta = "Valor da parcela única é inválido."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_valor" = "0"
	Then Erro "Valor da parcela única é inválido!!"
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_valor" = "-1"
	Then Erro "Valor da parcela única é inválido!!"
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_valor" = "1"
	Then Sem Erro "Valor da parcela única é inválido!!"


Scenario: c_pu_vencto_apos
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pu_vencto_apos.value)=='') {
	#		alert('Indique o intervalo de vencimento da parcela única!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pu_vencto_apos = "" then
	#	alerta = "Indique o intervalo de vencimento da parcela única."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_vencto_apos" = ""
	Then Erro "Indique o intervalo de vencimento da parcela única!!"

Scenario: c_pu_vencto_apos inválido
	#loja/PedidoNovoConsiste.asp
	#nip=converte_numero(f.c_pu_vencto_apos.value);
	#if (nip<=0) {
	#		alert('Intervalo de vencimento da parcela única é inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pu_vencto_apos) <= 0 then
	#	alerta = "Intervalo de vencimento da parcela única é inválido."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_vencto_apos" = "0"
	Then Erro "Indique o intervalo de vencimento da parcela única!!"
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_vencto_apos" = "-1"
	Then Erro "Indique o intervalo de vencimento da parcela única!!"
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "c_pu_vencto_apos" = "1"
	Then sem Erro "Indique o intervalo de vencimento da parcela única!!"


#//	Parcelado no cartão (internet)
Scenario: c_pc_qtde
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pc_qtde.value)=='') {
	#		alert('Indique a quantidade de parcelas!!');
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = ""
	Then Erro "Indique a quantidade de parcelas!!"

Scenario: c_pc_qtde inválida
	#loja/PedidoNovoConfirma.asp
	#if c_pc_qtde = "" then
	#	alerta = "Indique a quantidade de parcelas (parcelado no cartão [internet])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = ""
	Then Erro "Indique a quantidade de parcelas (parcelado no cartão [internet])."

	#loja/PedidoNovoConsiste.asp
	#n=converte_numero(f.c_pc_qtde.value);
	#if (n < 1) {
	#		alert('Quantidade de parcelas inválida!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pc_qtde) < 1 then
	#	alerta = "Quantidade de parcelas inválida (parcelado no cartão [internet])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = "0"
	Then Erro "Quantidade de parcelas inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = "-1"
	Then Erro "Quantidade de parcelas inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = "2"
	Then Sem nenhum erro

Scenario: c_pc_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pc_valor.value)=='') {
	#		alert('Indique o valor da parcela!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pc_valor = "" then
	#	alerta = "Indique o valor da parcela (parcelado no cartão [internet])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_valor" = ""
	Then Erro "Indique o valor da parcela!!"

Scenario: c_pc_valorinválido
	#loja/PedidoNovoConsiste.asp
	#vp=converte_numero(f.c_pc_valor.value);
	#if (vp<=0) {
	#		alert('Valor de parcela inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pc_valor) <= 0 then
	#	alerta = "Valor de parcela inválido (parcelado no cartão [internet])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_valor" = "0"
	Then Erro "Valor de parcela inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_valor" = "-1"
	Then Erro "Valor de parcela inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_valor" = "2"
	Then Sem nenhum erro


#//	Parcelado no cartão (maquineta)
Scenario: c_pc_maquineta_qtde
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pc_maquineta_qtde.value)=='') {
	#		alert('Indique a quantidade de parcelas!!');
	#loja/PedidoNovoConfirma.asp
	#if c_pc_maquineta_qtde = "" then
	#	alerta = "Indique a quantidade de parcelas (parcelado no cartão [maquineta])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_qtde" = ""
	Then Erro "Indique a quantidade de parcelas!!"

Scenario: c_pc_maquineta_qtde inválida
	#loja/PedidoNovoConsiste.asp
	#n=converte_numero(f.c_pc_maquineta_qtde.value);
	#if (n < 1) {
	#		alert('Quantidade de parcelas inválida!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pc_maquineta_qtde) < 1 then
	#	alerta = "Quantidade de parcelas inválida (parcelado no cartão [maquineta])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_qtde" = "0"
	Then Erro "Quantidade de parcelas inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_qtde" = "-1"
	Then Erro "Quantidade de parcelas inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_qtde" = "2"
	Then Sem Erro "Quantidade de parcelas inválida!!"

Scenario: c_pc_maquineta_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pc_maquineta_valor.value)=='') {
	#		alert('Indique o valor da parcela!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pc_maquineta_valor = "" then
	#	alerta = "Indique o valor da parcela (parcelado no cartão [maquineta])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_valor" = ""
	Then Erro "Indique o valor da parcela!!"

Scenario: c_pc_maquineta_valor invalido
	#loja/PedidoNovoConsiste.asp
	#vp=converte_numero(f.c_pc_maquineta_valor.value);
	#if (vp<=0) {
	#		alert('Valor de parcela inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pc_maquineta_valor) <= 0 then
	#	alerta = "Valor de parcela inválido (parcelado no cartão [maquineta])."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_valor" = "0"
	Then Erro "Valor de parcela inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_valor" = "-1"
	Then Erro "Valor de parcela inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_valor" = "2"
	Then Sem erro "Valor de parcela inválido!!"

#//	Parcelado com entrada
Scenario: op_pce_entrada_forma_pagto
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.op_pce_entrada_forma_pagto.value)=='') {
	#		alert('Indique a forma de pagamento da entrada!!');
	#loja/PedidoNovoConfirma.asp
	#if op_pce_entrada_forma_pagto = "" then
	#	alerta = "Indique a forma de pagamento da entrada (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "op_pce_entrada_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento da entrada!!"

Scenario: c_pce_entrada_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pce_entrada_valor.value)=='') {
	#		alert('Indique o valor da entrada!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pce_entrada_valor = "" then
	#	alerta = "Indique o valor da entrada (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = ""
	Then Erro "Indique o valor da entrada!!"

Scenario: c_pce_entrada_valor inválida
	#loja/PedidoNovoConsiste.asp
	#ve=converte_numero(f.c_pce_entrada_valor.value);
	#if (ve<=0) {
	#		alert('Valor da entrada inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pce_entrada_valor) <= 0 then
	#	alerta = "Valor da entrada inválido (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = "0"
	Then Erro "Valor da entrada inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = "-1"
	Then Erro "Valor da entrada inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = "2"
	Then Sem Erro "Valor da entrada inválido!!"

Scenario: op_pce_prestacao_forma_pagto
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.op_pce_prestacao_forma_pagto.value)=='') {
	#		alert('Indique a forma de pagamento das prestações!!');
	#loja/PedidoNovoConfirma.asp
	#elseif op_pce_prestacao_forma_pagto = "" then
	#	alerta = "Indique a forma de pagamento das prestações (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "op_pce_prestacao_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento das prestações!!"

Scenario: c_pce_prestacao_qtde
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pce_prestacao_qtde.value)=='') {
	#		alert('Indique a quantidade de prestações!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pce_prestacao_qtde = "" then
	#	alerta = "Indique a quantidade de prestações (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_qtde" = ""
	Then Erro "Indique a quantidade de prestações!!"

Scenario: c_pce_prestacao_qtde inválida
	#loja/PedidoNovoConsiste.asp
	#n=converte_numero(f.c_pce_prestacao_qtde.value);
	#if (n<=0) {
	#		alert('Quantidade de prestações inválida!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pce_prestacao_qtde) <= 0 then
	#	alerta = "Quantidade de prestações inválida (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_qtde" = "0"
	Then Erro "Quantidade de prestações inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_qtde" = "-1"
	Then Erro "Quantidade de prestações inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_qtde" = "2"
	Then Sem Erro "Quantidade de prestações inválida!!"

Scenario: c_pce_prestacao_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pce_prestacao_valor.value)=='') {
	#		alert('Indique o valor da prestação!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pce_prestacao_valor = "" then
	#	alerta = "Indique o valor da prestação (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_valor" = ""
	Then Erro "Indique o valor da prestação!!"


Scenario: c_pce_prestacao_valor inválida
	#loja/PedidoNovoConsiste.asp
	#vp=converte_numero(f.c_pce_prestacao_valor.value);
	#if (vp<=0) {
	#		alert('Valor de prestação inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pce_prestacao_valor) <= 0 then
	#	alerta = "Valor de prestação inválido (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_valor" = "0"
	Then Erro "Valor de prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_valor" = "-1"
	Then Erro "Valor de prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_valor" = "2"
	Then Sem Erro "Valor de prestação inválido!!"

Scenario: c_pce_prestacao_periodo
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pce_prestacao_periodo.value)=='') {
	#		alert('Indique o intervalo de vencimento entre as parcelas!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pce_prestacao_periodo = "" then
	#	alerta = "Indique o intervalo de vencimento entre as parcelas (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_periodo" = ""
	Then Erro "Indique o intervalo de vencimento entre as parcelas!!"


Scenario: c_pce_prestacao_periodo inválida
	#loja/PedidoNovoConsiste.asp
	#ni=converte_numero(f.c_pce_prestacao_periodo.value);
	#if (ni<=0) {
	#		alert('Intervalo de vencimento inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pce_prestacao_periodo) <= 0 then
	#	alerta = "Intervalo de vencimento inválido (parcelado com entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_periodo" = "0"
	Then Erro "Intervalo de vencimento inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_periodo" = "-1"
	Then Erro "Intervalo de vencimento inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "c_pce_prestacao_periodo" = "2"
	Then Sem Erro "Intervalo de vencimento inválido!!"


#//	Parcelado sem entrada
Scenario: op_pse_prim_prest_forma_pagto
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.op_pse_prim_prest_forma_pagto.value)=='') {
	#		alert('Indique a forma de pagamento da 1ª prestação!!');
	#loja/PedidoNovoConfirma.asp
	#if op_pse_prim_prest_forma_pagto = "" then
	#	alerta = "Indique a forma de pagamento da 1ª prestação (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "op_pse_prim_prest_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento da 1ª prestação!!"

Scenario: c_pse_prim_prest_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pse_prim_prest_valor.value)=='') {
	#		alert('Indique o valor da 1ª prestação!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pse_prim_prest_valor = "" then
	#	alerta = "Indique o valor da 1ª prestação (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = ""
	Then Erro "Indique o valor da 1ª prestação!!"

Scenario: c_pse_prim_prest_valor inválida
	#loja/PedidoNovoConsiste.asp
	#ve=converte_numero(f.c_pse_prim_prest_valor.value);
	#if (ve<=0) {
	#		alert('Valor da 1ª prestação inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pse_prim_prest_valor) <= 0 then
	#	alerta = "Valor da 1ª prestação inválido (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = "0"
	Then Erro "Valor da 1ª prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = "-1"
	Then Erro "Valor da 1ª prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = "2"
	Then Sem Erro "Valor da 1ª prestação inválido!!"

Scenario: c_pse_prim_prest_apos
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pse_prim_prest_apos.value)=='') {
	#		alert('Indique o intervalo de vencimento da 1ª parcela!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pse_prim_prest_apos = "" then
	#	alerta = "Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_apos" = ""
	Then Erro "Indique o intervalo de vencimento da 1ª parcela!!"

Scenario: c_pse_prim_prest_apos inválida
	#loja/PedidoNovoConsiste.asp
	#nip=converte_numero(f.c_pse_prim_prest_apos.value);
	#if (nip<=0) {
	#		alert('Intervalo de vencimento da 1ª parcela é inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pse_prim_prest_apos) <= 0 then
	#	alerta = "Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_apos" = "0"
	Then Erro "Intervalo de vencimento da 1ª parcela é inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_apos" = "-1"
	Then Erro "Intervalo de vencimento da 1ª parcela é inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_apos" = "2"
	Then Sem Erro "Intervalo de vencimento da 1ª parcela é inválido!!"

Scenario: op_pse_demais_prest_forma_pagto
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.op_pse_demais_prest_forma_pagto.value)=='') {
	#		alert('Indique a forma de pagamento das demais prestações!!');
	#loja/PedidoNovoConfirma.asp
	#elseif op_pse_demais_prest_forma_pagto = "" then
	#	alerta = "Indique a forma de pagamento das demais prestações (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "op_pse_demais_prest_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento das demais prestações!!"

Scenario: c_pse_demais_prest_qtde
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pse_demais_prest_qtde.value)=='') {
	#		alert('Indique a quantidade das demais prestações!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pse_demais_prest_qtde = "" then
	#	alerta = "Indique a quantidade das demais prestações (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_qtde" = ""
	Then Erro "Indique a quantidade das demais prestações!!"

Scenario: c_pse_demais_prest_qtde c_pse_demais_prest_qtde
	#loja/PedidoNovoConsiste.asp
	#n=converte_numero(f.c_pse_demais_prest_qtde.value);
	#if (n<=0) {
	#		alert('Quantidade de prestações inválida!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pse_demais_prest_qtde) <= 0 then
	#	alerta = "Quantidade de prestações inválida (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_qtde" = "0"
	Then Erro "Quantidade de prestações inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_qtde" = "-1"
	Then Erro "Quantidade de prestações inválida!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_qtde" = "2"
	Then Sem Erro "Quantidade de prestações inválida!!"

Scenario: c_pse_demais_prest_valor
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pse_demais_prest_valor.value)=='') {
	#		alert('Indique o valor das demais prestações!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pse_demais_prest_valor = "" then
	#	alerta = "Indique o valor das demais prestações (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_valor" = ""
	Then Erro "Indique o valor das demais prestações!!"

Scenario: c_pse_demais_prest_valor inválido
	#loja/PedidoNovoConsiste.asp
	#vp=converte_numero(f.c_pse_demais_prest_valor.value);
	#if (vp<=0) {
	#		alert('Valor de prestação inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pse_demais_prest_valor) <= 0 then
	#	alerta = "Valor de prestação inválido (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_valor" = "0"
	Then Erro "Valor de prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_valor" = "-1"
	Then Erro "Valor de prestação inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_valor" = "2"
	Then Sem Erro "Valor de prestação inválido!!"

Scenario: c_pse_demais_prest_periodo
	#loja/PedidoNovoConsiste.asp
	#if (trim(f.c_pse_demais_prest_periodo.value)=='') {
	#		alert('Indique o intervalo de vencimento entre as parcelas!!');
	#loja/PedidoNovoConfirma.asp
	#elseif c_pse_demais_prest_periodo = "" then
	#	alerta = "Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_periodo" = ""
	Then Erro "Indique o intervalo de vencimento entre as parcelas!!"

Scenario: c_pse_demais_prest_periodo inválido
	#loja/PedidoNovoConsiste.asp
	#ni=converte_numero(f.c_pse_demais_prest_periodo.value);
	#if (ni<=0) {
	#		alert('Intervalo de vencimento inválido!!');
	#loja/PedidoNovoConfirma.asp
	#elseif converte_numero(c_pse_demais_prest_periodo) <= 0 then
	#	alerta = "Intervalo de vencimento inválido (parcelado sem entrada)."
	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_periodo" = "0"
	Then Erro "Intervalo de vencimento inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_periodo" = "-1"
	Then Erro "Intervalo de vencimento inválido!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_demais_prest_periodo" = "2"
	Then Sem Erro "Intervalo de vencimento inválido!!"

Scenario: forma_pagto invalida
	#loja/PedidoNovoConsiste.asp
	#// Nenhuma forma de pagamento foi escolhida
	#alert('Indique a forma de pagamento!!');
	#loja/PedidoNovoConfirma.asp
	#alerta = "É obrigatório especificar a forma de pagamento"
	Given Pedido base
	When Informo "tipo_parcelamento" = ""
	Then Erro "Indique a forma de pagamento!!"

	Given Pedido base
	When Informo "tipo_parcelamento" = "XX"
	Then Erro "Indique a forma de pagamento!!"

