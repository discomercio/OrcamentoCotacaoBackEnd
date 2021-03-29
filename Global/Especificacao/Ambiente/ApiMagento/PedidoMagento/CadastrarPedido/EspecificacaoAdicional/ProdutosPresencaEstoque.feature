@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: ProdutosPresencaEstoque

Scenario: PresencaEstoque
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
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "338.85"
	When Lista de itens "0" informo "Preco_NF" = "340.00"
	Given Zerar todo o estoque
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "fabricante" = "001"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "produto" = "001000"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "1"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "338.85"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_NF" = "340.00"

#esse teste esta sendo feito em "EspecificacaoMagento.feature/preço: aceitamos o valor que vier do magento."
Scenario: Produtos
	#	preço: aceitamos o valor que vier do magento. Não validamos o preço.
	#Implementado em Especificacao.Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.EspecificacaoMagentoFeature.preço: aceitamos o valor que vier do magento.
	Given Validação feita em outro arquivo
