@ignore
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
    De 1 para 2 -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
		se não permitir, verificar com Karina qual deve ser o status da analise_credito 
    De 1 para 3, rejeitado -> cancelamos automaticamente o pedido, conforme o flag por marketplace para habilitar o cancelamento automático
		t_CODIGO_DESCRICAO, grupo = PedidoECommerce_Origem_Grupo, campo parametro_4_campo_flag
	Na transição de status, incluir um bloco de notas no pedido e também um log
	O campo é controlado somente pelo pedido pai.

Scenario: AlterarMagentoPedidoStatus
	Then Testes implementados em "AlterarMagentoPedidoStatus.feature"

##Ata de reunião:
#	- statuspedido:
#	  gravar campo na t_pedido
#	  na transição de status, incluir um bloco de notas no pedido
#	  e também um log
#	  campo controlado somente pelo pedido pai
#	- status: o magento pode passar um pedido de aprovado para rejeitado?
#	  resposta: não
#	- Status do pedido: quais os status possíveis?
#	  (inicialmente: pedido aprovado ou não aprovado)
#      se o pedido estiver com status = cancelado ou entregue, não podemos mexer no pedido. Se acontecer,
#		mandar email para karina e retornar erro.
#      possíveis:
#      - aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
#      - aprovado -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
#      - rejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace para habilitar o
#	     cancelamento automático (definr o flag em t_codigo_descricao)
#      Talvez: colocar um bloco na tela inicial do verdinho listando os pedidos parados em aprovação pendente há mais de X dias.
#      ter mais um status de analise de crédito: esperando aprovação pelo magento.
