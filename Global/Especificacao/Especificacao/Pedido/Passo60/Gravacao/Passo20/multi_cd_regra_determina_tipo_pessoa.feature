@ignore
@Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pessoa
Feature: multi_cd_regra_determina_tipo_pessoa

#BDD.asp
#function multi_cd_regra_determina_tipo_pessoa(byval tipo_cliente, byval contribuinte_icms_status, byval produtor_rural_status)
#dim tipo_pessoa
#	tipo_pessoa = ""
#
#	if tipo_cliente = ID_PF then
#		if converte_numero(produtor_rural_status) = converte_numero(COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) then
#			tipo_pessoa = COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL
#		elseif converte_numero(produtor_rural_status) = converte_numero(COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) then
#			tipo_pessoa = COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA
#			end if
#	elseif tipo_cliente = ID_PJ then
#		if converte_numero(contribuinte_icms_status) = converte_numero(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) then
#			tipo_pessoa = COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE
#		elseif converte_numero(contribuinte_icms_status) = converte_numero(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO) then
#			tipo_pessoa = COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE
#		elseif converte_numero(contribuinte_icms_status) = converte_numero(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO) then
#			tipo_pessoa = COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO
#			end if
#		end if
#
#	multi_cd_regra_determina_tipo_pessoa = tipo_pessoa
#end function
#temos que testar a rotina UtilsProduto.MultiCdRegraDeterminaPessoa
#se houver alugma outra versão dela, a que está sendo usada no produto é essa.
Scenario Outline: Testar multi_cd_regra_determina_tipo_pessoa
	Then Chamar rotina MULTI_CD_REGRA_DETERMINA_TIPO_PESSOA tipo cliente = "<tipo_cliente>", contribuinte = "<contribuinte_icms_status>", produtor rural = "<produtor_rural_status>" e resultado = "<resultado>"

	#criar um steps.cs dentro do mesmo diretório do feature
	Examples:
		| tipo_cliente | contribuinte_icms_status                | produtor_rural_status             | resultado                                                             |
		| ID_PF        |                                         | COD_ST_CLIENTE_PRODUTOR_RURAL_SIM | COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL                   |
		| ID_PF        |                                         | COD_ST_CLIENTE_PRODUTOR_RURAL_NAO | COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA                    |
		| ID_PJ        | COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM    |                                   | COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE     |
		| ID_PJ        | COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO    |                                   | COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE |
		| ID_PJ        | COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO |                                   | COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO           |
#descrevndo os códigos:
#
#  ' CÓDIGOS QUE IDENTIFICAM SE É PESSOA FÍSICA OU JURÍDICA
#	Const ID_PF = "PF"
#	Const ID_PJ = "PJ"
#
#  ' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO CONTRIBUINTE DO ICMS
#	Const COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = "0"
#	Const COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = "1"
#	Const COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = "2"
#	Const COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = "3"
#
#  ' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO PRODUTOR RURAL
#	Const COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = "0"
#	Const COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = "1"
#	Const COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = "2"
#
#'	CÓDIGOS DE TIPO DE PESSOA USADOS NA REGRA DE CONSUMO DO ESTOQUE (MULTI CD)
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA = "PF"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL = "PR"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE = "PJC"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE = "PJNC"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO = "PJI"