@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: obtemCtrlEstoqueProdutoRegra

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

@ignore
Scenario: obtemCtrlEstoqueProdutoRegra
	#Para cada produto no pedido:
	#	SELECT id_wms_regra_cd FROM t_PRODUTO_X_WMS_REGRA_CD WHERE fabricante = FABRICANTE AND produto = PRODUTO
	#		se t_PRODUTO_X_WMS_REGRA_CD.id_wms_regra_cd = 0, erro
	#		se nenhum registro, erro
	#		se mais de um registro, erro (não está no ASP)
	#	SELECT * FROM t_WMS_REGRA_CD WHERE id = id_wms_regra_cd
	#		se nenhum registro, erro
	#		se mais de um registro, erro (não está no ASP)
	#	SELECT * FROM t_WMS_REGRA_CD_X_UF WHERE (id_wms_regra_cd = id_wms_regra_cd) AND (uf = UF)
	#		se nenhum registro, erro
	#		se mais de um registro, erro (não está no ASP)
	#	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA WHERE (id_wms_regra_cd_x_uf = t_WMS_REGRA_CD_X_UF.id) AND (tipo_pessoa = tipo_pessoa)
	#		se nenhum registro, erro
	#		se mais de um registro, erro (não está no ASP)
	#		se t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente = 0, erro
	#	SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente)
	#		se nenhum registro, tudo bem --- confirmar com Hamilton
	#		se t_NFe_EMITENTE.st_ativo <> 1, erro
	#	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD WHERE (id_wms_regra_cd_x_uf_x_pessoa = t_WMS_REGRA_CD_X_UF_X_PESSOA.id) ORDER BY ordem_prioridade
	#		se nenhum registro, erro
	#		para cada registro:
	#			SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente)
	#				se nenhum registro, tudo bem --- confirmar com Hamilton
	#				se t_NFe_EMITENTE.st_ativo <> 1 então considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1
	#			Localizar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente
	#				se t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1, erro
	#				se não localizar, tudo bem
	When Fazer esta validação
