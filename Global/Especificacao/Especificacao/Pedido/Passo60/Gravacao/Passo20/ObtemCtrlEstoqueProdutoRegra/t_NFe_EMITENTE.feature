@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_NFe_EMITENTE

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

#	SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente)
#		se nenhum registro, tudo bem --- confirmar com Hamilton
#		se t_NFe_EMITENTE.st_ativo <> 1, erro

Scenario: t_NFe_EMITENTE - st_ativo = 0
	Given Tabela "t_NFe_EMITENTE" registro tipo de pessoa = "PF" e id_wms_regra_cd_x_uf = "134", alterar campo "st_ativo" = "0"
	Given Tabela "t_NFe_EMITENTE" registro tipo de pessoa = "PR" e id_wms_regra_cd_x_uf = "134", alterar campo "st_ativo" = "0"
	Given Pedido base
	Then Erro "Falha na regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 especifica um CD para aguardar produtos sem presença no estoque que não está habilitado (Id=5)"