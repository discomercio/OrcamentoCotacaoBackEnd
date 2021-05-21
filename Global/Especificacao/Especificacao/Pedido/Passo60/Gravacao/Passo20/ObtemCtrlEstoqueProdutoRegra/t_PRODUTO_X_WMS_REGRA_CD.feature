@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_PRODUTO_X_WMS_REGRA_CD

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

	#	SELECT id_wms_regra_cd FROM t_PRODUTO_X_WMS_REGRA_CD WHERE fabricante = FABRICANTE AND produto = PRODUTO
	#		se t_PRODUTO_X_WMS_REGRA_CD.id_wms_regra_cd = 0, erro
	#		se nenhum registro, erro
	#		se mais de um registro, erro (não está no ASP)


Scenario: t_PRODUTO_X_WMS_REGRA_CD - id_wms_regra_cd = 0
	Given Tabela "t_PRODUTO_X_WMS_REGRA_CD" fabricante = "003" e produto = "003220", alterar registro do campo "id_wms_regra_cd" = "0"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': produto (003)003220 não está associado a nenhuma regra"
	And Tabela t_PRODUTO_X_WMS_REGRA_CD fabricante = "003" e produto = "003220", verificar campo "id_wms_regra_cd" = "0"

Scenario: t_PRODUTO_X_WMS_REGRA_CD - produto duplicado
	#Não podemos fazer esse teste porque fabricante e produto são chaves
	#Given Tabela "t_PRODUTO_X_WMS_REGRA_CD" duplicar regra para fabricante = "003" e produto = "003221" com id_wms_regra_cd = "6"
	#Given Pedido base
	#Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': produto (003)003221 não possui regra associada"

Scenario: t_PRODUTO_X_WMS_REGRA_CD - sem regra
	Given Tabela "t_PRODUTO_X_WMS_REGRA_CD" apagar registro do fabricante = "003" e produto = "003220"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': produto (003)003220 não possui regra associada"
