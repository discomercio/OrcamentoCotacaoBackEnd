@ignore
Feature: SplitEstoqueResultanteTabela
#Vamos testar vários cenários de auto-split
#com 1 produto e possível split em dois CDs

Background: Background
	Given Pedido base sem itens
	And Usando fabricante = "001", produto = "001000"
	And Zerar todo o estoque

Scenario Outline: Teste de auto-slipt

	#setup do estoque
	Given Definir estoque id_nfe_emitente = "4003", saldo de estoque = <inicial1>
	And Definir estoque id_nfe_emitente = "4903", saldo de estoque = <inicial2>

	And Pedido base sem itens
	And Criar novo item com qde = <qde>

	When Cadastrar pedido
	Then Gerado <NroPedidos> pedidos

	And Pedido gerado <pedido>, verificar st_entrega = <st_entrega>
	And Pedido gerado <pedido>, verificar id_nfe_emitente = <CD> e qde = <iqde>

	And Verificar estoque id_nfe_emitente = <CD>, saldo de estoque = <estoque>
	And Verificar estoque id_nfe_emitente = <CD>, saldo de ID_ESTOQUE_SEM_PRESENCA = <spe>

	Examples:
		| Caso | qde | inicial1 | inicial2 | NroPedidos | pedido | st_entrega | CD   | iqde | estoque | spe | Comentários                                                |
		| 1    | 10  | 40       | 40       | 1          | 0      | SEP        | 4003 | 10   | 30      | 0   | Pedido totalmente atendido pelo CD 1                       |
		| 2    | 50  | 40       | 40       | 2          | 0      | SEP        | 4003 | 40   | 0       | 0   | Pedido totalmente atendido pelo CD 1 e 2                   |
		| 2    | 50  | 40       | 40       | 2          | 1      | SEP        | 4903 | 10   | 30      | 0   |                                                            |
		| 3    | 100 | 40       | 40       | 2          | 0      | SPL        | 4003 | 60   | 0       | 20  | Pedido atendido pelo CD 1 e 2 e sobram mais 20 para o CD 1 |
		| 3    | 100 | 40       | 40       | 2          | 1      | SEP        | 4903 | 40   | 0       | 0   |                                                            |
		| 4    | 80  | 40       | 40       | 2          | 0      | SEP        | 4003 | 40   | 0       | 0   | Pedido atendido pelo CD 1 e 2 e zera todo o estoque        |
		| 4    | 80  | 40       | 40       | 2          | 1      | SEP        | 4903 | 40   | 0       | 0   |                                                            |
		| 5    | 100 | 0        | 0        | 1          | 0      | ESP        | 4003 | 100  | 0       | 100 | Pedido atendido pelo CD 1, tudo sem presença no estoque    |
		| 6    | 15  | 0        | 40       | 1          | 0      | SEP        | 4903 | 15   | 25      | 0   | Pedido atendido pelo CD 2                                  |
		| 7    | 120 | 10       | 0        | 1          | 0      | SPL        | 4003 | 120  | 0       | 110 | Pedido atendido pelo CD 1 com sem presença no estoque      |

#'   STATUS DE ENTREGA DO PEDIDO
#	Const ST_ENTREGA_ESPERAR		            = "ESP"  ' NENHUMA MERCADORIA SOLICITADA ESTÁ DISPONÍVEL
#	Const ST_ENTREGA_SPLIT_POSSIVEL             = "SPL"  ' PARTE DA MERCADORIA ESTÁ DISPONÍVEL PARA ENTREGA
#	Const ST_ENTREGA_SEPARAR		            = "SEP"	 ' TODA A MERCADORIA ESTÁ DISPONÍVEL E JÁ PODE SER SEPARADA PARA ENTREGA
#	Const ST_ENTREGA_A_ENTREGAR		            = "AET"	 ' A TRANSPORTADORA JÁ SEPAROU A MERCADORIA PARA ENTREGA
#	Const ST_ENTREGA_ENTREGUE		            = "ETG"	 ' MERCADORIA FOI ENTREGUE
#	Const ST_ENTREGA_CANCELADO		            = "CAN"	 ' VENDA FOI CANCELADA

