Feature: MagentoPedidoStatus
	MagentoPedidoStatus deve ser aprovado ou aprovação pendente
	Gravar campo MagentoPedidoStatus na t_PEDIDO
      - aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
      - aprovado -> analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)

Scenario: MagentoPedidoStatus deve ser aprovado ou aprovação pendente
	Given Fazer este teste

Scenario: Gravar campo MagentoPedidoStatus na t_PEDIDO pai
	Given Fazer este teste

Scenario: Gravar campo MagentoPedidoStatus na t_PEDIDO filhote
	Given Fazer este teste

Scenario:  aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
	Given Fazer este teste

Scenario:  aprovado -> analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
se não, fica em qual status???
	Given Fazer este teste

