@ignore
@Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
Feature: CadastrarPrepedidoVerificarQueExecutou
	Os testes feitos ao cadastrar o prepedido

Scenario: Verificar que executou
#teste da autenticação
	Then Verificar que executou "Especificacao.Comuns.Api.Autenticacao"
	Then Verificar que executou "Especificacao.Pedido"
