@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ProdutosCompostos
	Verifica se a Api Magento está aceitando o pedido com produtos compostos.
	O Magento pode enviar os produtos compostos para que a ApiMagento faça a separação dos produtos compostos antes de inserir o pedido

SKU:	001090	
É composto por:	
001 / 001000	x 1
001 / 001001	x 1

Scenario: ProdutosCompostos - Sucesso
	#Um produto composto (1090) sendo dividido em dois
	Given Pedido base
	When Lista de itens com "1" itens
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
	When Lista de itens "1" informo "Sku" = "001000"
	When Lista de itens "1" informo "Quantidade" = "1"
	When Lista de itens "1" informo "Subtotal" = "340.00"
	When Lista de itens "1" informo "RowTotal" = "340.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "680.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "680.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"

Scenario: ProdutosCompostos - com desconto
	#Um produto composto (1090) - verificar o cálculo do desconto
	Given Pedido base
	When Lista de itens com "1" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "1300.00"
	When Lista de itens "0" informo "DiscountAmount" = "200.00"
	When Lista de itens "0" informo "RowTotal" = "1100.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"

Scenario: ProdutosCompostos - produtos repetidos com desconto
	#Um produto composto (1090) sendo dividido em dois - calculando os descontos corretamente
	Given Pedido base
	When Lista de itens com "2" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "DiscountAmount" = "200.00"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	#item 2
	When Lista de itens "1" informo "Sku" = "001000"
	When Lista de itens "1" informo "Quantidade" = "1"
	When Lista de itens "1" informo "Subtotal" = "340.00"
	When Lista de itens "0" informo "DiscountAmount" = "100.00"
	When Lista de itens "1" informo "RowTotal" = "340.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_nf" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_nf" = "ajustar valor"

Scenario: ProdutosCompostos - diluir frete
	#Dois produtos compostos - diluindo o frete entre eles
	Given Pedido base
	When Informo "PedidoTotaisMagentoDto.FreteBruto" = "69.90"
	And Lista de itens com "2" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	#item 2
	When Lista de itens "1" informo "Sku" = "001091"
	When Lista de itens "1" informo "Quantidade" = "1"
	When Lista de itens "1" informo "Subtotal" = "ajustar valores"
	When Lista de itens "1" informo "RowTotal" = "ajustar valores"
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
	#item 3
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "3" campo "preco_nf" = "ajustar valor"
	#item 4
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "produto" = "001001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "desc_dado" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "preco_venda" = "ajustar valor"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "4" campo "preco_nf" = "ajustar valor"

Scenario: ProdutosCompostos - com desconto e diluir frete
	#Um produto composto (1090), um produto solto, com desconto nos produtos e com frete
	When Fazer esta validação

Scenario: ProdutosCompostos - não existe
	Given Pedido base
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Sku" = "001111"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	Then Erro "ajustar msg"