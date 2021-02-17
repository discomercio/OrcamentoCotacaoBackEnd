@Especificacao.Pedido.PedidoFaltandoImplementarSteps
#@Especificacao.Pedido.Passo40
Feature: FormaPagamentoDivergenciaTotal

Background: não executado na api magento
	#ignoramos na API magento porque é feito em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\FormaPagtoCriacaoMagento.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	#no CadastrarPrepedidoPrepedidoApi parece que a ordem de validação é diferente
	Given No ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi" mapear erro "Valor total da forma de pagamento diferente do valor total!" para "regex Tipo do parcelamento .* está incorreto!"
	#está com muitas diferenças....
	Given Ignorar cenário no ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi"
	#para testar, afazer, voltar isto para testar o pedido!!!
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"


#documentação auxiliar:
#Const COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA = "CE"
#Const COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA = "SE"
#Const COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA = "AV"


Scenario: COD_FORMA_PAGTO_A_VISTA - divergência entre o valor total do pedido
	#loja/PedidoNovoConfirma.asp
	#if rb_forma_pagto = COD_FORMA_PAGTO_A_VISTA then vlTotalFormaPagto = vl_total_NF
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	#este erro nunca vai acontecer porque ele deixa as duas variáveis iguais e depois testa se estão diferentes
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "vl_total_NF" = "1"
	And Informo "vl_total_NF" = "2"
#este erro nunca vai acontecer porque ele deixa as duas variáveis iguais e depois testa se estão diferentes
#Then Erro "regex .*Há divergência entre o valor total do pedido.*"


Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - somente para PJ
	#loja/PedidoNovoConsiste.asp
	#<% if EndCob_tipo_pessoa = ID_PF then %>
	#if (($("#c_loja").val() != NUMERO_LOJA_ECOMMERCE_AR_CLUBE) && (!FLAG_MAGENTO_PEDIDO_COM_INDICADOR)) $(".TR_FP_PU").hide();
	#$(".TR_FP_PSE").hide();
	#<% end if %>
	Given Pedido base cliente PF
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "FormaPagtoCriacao.Op_pu_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pu_vencto_apos" = "1"
	Then Erro "Forma de pagamento não aceita para esse indicador."

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - divergência entre o valor total do pedido
	#ignoramos na API magneto porque não temos todos os campos
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
#loja/PedidoNovoConsiste.asp
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
#		f.c_pu_valor.focus();

#vtNF=fp_vl_total_pedido(); --> total do pedido
#ve=converte_numero(f.c_pu_valor.value);
#vtFP=ve;

#loja/PedidoNovoConfirma.asp
#	if alerta = "" then vlTotalFormaPagto = converte_numero(c_pu_valor)
#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"

	#COD_FORMA_PAGTO_PARCELA_UNICA somente para PJ
	Given Pedido base cliente PJ
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "FormaPagtoCriacao.Op_pu_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pu_vencto_apos" = "1"
	When Informo "FormaPagtoCriacao.c_pu_valor" = "1"
	And Informo "vl_total_NF" = "2"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - Parcelado no cartão (internet) divergência
#loja/PedidoNovoConsiste.asp
#vtFP=n*vp;
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');

	#loja/PedidoNovoConfirma.asp
	#if alerta = "" then vlTotalFormaPagto = converte_numero(c_pc_qtde) * converte_numero(c_pc_valor)
	#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
	#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "c_pc_qtde" = "10"
	When Informo "c_pc_valor" = "10"
	And Informo "vl_total_NF" = "2"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

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
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "c_pc_maquineta_qtde" = "10"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "10"
	When Informo "c_pc_maquineta_valor" = "10"
	And Informo "vl_total_NF" = "2"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

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
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = "10"
	When Informo "c_pce_prestacao_qtde" = "10"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "11"
	When Informo "c_pce_prestacao_valor" = "10"
	When Informo "Op_pce_entrada_forma_pagto" = "1"
	When Informo "Op_pce_prestacao_forma_pagto" = "1"
	When Informo "C_pce_prestacao_periodo" = "1"
	And Informo "vl_total_NF" = "2"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "c_pce_entrada_valor" = "10"
	When Informo "c_pce_prestacao_qtde" = "10"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "11"
	When Informo "c_pce_prestacao_valor" = "10"
	When Informo "Op_pce_entrada_forma_pagto" = "1"
	When Informo "Op_pce_prestacao_forma_pagto" = "1"
	When Informo "C_pce_prestacao_periodo" = "1"
	And Informo "vl_total_NF" = "110"
	Then Sem erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA - Parcelado sem entrada divergência

#esta opção está desbilitada para PF e PJ
#loja/PedidoNovoConsiste.asp
#<% if EndCob_tipo_pessoa = ID_PF then %>
#if (($("#c_loja").val() != NUMERO_LOJA_ECOMMERCE_AR_CLUBE) && (!FLAG_MAGENTO_PEDIDO_COM_INDICADOR)) $(".TR_FP_PU").hide();
#$(".TR_FP_PSE").hide();
#<% end if %>
#<% if EndCob_tipo_pessoa = ID_PJ then %>
#$(".TR_FP_PSE").hide();
#<% end if %>


#loja/PedidoNovoConsiste.asp
#vtFP=ve+(n*vp);
#if (Math.abs(vtFP-vtNF)>MAX_ERRO_ARREDONDAMENTO) {
#		alert('Há divergência entre o valor total do pedido (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtNF) + ') e o valor total descrito através da forma de pagamento (' + SIMBOLO_MONETARIO + ' ' + formata_moeda(vtFP) + ')!!');
#		f.c_pse_demais_prest_valor.focus();
#loja/PedidoNovoConfirma.asp
#vlTotalFormaPagto = converte_numero(c_pse_prim_prest_valor) + (converte_numero(c_pse_demais_prest_qtde) * converte_numero(c_pse_demais_prest_valor))
#if Abs(vlTotalFormaPagto-vl_total_NF) > 0.1 then
#	alerta = "Há divergência entre o valor total do pedido (" & SIMBOLO_MONETARIO & " " & formata_moeda(vl_total_NF) & ") e o valor total descrito através da forma de pagamento (" & SIMBOLO_MONETARIO & " " & formata_moeda(vlTotalFormaPagto) & ")!!"

	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = "10"
	When Informo "c_pse_demais_prest_qtde" = "10"
	When Informo "c_pse_demais_prest_valor" = "10"
	And Informo "vl_total_NF" = "2"
	#nao aceita esse tipo de parcelamento
	Then Erro "Forma de pagamento não aceita para esse indicador."

Scenario: TipoParcelamento e QtdeParcelas
#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia
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

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_A_VISTA 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_A_VISTA 1
	#	if rb_forma_pagto=COD_FORMA_PAGTO_A_VISTA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA
	#		c_custoFinancFornecQtdeParcelasConferencia="0"
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	#o proximo erro da lista.... qualquer um serve que não tenha a ver com o TipoParcelamento
	Then Erro "Coeficiente do fabricante (003) está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_A_VISTA 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_A_VISTA 2
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "1"
	Then Erro "Quantidade da parcela esta divergente!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_A_VISTA 3
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_A_VISTA 3
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'CE') está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_A_VISTA 4
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_A_VISTA"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'SE') está incorreto!"


Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELA_UNICA 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELA_UNICA 1
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELA_UNICA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia="1"
	Given Pedido base cliente PJ
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "FormaPagtoCriacao.Op_pu_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pu_vencto_apos" = "1"
	When Informo "FormaPagtoCriacao.C_pu_valor" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "1"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELA_UNICA 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELA_UNICA 2
	Given Pedido base cliente PJ
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "FormaPagtoCriacao.Op_pu_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pu_vencto_apos" = "1"
	When Informo "FormaPagtoCriacao.C_pu_valor" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "2"
	Then Erro "Quantidade da parcela esta divergente!"
Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELA_UNICA 3
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELA_UNICA 3
	Given Pedido base cliente PJ
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "FormaPagtoCriacao.Op_pu_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pu_vencto_apos" = "1"
	When Informo "FormaPagtoCriacao.C_pu_valor" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "1"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'CE') está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELADO_CARTAO 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_CARTAO 1
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_CARTAO then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pc_qtde
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_qtde" = "3"
	When Informo "FormaPagtoCriacao.C_pc_valor" = "3"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELADO_CARTAO 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_CARTAO 2
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_qtde" = "4"
	Then Erro "Quantidade de parcelas esta divergente!"
Scenario: TipoParcelamento e QtdeParcelas - COD_FORMA_PAGTO_PARCELADO_CARTAO 3
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_qtde" = "3"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'CE') está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA 1
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pc_maquineta_qtde
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_qtde" = "3"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_valor" = "3"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA 2
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_qtde" = "4"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_valor" = "3"
	Then Erro "Quantidade de parcelas esta divergente!"
Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA 3
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_qtde" = "3"
	When Informo "FormaPagtoCriacao.C_pc_maquineta_valor" = "3"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'CE') está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 1
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=c_pce_prestacao_qtde
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "FormaPagtoCriacao.Op_pce_entrada_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.Op_pce_prestacao_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_entrada_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_periodo" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_qtde" = "3"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 2
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "FormaPagtoCriacao.Op_pce_entrada_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.Op_pce_prestacao_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_entrada_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_periodo" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_qtde" = "4"
	Then Erro "Quantidade de parcelas esta divergente!"
Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 3
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA 3
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA"
	When Informo "FormaPagtoCriacao.Op_pce_entrada_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.Op_pce_prestacao_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_entrada_valor" = "1"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_periodo" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pce_prestacao_qtde" = "3"
	Then Erro "Tipo do parcelamento (CustoFinancFornecTipoParcelamento 'SE') está incorreto!"

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 1
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 1
	#	elseif rb_forma_pagto=COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA then
	#		c_custoFinancFornecTipoParcelamentoConferencia=COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
	#		c_custoFinancFornecQtdeParcelasConferencia=Cstr(converte_numero(c_pse_demais_prest_qtde)+1)
	#	else
	#		c_custoFinancFornecTipoParcelamentoConferencia=""
	#		c_custoFinancFornecQtdeParcelasConferencia="0"
	#		end if
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pse_demais_prest_qtde" = "4"
	#Then Sem nenhum erro
	#nao aceita esse tipo de parcelamento
	Then Erro "Forma de pagamento não aceita para esse indicador."

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 2
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 2
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pse_demais_prest_qtde" = "5"
	#Then Erro "regex .*Foi detectada uma inconsistência na quantidade de parcelas de pagamento.*"
	#nao aceita esse tipo de parcelamento
	Then Erro "Forma de pagamento não aceita para esse indicador."

Scenario: TipoParcelamento e QtdeParcelas- COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 3
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA 3
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "3"
	When Informo "FormaPagtoCriacao.C_pse_demais_prest_qtde" = "4"
	#Then Erro "regex .*Foi detectada uma inconsistência no tipo de parcelamento do pagamento.*"
	#nao aceita esse tipo de parcelamento
	Then Erro "Forma de pagamento não aceita para esse indicador."

Scenario: TipoParcelamento e QtdeParcelas- outros
	#c_custoFinancFornecTipoParcelamento e c_custoFinancFornecQtdeParcelasConferencia - outros
	#	else
	#		c_custoFinancFornecTipoParcelamentoConferencia=""
	#		c_custoFinancFornecQtdeParcelasConferencia="0"
	#		end if
	#nao temos como testar este, vai ser uma rb_forma_pagto inválida!
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO inválido"
	Then Erro "Tipo do parcelamento inválido"

