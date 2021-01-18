Feature: Tipo_Parcelamento
#Pedido que vier do Markeplace deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"  
#e Op_pu_forma_pagto = "2" "Depósito" e C_pu_vencto_apos = 30 dias (Definido no appsettings)
#Pedido que vier do Magento deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1" 
#e Op_av_forma_pagto = "6" "Boleto" ou Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2" 
#(Definido no appsettings)
#Tipo_Parcelamento:
#    COD_FORMA_PAGTO_A_VISTA = "1",
#    COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
#    COD_FORMA_PAGTO_PARCELA_UNICA = "5",


#esse teste esta sendo feito em "FormaPagtoCriacaoMagento.feature"
@ignore
Scenario: Tipo_Parcelamento
	Given fazere sta validação
