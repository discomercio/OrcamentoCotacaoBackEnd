@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
Feature: SemPedidosPj
#api mangeto não aceita pedidos de PJ
#para ver porque, consulte a rotina PedidoMagentoBll.LimitarPedidosMagentoPJ

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente.SemPedidosPj"

Scenario: SemPedidosPj
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "XX"
	Then Erro "A API somente aceita pedidos para PF."

Scenario: SemPedidosPj 2
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "A API somente aceita pedidos para PF."
