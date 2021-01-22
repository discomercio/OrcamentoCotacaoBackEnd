@Especificacao.Pedido.Passo40
Feature: FormaPagamentoPreenchimento_COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA

Background: não executado na api magento
	#ignoramos na API magento porque é feito em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\FormaPagtoCriacaoMagentoDto.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

	#está com muitas diferenças....
	Given Ignorar cenário no ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi"

	#para testar, afazer, voltar isto para testar o pedido!!!
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"

@ignore
Scenario: tirar esta linha acima!!! não está estando na loja!!
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"


Scenario: Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
	When Informo "c_pse_prim_prest_valor" = "10"
	When Informo "c_pse_demais_prest_qtde" = "10"
	When Informo "c_pse_demais_prest_valor" = "10"
	And Informo "vl_total_NF" = "2"
	#nao aceita esse tipo de parcelamento
	#por isso, este arquivo está todo comentado
	Then Erro "Forma de pagamento não aceita para esse indicador."

##//	Parcelado sem entrada
#Scenario: op_pse_prim_prest_forma_pagto
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.op_pse_prim_prest_forma_pagto.value)=='') {
#	#		alert('Indique a forma de pagamento da 1ª prestação!!');
#	#loja/PedidoNovoConfirma.asp
#	#if op_pse_prim_prest_forma_pagto = "" then
#	#	alerta = "Indique a forma de pagamento da 1ª prestação (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "op_pse_prim_prest_forma_pagto" = ""
#	Then Erro "Indique a forma de pagamento da 1ª prestação!!"
#
#Scenario: c_pse_prim_prest_valor
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.c_pse_prim_prest_valor.value)=='') {
#	#		alert('Indique o valor da 1ª prestação!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif c_pse_prim_prest_valor = "" then
#	#	alerta = "Indique o valor da 1ª prestação (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_valor" = ""
#	Then Erro "Indique o valor da 1ª prestação!!"
#
#Scenario: c_pse_prim_prest_valor inválida
#	#loja/PedidoNovoConsiste.asp
#	#ve=converte_numero(f.c_pse_prim_prest_valor.value);
#	#if (ve<=0) {
#	#		alert('Valor da 1ª prestação inválido!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif converte_numero(c_pse_prim_prest_valor) <= 0 then
#	#	alerta = "Valor da 1ª prestação inválido (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_valor" = "0"
#	Then Erro "Valor da 1ª prestação inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_valor" = "-1"
#	Then Erro "Valor da 1ª prestação inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_valor" = "2"
#	Then Sem Erro "Valor da 1ª prestação inválido!!"
#
#Scenario: c_pse_prim_prest_apos
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.c_pse_prim_prest_apos.value)=='') {
#	#		alert('Indique o intervalo de vencimento da 1ª parcela!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif c_pse_prim_prest_apos = "" then
#	#	alerta = "Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_apos" = ""
#	Then Erro "Indique o intervalo de vencimento da 1ª parcela!!"
#
#Scenario: c_pse_prim_prest_apos inválida
#	#loja/PedidoNovoConsiste.asp
#	#nip=converte_numero(f.c_pse_prim_prest_apos.value);
#	#if (nip<=0) {
#	#		alert('Intervalo de vencimento da 1ª parcela é inválido!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif converte_numero(c_pse_prim_prest_apos) <= 0 then
#	#	alerta = "Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_apos" = "0"
#	Then Erro "Intervalo de vencimento da 1ª parcela é inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_apos" = "-1"
#	Then Erro "Intervalo de vencimento da 1ª parcela é inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_prim_prest_apos" = "2"
#	Then Sem Erro "Intervalo de vencimento da 1ª parcela é inválido!!"
#
#Scenario: op_pse_demais_prest_forma_pagto
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.op_pse_demais_prest_forma_pagto.value)=='') {
#	#		alert('Indique a forma de pagamento das demais prestações!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif op_pse_demais_prest_forma_pagto = "" then
#	#	alerta = "Indique a forma de pagamento das demais prestações (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "op_pse_demais_prest_forma_pagto" = ""
#	Then Erro "Indique a forma de pagamento das demais prestações!!"
#
#Scenario: c_pse_demais_prest_qtde
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.c_pse_demais_prest_qtde.value)=='') {
#	#		alert('Indique a quantidade das demais prestações!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif c_pse_demais_prest_qtde = "" then
#	#	alerta = "Indique a quantidade das demais prestações (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_qtde" = ""
#	Then Erro "Indique a quantidade das demais prestações!!"
#
#Scenario: c_pse_demais_prest_qtde c_pse_demais_prest_qtde
#	#loja/PedidoNovoConsiste.asp
#	#n=converte_numero(f.c_pse_demais_prest_qtde.value);
#	#if (n<=0) {
#	#		alert('Quantidade de prestações inválida!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif converte_numero(c_pse_demais_prest_qtde) <= 0 then
#	#	alerta = "Quantidade de prestações inválida (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_qtde" = "0"
#	Then Erro "Quantidade de prestações inválida!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_qtde" = "-1"
#	Then Erro "Quantidade de prestações inválida!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_qtde" = "2"
#	Then Sem Erro "Quantidade de prestações inválida!!"
#
#Scenario: c_pse_demais_prest_valor
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.c_pse_demais_prest_valor.value)=='') {
#	#		alert('Indique o valor das demais prestações!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif c_pse_demais_prest_valor = "" then
#	#	alerta = "Indique o valor das demais prestações (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_valor" = ""
#	Then Erro "Indique o valor das demais prestações!!"
#
#Scenario: c_pse_demais_prest_valor inválido
#	#loja/PedidoNovoConsiste.asp
#	#vp=converte_numero(f.c_pse_demais_prest_valor.value);
#	#if (vp<=0) {
#	#		alert('Valor de prestação inválido!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif converte_numero(c_pse_demais_prest_valor) <= 0 then
#	#	alerta = "Valor de prestação inválido (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_valor" = "0"
#	Then Erro "Valor de prestação inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_valor" = "-1"
#	Then Erro "Valor de prestação inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_valor" = "2"
#	Then Sem Erro "Valor de prestação inválido!!"
#
#Scenario: c_pse_demais_prest_periodo
#	#loja/PedidoNovoConsiste.asp
#	#if (trim(f.c_pse_demais_prest_periodo.value)=='') {
#	#		alert('Indique o intervalo de vencimento entre as parcelas!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif c_pse_demais_prest_periodo = "" then
#	#	alerta = "Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_periodo" = ""
#	Then Erro "Indique o intervalo de vencimento entre as parcelas!!"
#
#Scenario: c_pse_demais_prest_periodo inválido
#	#loja/PedidoNovoConsiste.asp
#	#ni=converte_numero(f.c_pse_demais_prest_periodo.value);
#	#if (ni<=0) {
#	#		alert('Intervalo de vencimento inválido!!');
#	#loja/PedidoNovoConfirma.asp
#	#elseif converte_numero(c_pse_demais_prest_periodo) <= 0 then
#	#	alerta = "Intervalo de vencimento inválido (parcelado sem entrada)."
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_periodo" = "0"
#	Then Erro "Intervalo de vencimento inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_periodo" = "-1"
#	Then Erro "Intervalo de vencimento inválido!!"
#
#	Given Pedido base
#	When Informo "tipo_parcelamento" = "#COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA"
#	When Informo "c_pse_demais_prest_periodo" = "2"
#	Then Sem Erro "Intervalo de vencimento inválido!!"
#
