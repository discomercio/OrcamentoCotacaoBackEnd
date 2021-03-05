@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: st_entrega

Background:
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

#Se algum produto saiu do estoque e nenhum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SEPARAR
Scenario: st_entrega - separar
	Given Pedido base
	Given Definir saldo de estoque = "1000" para produto "um"
	Given Definir saldo de estoque = "1000" para produto "dois"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SEP"

#Se nenhum produto saiu do estoque (tabela t_ESTOQUE_ITEM)
#	st_entrega = ST_ENTREGA_ESPERAR
Scenario: st_entrega - esperar
	Given Pedido base
	Given Definir saldo de estoque = "0" para produto "um"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "ESP"

#Se algum produto saiu do estoque e algum produto foi vendido sem presença no estoque
#	st_entrega = ST_ENTREGA_SPLIT_POSSIVEL

Scenario: st_entregar - split possivel
	#criamos um pedido com split possivel no verdinho com esses dados
	Given Pedido base
	Given Definir saldo de estoque = "40" para produto "um"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SPL"