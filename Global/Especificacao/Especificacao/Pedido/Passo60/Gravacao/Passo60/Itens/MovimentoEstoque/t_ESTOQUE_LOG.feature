@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE_LOG

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

@ignore
Scenario Outline: Verificar log da movimentação
	Then afazer essa validação

