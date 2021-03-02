@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

@ignore
Scenario: Verificar data da última movimentação
	Given fazer essa validação

@ignore
Scenario Outline: verificar alteração de último movimento estoque
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE" registro com campo "id_estoque" = "id_estoque", verificar campo "<campo>" = "<valor>"

	#OBS => ACHO QUE NÃO PRECISAMOS VALIDAR TODOS OS CAMPOS, pois só é alterado a data da último movto
	#testoque.Data_ult_movimento = DateTime.Now.Date;
	#verificar se o campo "data_ult_movimento" esta com a data correta
	Examples:
		| campo                       | valor               |
		| id_estoque                  | 000000119328        |
		| data_entrada                | 2020-12-29 00:00:00 |
		| hora_entrada                | 135628              |
		| fabricante                  | 003                 |
		| documento                   | tstenovo            |
		| usuario                     | PRAGMATICA          |
		| data_ult_movimento          | 2021-01-20 00:00:00 |
		| [timestamp]                 | [x[                 |
		| kit                         | 0                   |
		| entrada_especial            | 0                   |
		| devolucao_status            | 0                   |
		| devolucao_data              | null                |
		| devolucao_hora              | null                |
		| devolucao_usuario           | null                |
		| devolucao_loja              | null                |
		| devolucao_pedido            | null                |
		| devolucao_id_item_devolvido | null                |
		| devolucao_id_estoque        | null                |
		| obs                         | teste               |
		| id_nfe_emitente             | 4903                |
		| entrada_tipo                | 0                   |
		| perc_agio                   | 0.0                 |
		| data_emissao_NF_entrada     | null                |