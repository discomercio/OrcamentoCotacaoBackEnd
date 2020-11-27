@ignore
@Especificacao.Pedido.Passo30
Feature: Validações do opcao_possui_RA

#sem indicador
Scenario: NUMERO_LOJA_ECOMMERCE_AR_CLUBE não pode
	Given Loja do usuário = "NUMERO_LOJA_ECOMMERCE_AR_CLUBE"
	When Pedido base
	And Informo "opcao_possui_RA" = "1"
	And Informo "indicador" = ""
	Then Erro "Usuário não pode opcao_possui_RA"

Scenario: NUMERO_LOJA_ECOMMERCE_AR_CLUBE não pode 2
	Given Loja do usuário = "202"
	When Pedido base
	And Informo "opcao_possui_RA" = "1"
	And Informo "indicador" = ""
	Then Sem erro "Usuário não pode opcao_possui_RA"

#com indicador
#Testado em Perc_RT.feature
