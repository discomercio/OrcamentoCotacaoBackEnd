@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: EspecificacaoMagento
Definições da ApiMagento
Paradigma de salvamento: fazer o mesmo que acontece com o processo semi-automático.
Se o semi-automático der erro, damos erro. Se aceitar, aceitamos.
Estoque: não é um problema. 

Scenario: preço: aceitamos o valor que vier do magento.
	#nao fazemos nenhuma crítica nos preços que chegam do magento
	Given Pedido base
	When Informo "Frete" = "10"
	When Lista de itens "0" informo "Preco_Venda" = "650.00"
	When Lista de itens "0" informo "Preco_NF" = "800.00"
	When Lista de itens "1" informo "Preco_Venda" = "950.00"
	When Lista de itens "1" informo "Preco_NF" = "1100.00"
	Then Sem nenhum erro