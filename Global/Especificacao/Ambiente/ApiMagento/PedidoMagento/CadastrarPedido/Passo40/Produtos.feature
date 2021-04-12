@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: Produtos
	Passo40: No magento alguns testes tem a mensagem diferente do Global

Scenario: produtos: sempre virão divididos, nunca vai vir um produto composto.
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001090"
	When Lista de itens "0" informo "Preco_Venda" = "1648.00"
	When Lista de itens "0" informo "Preco_NF" = "1648.00"
	Then Erro "Produto não cadastrado para a loja. Produto: 001090, loja: 201"

