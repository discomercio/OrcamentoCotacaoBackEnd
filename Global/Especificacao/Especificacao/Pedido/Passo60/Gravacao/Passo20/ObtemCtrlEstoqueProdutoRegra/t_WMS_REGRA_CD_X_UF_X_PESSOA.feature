@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_WMS_REGRA_CD_X_UF_X_PESSOA

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

#	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA WHERE (id_wms_regra_cd_x_uf = t_WMS_REGRA_CD_X_UF.id) AND (tipo_pessoa = tipo_pessoa)
#		se nenhum registro, erro
#		se mais de um registro, erro (não está no ASP)
#		se t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente = 0, erro
Scenario: t_WMS_REGRA_CD_X_UF_X_PESSOA - sem regra
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" apagar registro id = "134" e tipo de pessoa = "PF"
	#Para o prepedido
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" apagar registro id = "134" e tipo de pessoa = "PR"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)"

Scenario: t_WMS_REGRA_CD_X_UF_X_PESSOA - duplicado
	#no SQL server existe um indice que nao deixa duplicar o registro
	Given Ignorar cenário no ambiente "UsarSqlServerNosTestesAutomatizados"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" duplicar registro id_wms_regra_cd_x_uf = "134" e tipo de pessoa = "PF" com id = "811" se não UsarSqlServerNosTestesAutomatizados
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" duplicar registro id_wms_regra_cd_x_uf = "134" e tipo de pessoa = "PR" com id = "812" se não UsarSqlServerNosTestesAutomatizados
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)"

Scenario: t_WMS_REGRA_CD_X_UF_X_PESSOA - spe_id_nfe_emitente = 0
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" registro id_wms_regra_cd_x_uf = "134" e tipo de pessoa = "PF", alterar campo "spe_id_nfe_emitente" = "0"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA" registro id_wms_regra_cd_x_uf = "134" e tipo de pessoa = "PR", alterar campo "spe_id_nfe_emitente" = "0"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=5)"