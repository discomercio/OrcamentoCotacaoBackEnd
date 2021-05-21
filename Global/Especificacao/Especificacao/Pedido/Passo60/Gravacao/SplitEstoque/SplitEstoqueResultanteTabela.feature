@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: SplitEstoqueResultanteTabela

#Vamos testar vários cenários de auto-split
#com 1 produto e possível split em dois CDs
#fazer esse arquivo todo
Background: Background
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Pedido base
	When Lista de itens com "1" itens
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Zerar todo o estoque

#'   STATUS DE ENTREGA DO PEDIDO
#	Const ST_ENTREGA_ESPERAR		            = "ESP"  ' NENHUMA MERCADORIA SOLICITADA ESTÁ DISPONÍVEL
#	Const ST_ENTREGA_SPLIT_POSSIVEL             = "SPL"  ' PARTE DA MERCADORIA ESTÁ DISPONÍVEL PARA ENTREGA
#	Const ST_ENTREGA_SEPARAR		            = "SEP"	 ' TODA A MERCADORIA ESTÁ DISPONÍVEL E JÁ PODE SER SEPARADA PARA ENTREGA
#	Const ST_ENTREGA_A_ENTREGAR		            = "AET"	 ' A TRANSPORTADORA JÁ SEPAROU A MERCADORIA PARA ENTREGA
#	Const ST_ENTREGA_ENTREGUE		            = "ETG"	 ' MERCADORIA FOI ENTREGUE
#	Const ST_ENTREGA_CANCELADO		            = "CAN"	 ' VENDA FOI CANCELADA
Scenario Outline: Teste de auto-slipt - magento
	#Vamos ignorar na Loja pois o split funciona de forma diferente
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	#setup do estoque
	Given Definir saldo estoque = "<inicial1>" para produto = "um" e id_nfe_emitente = "4003"
	#precisamos ativar esse CD
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD" alterar registro id_wms_regra_cd_x_uf_x_pessoa = "666" e id_nfe_emitente = "4003", campo "st_inativo" = "0"
	Given Definir saldo estoque = "<inicial2>" para produto = "um" e id_nfe_emitente = "4903"
	When Lista de itens "0" informo "Qtde" = "<qde>"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	Then Gerado <NroPedidos> pedidos
	And Pedido gerado "<pedido>", verificar st_entrega = "<st_entrega>"
	And Pedido gerado <pedido>, verificar id_nfe_emitente = <CD> e qde = <iqde>
	And Verificar estoque id_nfe_emitente = <CD>, saldo de estoque = <estoque>
	And Verificar pedido gerado "<pedido>", saldo de ID_ESTOQUE_SEM_PRESENCA = "<spe>"

	Examples:
		| Caso | qde | inicial1 | inicial2 | NroPedidos | pedido | st_entrega | CD   | iqde | estoque | spe | Comentários                                                |
		| 1    | 10  | 40       | 40       | 1          | 0      | SEP        | 4903 | 10   | 30      | 0   | Pedido totalmente atendido pelo CD 1                       |
		| 2    | 50  | 40       | 40       | 2          | 0      | SEP        | 4903 | 40   | 0       | 0   | Pedido totalmente atendido pelo CD 1 e 2                   |
		| 2    | 50  | 40       | 40       | 2          | 1      | SEP        | 4003 | 10   | 30      | 0   |                                                            |
		| 3    | 100 | 40       | 40       | 2          | 0      | SPL        | 4903 | 60   | 0       | 20  | Pedido atendido pelo CD 1 e 2 e sobram mais 20 para o CD 1 |
		| 3    | 100 | 40       | 40       | 2          | 1      | SEP        | 4003 | 40   | 0       | 0   |                                                            |
		| 4    | 80  | 40       | 40       | 2          | 0      | SEP        | 4903 | 40   | 0       | 0   | Pedido atendido pelo CD 1 e 2 e zera todo o estoque        |
		| 4    | 80  | 40       | 40       | 2          | 1      | SEP        | 4003 | 40   | 0       | 0   |                                                            |
		| 5    | 100 | 0        | 0        | 1          | 0      | ESP        | 4903 | 100  | 0       | 100 | Pedido atendido pelo CD 1, tudo sem presença no estoque    |
		| 6    | 15  | 0        | 40       | 1          | 0      | SEP        | 4903 | 15   | 25      | 0   | Pedido atendido pelo CD 2                                  |
		| 7    | 120 | 10       | 0        | 2          | 0      | ESP        | 4903 | 110  | 0       | 110 | Pedido atendido pelo CD 1 com sem presença no estoque      |
		| 7    | 120 | 10       | 0        | 2          | 1      | SEP        | 4003 | 10   | 0       | 110 | Pedido atendido pelo CD 1 com sem presença no estoque      |
# No magento devemos atender o pedido todo, sendo assim
# o caso 7 será splitado gerando 2 pedidos