@Especificacao.Comuns.Api.Autenticacao.Autenticacao
Feature: Autenticacao
	Verificar a autenticação

Scenario: Autenticação inválida
	Given Dado base
	When Informo "TokenAcesso" = "um token inválido"
	Then Erro status code "401"

Scenario: Autenticação válida
	Given Dado base
	#status code 200 é sem erro
	Then Erro status code "200"

