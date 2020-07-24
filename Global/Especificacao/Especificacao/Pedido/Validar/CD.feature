@ignore
Feature: Validação do CD

@loja
Scenario: Lista de CDs disponíveis
	afazer todo: terminar de fazer

@api @loja
Scenario: Validar permissão de CD
	Given Pedido base
	And Usuário sem permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD

@api @loja
Scenario: Validar CD escolhido caso manual
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD

