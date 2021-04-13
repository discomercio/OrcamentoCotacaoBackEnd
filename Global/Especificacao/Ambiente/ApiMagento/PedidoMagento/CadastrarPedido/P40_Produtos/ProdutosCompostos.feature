@ignore
Feature: ProdutosCompostos
	Verifica se a Api Magento está aceitando o pedido com produtos compostos.
	Obs: Esse processo é novo! Passaremos a permitir que o Magento envie os produtos compostos para que a ApiMagento
		 faça a separação dos produtos compostos antes de inserir o pedido

Scenario: ProdutosCompostos - execede Qtde maxima de itens
	Then afazer: montar pedido somente com produtos compostos

Scenario: ProdutosCompostos - Sucesso
	Then afazer: criar um pedido com produtos compostos

Scenario: ProdutosCompostos - produtos repetidos
#queremos verificar se esta normalizando a lista de itens
#inserir um produto composto e um simples que conste no composto para que seja separado e altere a quantidade do produto simples
Then afazer: criar pedido com produto composto e inserir produtos simples que existe no composto

