@ignore
Feature: ESTOQUE_produto_saida_v2

Scenario: ESTOQUE_produto_saida_v2
	#Testar rotina ESTOQUE_produto_saida_v2
	When Fazer esta validação

Scenario Outline: Verificar estoque item
	Given Pedido base
	Then Sem nehum erro
	And pegar o número do pedido gerado
	And pegar o id_estoque na tabela t_ESTOQUE_MOVIMENTO
	And Tabela "t_ESTOQUE_ITEM" registro com campo "id_estoque" = "id_estoque da t_ESTOQUE_MOVIMENTO", verificar campo "<campo>" = "<valor>"

	Examples:
		| id_estoque | sequencia | campo                             | valor               |
		| id_estoque | 1         | id_estoque                        | 000000119328        |
		| id_estoque | 1         | fabricante                        | 003                 |
		| id_estoque | 1         | produto                           | 003220              |
		| id_estoque | 1         | qtde                              | 100                 |
		| id_estoque | 1         | preco_fabricante                  | 670.8500            |
		| id_estoque | 1         | qtde_utilizada                    | 100                 |
		| id_estoque | 1         | data_ult_movimento                | 2021-01-20 00:00:00 |
		| id_estoque | 1         | sequencia                         | 1                   |
		| id_estoque | 1         | [timestamp]                       | [xS                 |
		| id_estoque | 1         | vl_custo2                         | 670.8500            |
		| id_estoque | 1         | vl_BC_ICMS_ST                     | 0.0000              |
		| id_estoque | 1         | vl_ICMS_ST                        | 0.0000              |
		| id_estoque | 1         | ncm                               | 84151011            |
		| id_estoque | 1         | cst                               | 000                 |
		| id_estoque | 1         | st_ncm_cst_herdado_tabela_produto | 0                   |
		| id_estoque | 1         | ean                               | null                |
		| id_estoque | 1         | aliq_ipi                          | 0.0                 |
		| id_estoque | 1         | aliq_icms                         | 0.0                 |
		| id_estoque | 1         | vl_ipi                            | 0.0000              |
		| id_estoque | 1         | preco_origem                      | null                |
		| id_estoque | 1         | produto_xml                       | null                |
		| id_estoque | 1         | vl_frete                          | null                |
		| id_estoque | 2         | id_estoque                        | 000000119328        |
		| id_estoque | 2         | fabricante                        | 003                 |
		| id_estoque | 2         | produto                           | 003221              |
		| id_estoque | 2         | qtde                              | 100                 |
		| id_estoque | 2         | preco_fabricante                  | 1006.2800           |
		| id_estoque | 2         | qtde_utilizada                    | 100                 |
		| id_estoque | 2         | data_ult_movimento                | 2021-01-20 00:00:00 |
		| id_estoque | 2         | sequencia                         | 2                   |
		| id_estoque | 2         | [timestamp]                       | [xZ                 |
		| id_estoque | 2         | vl_custo2                         | 1006.2800           |
		| id_estoque | 2         | vl_BC_ICMS_ST                     | 0.0000              |
		| id_estoque | 2         | vl_ICMS_ST                        | 0.0000              |
		| id_estoque | 2         | ncm                               | 84151011            |
		| id_estoque | 2         | cst                               | 000                 |
		| id_estoque | 2         | st_ncm_cst_herdado_tabela_produto | 0                   |
		| id_estoque | 2         | ean                               | null                |
		| id_estoque | 2         | aliq_ipi                          | 0.0                 |
		| id_estoque | 2         | aliq_icms                         | 0.0                 |
		| id_estoque | 2         | vl_ipi                            | 0.0000              |
		| id_estoque | 2         | preco_origem                      | null                |
		| id_estoque | 2         | produto_xml                       | null                |
		| id_estoque | 2         | vl_frete                          | null                |

#vamos buscar pelo número do pedido gerado
#verificar os dados da tabela
Scenario Outline: verfificar estoque movimento
	Given Pedido base
	Then Sem nehum erro
	And pegar o número do pedido gerado
	And Tabela "t_ESTOQUE_MOVIMENTO" registro com campo "pedido" = "pedido gerado", verificar campo "<campo>" = "<valor>"

	Examples:
		| pedido | produto | campo            | valor               |
		| pedido | 003221  | id_movimento     | 000003020931        |
		| pedido | 003221  | data             | 2021-01-20 00:00:00 |
		| pedido | 003221  | hora             | 174738              |
		| pedido | 003221  | usuario          | HAMILTON            |
		| pedido | 003221  | id_estoque       | 000000119328        |
		| pedido | 003221  | fabricante       | 003                 |
		| pedido | 003221  | produto          | 003221              |
		| pedido | 003221  | qtde             | 2                   |
		| pedido | 003221  | operacao         | VDA                 |
		| pedido | 003221  | estoque          | VDO                 |
		| pedido | 003221  | pedido           | 222266N             |
		| pedido | 003221  | loja             |                     |
		| pedido | 003221  | anulado_status   | 0                   |
		| pedido | 003221  | anulado_data     | null                |
		| pedido | 003221  | anulado_hora     | null                |
		| pedido | 003221  | anulado_usuario  | null                |
		| pedido | 003221  | timestamp        | [x.                 |
		| pedido | 003221  | kit              | 0                   |
		| pedido | 003221  | kit_id_estoque   |                     |
		| pedido | 003221  | id_ordem_servico | null                |
		| pedido | 003220  | id_movimento     | 000003020930        |
		| pedido | 003220  | data             | 2021-01-20 00:00:00 |
		| pedido | 003220  | hora             | 174737              |
		| pedido | 003220  | usuario          | HAMILTON            |
		| pedido | 003220  | id_estoque       | 000000119328        |
		| pedido | 003220  | fabricante       | 003                 |
		| pedido | 003220  | produto          | 003220              |
		| pedido | 003220  | qtde             | 2                   |
		| pedido | 003220  | operacao         | VDA                 |
		| pedido | 003220  | estoque          | VDO                 |
		| pedido | 003220  | pedido           | 222266N             |
		| pedido | 003220  | loja             |                     |
		| pedido | 003220  | anulado_status   | 0                   |
		| pedido | 003220  | anulado_data     | null                |
		| pedido | 003220  | anulado_hora     | null                |
		| pedido | 003220  | anulado_usuario  | null                |
		| pedido | 003220  | timestamp        | [x-                 |
		| pedido | 003220  | kit              | 0                   |
		| pedido | 003220  | kit_id_estoque   |                     |
		| pedido | 003220  | id_ordem_servico | null                |

Scenario Outline: verificar log da movimentação
	Then afazer essa validação

Scenario Outline: verificar alteração de último movimento estoque
	Given Pedido base
	Then Sem nehum erro
	And pegar o número do pedido gerado
	And pegar o id_estoque na tabela t_ESTOQUE_MOVIMENTO
	And Tabela "t_ESTOQUE" registro com campo "id_estoque" = "id_estoque", verificar campo "<campo>" = "<valor>"
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