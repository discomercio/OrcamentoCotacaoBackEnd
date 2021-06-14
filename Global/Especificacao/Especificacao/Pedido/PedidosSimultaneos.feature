@Especificacao.Pedido.PedidosSimultaneos
@Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_RotinaSteps
@GerenciamentoBanco
Feature: PedidosSimultaneos
#Fazer pedidos simultâneos
#e verificamos se o estoque ficou certo


#este teste somente vai funcionar no SQL Server real quando tivemos o sistema de bloqueio em funcionamento
Scenario: PedidosSimultaneos
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	Given Zerar todo o estoque
	Given Definir saldo de estoque = "300" para produto "um"
	Given Definir saldo de estoque = "300" para produto "dois"
	#cria todos os pedidos
	Given Testar pedidos simultâneos com multiplicadorPorPedido = "2" (magento e loja), pedidosPorThread = "5" e numeroThreads = "10"
#vai gerar 2*5*10 + 2 = 102 pedidos. Cada pedido tem 2 de cada produto e no início ele gera um pedido adicional.
# multiplicadorPorPedido + numeroThreads * pedidosPorThread * multiplicadorPorPedido = 102

	Then Movimento de estoque = "204" para produto "um"
	Then Saldo de estoque = "96" para produto "um"

	Then Movimento de estoque = "204" para produto "dois"
	Then Saldo de estoque = "96" para produto "dois"
