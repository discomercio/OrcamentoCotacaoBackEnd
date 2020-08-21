@Especificacao.Comuns.Api.Autenticacao
Feature: Autenticacao
	Verificar a autenticação

Background: Testar em:
	Given Nome deste item "Especificacao.Comuns.Api.Autenticacao"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.ListaExecucao"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.ListaExecucao"	

Scenario: Autenticação inválida
	Given Dado base
	When Informo "TokenAcesso" = "um token inválido"
	Then Erro status code "401"

Scenario: Autenticação válida
	Given Dado base
	Then Erro status code "200"

