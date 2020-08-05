@Especificacao.Comuns.Api.Autenticacao
Feature: Autenticacao
	Verificar a autenticação

Background: Testar em:
	Given Implementado em "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido"
	

Scenario: Autenticação inválida
	Given Dado base
	When Informo "TokenAcesso" = "um token inválido"
	Then Erro status code "401"

Scenario: Autenticação válida
	Given Dado base
	Then Erro status code "200"

