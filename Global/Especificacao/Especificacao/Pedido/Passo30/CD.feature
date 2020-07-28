@ignore
Feature: Validação do CD
todo: terminar Validação do CD

Scenario: Lista de CDs disponíveis
	afazer todo: terminar de fazer

Scenario: Validar permissão de CD
	Given Pedido base
	And Usuário sem permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD

Scenario: Validar CD escolhido caso manual
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD

