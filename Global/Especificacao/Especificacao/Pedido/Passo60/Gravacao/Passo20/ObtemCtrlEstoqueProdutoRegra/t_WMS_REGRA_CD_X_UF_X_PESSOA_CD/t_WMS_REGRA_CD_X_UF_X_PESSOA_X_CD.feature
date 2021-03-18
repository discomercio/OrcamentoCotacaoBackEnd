@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

#	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD WHERE (id_wms_regra_cd_x_uf_x_pessoa = t_WMS_REGRA_CD_X_UF_X_PESSOA.id) ORDER BY ordem_prioridade
#		se nenhum registro, erro
#		para cada registro:
#			SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente)
#				se nenhum registro, tudo bem --- confirmar com Hamilton
#				se t_NFe_EMITENTE.st_ativo <> 1 então considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1
#			Localizar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente
#				se t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1, erro
#				se não localizar, tudo bem
Scenario: t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - sem registro
	Given Limpar tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não especifica nenhum CD para consumo do estoque (Id=5)"

@Especificacao.Pedido.Passo60.Gravacao.Passo20.ObtemCtrlEstoqueProdutoRegra.t_WMS_REGRA_CD_X_UF_X_PESSOA_CD
Scenario: t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1
	Given Tabela "t_NFe_EMITENTE" registro tipo de pessoa = "PR" e id_wms_regra_cd_x_uf = "26", alterar campo "st_ativo" = "0"
	Given Chamar ObtemCtrlEstoqueProdutoRegra e verificar regra do produto = "003220" e id_nfe_emitente = "4001", campo st_inativo = "1"
	Given Tabela "t_NFe_EMITENTE" verificar registro tipo de pessoa = "PR" e id_wms_regra_cd_x_uf = "26",campo "st_ativo" = "0"