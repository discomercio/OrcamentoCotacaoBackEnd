@ignore
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: ProdutosPresencaEstoque

Scenario: PresencaEstoque
	#pergunta:
	#	No magento, caso o produto não tenha presença no estoque, salvamos o pedido normalmente?
	#resposta:
	#	sim. quando entra o estoque, esse pedido é automaticamente suprido.
	Given Pedido base
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "338.85"
	When Lista de itens "0" informo "Preco_NF" = "340.00"
	When zerar estoque do produto em todos os cd's
	Then Sem nenhum erro

#esse teste esta sendo feito em "EspecificacaoMagento.feature/preço: aceitamos o valor que vier do magento."
Scenario: Produtos
	#	preço: aceitamos o valor que vier do magento. Não validamos o preço.
	Given fazer esta validacao
	When Implementado em Especificacao.Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.EspecificacaoMagentoFeature.preço: aceitamos o valor que vier do magento.