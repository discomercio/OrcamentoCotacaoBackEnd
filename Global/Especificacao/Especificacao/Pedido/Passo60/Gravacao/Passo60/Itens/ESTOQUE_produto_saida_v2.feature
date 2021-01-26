@ignore
Feature: ESTOQUE_produto_saida_v2

Scenario: ESTOQUE_produto_saida_v2
	#Testar rotina ESTOQUE_produto_saida_v2
	When Fazer esta validação

Scenario Outline: Verificar estoque item
	Then afazer esse

#vamos buscar pelo número do pedido gerado
#verificar os dados da tabela
Scenario Outline: verfificar estoque movimento
	Given Pedido base
	Then Sem nehum erro
	And pegar o número do pedido gerado
	And Tabela "t_ESTOQUE_MOVIMENTO" registro com campo "pedido" = "pedido gerado", verificar campo "<campo>" = "<valor>"

	Examples:
		| pedido | campo            | valor               |
		| pedido | id_movimento     | 000003020931        |
		| pedido | data             | 2021-01-20 00:00:00 |
		| pedido | hora             | 174738              |
		| pedido | usuario          | HAMILTON            |
		| pedido | id_estoque       | 000000119328        |
		| pedido | fabricante       | 003                 |
		| pedido | produto          | 003221              |
		| pedido | qtde             | 2                   |
		| pedido | operacao         | VDA                 |
		| pedido | estoque          | VDO                 |
		| pedido | pedido           | 222266N             |
		| pedido | loja             |                     |
		| pedido | anulado_status   | 0                   |
		| pedido | anulado_data     | null                |
		| pedido | anulado_hora     | null                |
		| pedido | anulado_usuario  | null                |
		| pedido | timestamp        | [x.                 |
		| pedido | kit              | 0                   |
		| pedido | kit_id_estoque   |                     |
		| pedido | id_ordem_servico | null                |
