@ignore
@Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
Feature: AlterarMagentoPedidoStatus


Scenario: AlterarMagentoPedidoStatus - Verificar se o Pedido_magento existe
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - pedido entregue
	#Se o pedido ou qualquer filhote estiver com status = cancelado ou entregue, não podemos mexer no pedido.
	#Se acontecer, mandar email para karina e retornar erro.
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - pedido cancelado
	#Se o pedido ou qualquer filhote estiver com status = cancelado ou entregue, não podemos mexer no pedido.
	#Se acontecer, mandar email para karina e retornar erro.
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - 	Transições possíveis: de 1 para 2, de 1 para 3. Qualquer outra transição resulta em erro.
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - aprovado
	#AlterarMagentoPedidoStatus - De 1 para 2 -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
	#se não permitir, verificar com Karina qual deve ser o status da analise_credito
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - rejeitado
	#De 1 para 3, rejeitado -> cancelamos automaticamente o pedido, conforme o flag por marketplace para habilitar o cancelamento automático
	#t_CODIGO_DESCRICAO, grupo = PedidoECommerce_Origem_Grupo, campo parametro_4_campo_flag
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - bloco de notas no pedido
	#Na transição de status, incluir um bloco de notas no pedido e também um log
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - verificar log
	#Na transição de status, incluir um bloco de notas no pedido e também um log
	When Fazer esta validação

Scenario: AlterarMagentoPedidoStatus - O campo é controlado somente pelo pedido pai.
	When Fazer esta validação
