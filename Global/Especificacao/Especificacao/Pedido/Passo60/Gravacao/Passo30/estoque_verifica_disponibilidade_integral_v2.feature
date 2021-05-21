@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: estoque_verifica_disponibilidade_integral_v2

Background: Configuracao
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

@Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo30
Scenario: Teste 1 - Estoque_Qtde_Estoque
	Given Definir saldo de estoque = "8" para produto "um"
	Given Definir saldo de estoque = "5" para produto "dois"
	Given Chamar ObtemCtrlEstoqueProdutoRegra
	Given Chamar Estoque_verifica_disponibilidade_integral_v2
	Then Regra t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD do produto = "um", verificar campo "estoque_qtde_estoque" = "8"
	Then Regra t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD do produto = "dois", verificar campo "estoque_qtde_estoque" = "5"

@Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo30
Scenario: Teste 2 - Estoque_Qtde_Estoque_Global
	Given Definir saldo de estoque = "18" para produto "um"
	Given Definir saldo de estoque = "25" para produto "dois"
	Given Chamar ObtemCtrlEstoqueProdutoRegra
	Given Chamar Estoque_verifica_disponibilidade_integral_v2
	Then Regra t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD do produto = "um", verificar campo "estoque_qtde_estoque_global" = "18"
	Then Regra t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD do produto = "dois", verificar campo "estoque_qtde_estoque_global" = "25"