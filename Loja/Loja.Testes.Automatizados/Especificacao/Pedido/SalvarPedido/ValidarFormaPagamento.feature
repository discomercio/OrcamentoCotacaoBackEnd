@tag validarformapagamento
Feature: Validar forma de apgamento

Scenario: Validar forma de apgamento
	Given Pedido base
	#Tipo_Parcelamento: COD_FORMA_PAGTO_A_VISTA = "1"
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "1"
	And Informo "FormaPagtoCriacao.Op_av_forma_pagto" = ""
	Then Erro "Indique a forma de pagamento (à vista)."

