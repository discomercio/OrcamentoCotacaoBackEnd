@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ProdutosSemPresencaEstoque

Background: Configuracoes
	Given Reiniciar banco imediatamente

Scenario: PresencaEstoque - produto simples
	#pergunta:
	#	No magento, caso o produto não tenha presença no estoque, salvamos o pedido normalmente?
	#resposta:
	#	sim. quando entra o estoque, esse pedido é automaticamente suprido.
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "C_pu_valor" = "340.00"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	When Informo "Frete" = "10"
	When Lista de itens com "1" itens
	#When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "338.85"
	When Lista de itens "0" informo "Preco_NF" = "340.00"
	Given Zerar todo o estoque
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "1"

@ignore
Scenario: PresencaEstoque - produto composto
	Given Pedido base
	When Lista de itens com "2" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	Given Zerar todo o estoque
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	#item 1
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "ajustar valor"
	#item 2
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"