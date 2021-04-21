@Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
Feature: FluxoAlterarMagentoPedidoStatus

Status:
1 = aprovação pendente (pedido cadastrado e pagamento não confirmado)
2 = aprovado (pagamento confirmado)
3 = rejeitado (pedido cancelado)

Fluxo:
	Verificar se o Pedido_magento existe
	Se o pedido ou qualquer filhote estiver com status = cancelado ou entregue, não podemos mexer no pedido. 
		Se acontecer, mandar email para karina e retornar erro.
	Transições possíveis: de 1 para 2, de 1 para 3. Qualquer outra transição resulta em erro.

	gravar campo na t_pedido
	Na transição de status, incluir um bloco de notas no pedido e também um log
??????????	  campo controlado somente pelo pedido pai
      possíveis:
      - aprovado -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
      - rejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace para habilitar o cancelamento automático (definr o flag em t_codigo_descricao)

Scenario: AlterarMagentoPedidoStatus
	Then afazer