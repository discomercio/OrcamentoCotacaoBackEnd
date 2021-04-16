@ignore
@Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
Feature: AlterarMagentoPedidoStatus
	- statuspedido:
	  gravar campo na t_pedido
	  na transição de status, incluir um bloco de notas no pedido
	  e também um log
	  campo controlado somente pelo pedido pai
	- status: o magento pode passar um pedido de aprovado para rejeitado?
	  resposta: não
	- Status do pedido: quais os status possíveis?
	  (inicialmente: pedido aprovado ou não aprovado)
      se o pedido estiver com status = cancelado ou entregue, não podemos mexer no pedido. Se acontecer, 
		mandar email para karina e retornar erro.
      possíveis:
      - aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
      - aprovado -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
      - rejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace para habilitar o 
	     cancelamento automático (definr o flag em t_codigo_descricao)
      Talvez: colocar um bloco na tela inicial do verdinho listando os pedidos parados em aprovação pendente há mais de X dias.
      ter mais um status de analise de crédito: esperando aprovação pelo magento.

Scenario: AlterarMagentoPedidoStatus - aprovacao pendente
	aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - aprovado
	aprovado -> quando for para aprovado, 
	passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - rejeitado
	rejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace para habilitar o 
	cancelamento automático (definr o flag em t_codigo_descricao)
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - pedido cancelado
	se o pedido estiver com status = cancelado, não podemos mexer no pedido. Se acontecer, 
	mandar email para karina e retornar erro.
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - pedido entregue
	se o pedido estiver com status = entregue, não podemos mexer no pedido. Se acontecer, 
	mandar email para karina e retornar erro.
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - verificar bloco de notas no pedido
	na transição de status, incluir um bloco de notas no pedido
	Then Sem nenhum Erro

Scenario: AlterarMagentoPedidoStatus - verificar log
	na transição de status, insere log
	Then Sem nenhum Erro