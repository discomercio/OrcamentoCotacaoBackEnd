@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

Scenario: Verificar data da última movimentação
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

	Given Pedido base
	Given Definir saldo de estoque = "40" para produto "um"
	Given Definir saldo de estoque = "40" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	#When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "50"
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE" registro pai, verificar campo "data_ult_movimento" = "data atual"

Scenario: teste com estoque zerado - magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Definir saldo de estoque = "0" para produto "um"
	Given Pedido base
	Then Sem erro "Produto 003220 do fabricante 003: faltam 5 unidades"