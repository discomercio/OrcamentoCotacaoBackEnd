@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: SemPedidosPj
#api mangeto não aceita pedidos de PJ
#para ver porque, consulte a rotina PedidoMagentoBll.LimitarPedidosMagentoPJ

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.SemPedidosPj"

@ListaDependencias
Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.SemPedidosPj"
	And Implementado em "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias"


Scenario: SemPedidosPj
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "XX"
	Then Erro "A API somente aceita pedidos para PF."

Scenario: SemPedidosPj 2
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "A API somente aceita pedidos para PF."
