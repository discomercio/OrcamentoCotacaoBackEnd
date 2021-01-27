@GerenciamentoBanco
@Especificacao.Pedido.Passo60.Gravacao.Passo60
Feature: Rotinas_Gera_num_pedido
	Testar a classe Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido


Background: Reiniciar banco ao terminar cenário
	Given Reiniciar banco ao terminar cenário

Scenario: Gerar 1
	#InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO = 6
	Given Colocar NSU do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "123"
	And Colocar Ano_Letra_Seq do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "A"
	Then Gera_num_pedido_pai = "000124A" sem erro
	Then Gera_num_pedido_pai = "000125A" sem erro
	Then Gera_num_pedido_pai = "000126A" sem erro

Scenario: outra letra
	#InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO = 6
	Given Colocar NSU do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "129"
	And Colocar Ano_Letra_Seq do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "B"
	Then Gera_num_pedido_pai = "000130B" sem erro
	Then Gera_num_pedido_pai = "000131B" sem erro

Scenario: erro - estouro
	#InfraBanco.Constantes.Constantes.TAM_MIN_NUM_PEDIDO = 6
	Given Colocar NSU do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "1234567"
	And Colocar Ano_Letra_Seq do InfraBanco.Constantes.Constantes.NSU_PEDIDO com "B"
	Then Gera_num_pedido_pai com erro contendo "descarte não é totalmente zero"

