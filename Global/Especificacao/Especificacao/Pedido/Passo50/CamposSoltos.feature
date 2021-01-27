@ignore
Feature: CamposSoltos

#var MAX_TAM_OBS1 = 500;
#var MAX_TAM_NF_TEXTO = 800;
#var MAX_TAM_FORMA_PAGTO = 250;

Scenario: obs1 tamanho
	#loja/PedidoNovoConsiste.asp
	#s = "" + f.c_obs1.value;
	#if (s.length > MAX_TAM_OBS1) {
	#	alert('Conteúdo de "Observações " excede em ' + (s.length-MAX_TAM_OBS1) + ' caracteres o tamanho máximo de ' + MAX_TAM_OBS1 + '!!');
	Given Pedido base
	When Informo "obs1" = um texto com "501" caracteres
	Then Erro "regex .*Conteúdo de \"Observações \" excede em .*"

	Given Pedido base
	When Informo "obs1" = um texto com "500" caracteres
	Then Sem nenhum erro

Scenario: c_nf_texto tamanho
	#loja/PedidoNovoConsiste.asp
	#s = "" + f.c_nf_texto.value;
	#if (s.length > MAX_TAM_NF_TEXTO) {
	#    alert('Conteúdo de "Constar na NF" excede em ' + (s.length-MAX_TAM_NF_TEXTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_NF_TEXTO + '!!');
	Given Pedido base
	When Informo "nf_texto" = um texto com "801" caracteres
	Then Erro regex "Conteúdo de "Constar na NF" excede em.*"

	Given Pedido base
	When Informo "nf_texto" = um texto com "800" caracteres
	Then Sem nenhum erro

Scenario: forma_pagto tamanho
	#loja/PedidoNovoConsiste.asp
	#s = "" + f.c_forma_pagto.value;
	#if (s.length > MAX_TAM_FORMA_PAGTO) {
	#	alert('Conteúdo de "Forma de Pagamento" excede em ' + (s.length-MAX_TAM_FORMA_PAGTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_FORMA_PAGTO + '!!');
	Given Pedido base
	When Informo "forma_pagto" = um texto com "251" caracteres
	Then Erro regex "Conteúdo de \"Forma de Pagamento\" excede em .*"

	Given Pedido base
	When Informo "forma_pagto" = um texto com "250" caracteres
	Then Sem nenhum erro


Scenario: bem_uso_consumo
	#loja/PedidoNovoConsiste.asp
	#if (!blnFlag) {
	#	alert('Informe se é "Bem de Uso/Consumo"');
	Given Pedido base
	When Informo "bem_uso_consumo" = "99"
	Then Erro "Informe se é "Bem de Uso/Consumo""

	Given Pedido base
	When Informo "bem_uso_consumo" = "COD_ST_BEM_USO_CONSUMO_NAO_DEFINIDO"
	Then Erro "Informe se é "Bem de Uso/Consumo""

	Given Pedido base
	When Informo "bem_uso_consumo" = "COD_ST_BEM_USO_CONSUMO_NAO"
	Then Sem nenhum erro

Scenario: instalador_instala
	#loja/PedidoNovoConsiste.asp
	#alert('Preencha o campo "Instalador Instala"');
	Given Pedido base
	When Informo "instalador_instala" = "99"
	Then Erro "Preencha o campo "Instalador Instala""

	Given Pedido base
	When Informo "instalador_instala" = "COD_INSTALADOR_INSTALA_NAO_DEFINIDO"
	Then Erro "Preencha o campo "Instalador Instala""

	Given Pedido base
	When Informo "instalador_instala" = "COD_INSTALADOR_INSTALA_NAO"
	Then Sem nenhum erro

Scenario: garantia_indicador
	#loja/PedidoNovoConsiste.asp
	#alert('Preencha o campo "Garantia Indicador"');
	Given Pedido base
	When Informo "garantia_indicador" = "99"
	Then Erro "Preencha o campo "Garantia Indicador""

	Given Pedido base
	When Informo "garantia_indicador" = "COD_GARANTIA_INDICADOR_STATUS__NAO_DEFINIDO"
	Then Erro "Preencha o campo "Garantia Indicador""

	Given Pedido base
	When Informo "garantia_indicador" = "COD_GARANTIA_INDICADOR_STATUS__SIM"
	Then Sem nenhum erro

Scenario: c_indicador c_perc_RT rb_RA garantia_indicador somente se for indicacao
#loja/PedidoNovoConfirma.asp
#if rb_indicacao = "S" then
#	c_indicador = Trim(Request.Form("c_indicador"))
#	c_perc_RT = Trim(Request.Form("c_perc_RT"))
#	rb_RA = Trim(Request.Form("rb_RA"))
#	rb_garantia_indicador = Trim(Request.Form("rb_garantia_indicador"))
#else
#	c_indicador = ""
#	c_perc_RT = ""
#	rb_RA = ""
#	rb_garantia_indicador = COD_GARANTIA_INDICADOR_STATUS__NAO
#	end if

	Given Pedido base
	When Informo "indicador" = "ZEZINHO"
	And Informo "indicacao" = "N"
	Then No pedido gravado, verificar campo "indicador" = ""

	Given Pedido base
	When Informo "perc_RT" = "123"
	And Informo "indicacao" = "N"
	Then No pedido gravado, verificar campo "perc_RT" = ""

	Given Pedido base
	When Informo "RA" = "True"
	And Informo "indicacao" = "N"
	Then No pedido gravado, verificar campo "RA" = "false"

	Given Pedido base
	When Informo "garantia_indicador" = "COD_GARANTIA_INDICADOR_STATUS__SIM"
	And Informo "indicacao" = "N"
	Then No pedido gravado, verificar campo "garantia_indicador" = "COD_GARANTIA_INDICADOR_STATUS__NAO"

