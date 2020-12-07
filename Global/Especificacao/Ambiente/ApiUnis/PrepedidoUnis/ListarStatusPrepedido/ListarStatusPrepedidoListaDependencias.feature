@ignore
@ListaDependencias
Feature: ListarStatusPrepedidoListaDependencias

Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiUnis.PrepedidoUnis.ListarStatusPrepedido.ListarStatusPrepedidoListaDependencias"
#teste da autenticação
	And Especificado em "Especificacao.Comuns.Api.Autenticacao"
#o teste em si
	And Especificado em "Ambiente.ApiUnis.PrepedidoUnis.ListarStatusPrepedido.ListarStatusPrepedido"

Scenario: este é feito nele mesmo
	Given Nome deste item "Ambiente.ApiUnis.PrepedidoUnis.ListarStatusPrepedido.ListarStatusPrepedido"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.ListarStatusPrepedido.ListarStatusPrepedidoListaDependencias"
