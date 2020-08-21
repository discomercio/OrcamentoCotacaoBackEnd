@ignore
Feature: ListaExecucao

Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.ListaExecucao"
#teste da autenticação
	And Especificado em "Especificacao.Comuns.Api.Autenticacao"
#o teste em si
	And Especificado em "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa"

