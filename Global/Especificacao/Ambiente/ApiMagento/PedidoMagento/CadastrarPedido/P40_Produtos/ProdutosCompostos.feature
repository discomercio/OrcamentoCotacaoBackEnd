@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ProdutosCompostos
	Verifica se a Api Magento está aceitando o pedido com produtos compostos.
	Obs: Esse processo é novo! Passaremos a permitir que o Magento envie os produtos compostos para que a ApiMagento
		 faça a separação dos produtos compostos antes de inserir o pedido

Scenario: ProdutosCompostos - Sucesso
	Given Pedido base
	When Lista de itens com "2" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"


Scenario: ProdutosCompostos - produtos repetidos
	#queremos verificar se esta normalizando a lista de itens
	#inserir um produto composto e um simples que conste no composto para que seja separado e altere a quantidade do produto simples
	Given Pedido base
	When Lista de itens com "2" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	#item 2
	When Lista de itens "0" informo "Sku" = "001000"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "340.00"
	When Lista de itens "0" informo "RowTotal" = "340.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "680.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "680.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"