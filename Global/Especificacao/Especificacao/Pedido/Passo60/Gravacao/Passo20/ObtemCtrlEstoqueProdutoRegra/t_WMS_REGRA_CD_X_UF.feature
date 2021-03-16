@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_WMS_REGRA_CD_X_UF

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

#	SELECT * FROM t_WMS_REGRA_CD_X_UF WHERE (id_wms_regra_cd = id_wms_regra_cd) AND (uf = UF)
#		se nenhum registro, erro
#		se mais de um registro, erro (não está no ASP)
Scenario: t_WMS_REGRA_CD_X_UF - sem regra
	Given Tabela "t_WMS_REGRA_CD_X_UF" apagar registro do id_wms_regra_cd = "5" da UF = "SP"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)"

Scenario: t_WMS_REGRA_CD_X_UF - duplicado
	Given Tabela "t_WMS_REGRA_CD_X_UF" duplicar registro do id_wms_regra_cd = "5" da UF = "SP"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)"