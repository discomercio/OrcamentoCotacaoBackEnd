@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: ESTOQUE_produto_saida_v2

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

@ignore
Scenario: ESTOQUE_produto_saida_v2
	#Testar rotina ESTOQUE_produto_saida_v2
	#testar o erros que deve retornar
	#NÃO HÁ PRODUTOS SUFICIENTES NO ESTOQUE!!
	#           if ((qtde_a_sair - qtde_autorizada_sem_presenca) > qtde_disponivel)
	#           {
	#               lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
	#                   ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_disponivel) + " unidades no estoque (" +
	#                   UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentesGravacao(id_nfe_emitente, dbGravacao) +
	#                   ") para poder atender ao pedido.");
	#               return false;
	#           }
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"
	Given Pedido base
	Given Definir saldo de estoque = "40" para produto "um"
	When Lista de itens "0" informo "Qtde" = "100"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "100", qtde_autorizada_sem_presenca = "50"
	Then Erro "regex .*Produto 003220 do fabricante 003: faltam 10 unidades no estoque"

#And afazer - Ajustar a mensagem de erro
#não conseguiu movimentar a quantidade suficiente
#           if (qtde_movimentada < (qtde_a_sair - qtde_autorizada_sem_presenca))
#           {
#               lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
#                   ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) +
#                   " unidades no estoque para poder atender ao pedido.");
#               return retorno = false;
#           }
#Given Pedido base
#When precisa alterar para que a qtde_movimentada seja menor que (qtde_a_sair - qtde_autorizada_sem_presenca)
#Then Erro "Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " + ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) + " unidades no estoque para poder atender ao pedido."
#And afazer - ajustar a mensagem de erro
@ignore
Scenario Outline: Verificar estoque item
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_ITEM" verificar campo "<campo>" = "<valor>"

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
Scenario Outline: Verificar estoque movimento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "<produto>", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo          | valor        |
		| 003220  | id_movimento   | 000002801469 |
		#| 003220  | usuario        | USRMAG       |
		| 003220  | id_estoque     |              |
		| 003220  | fabricante     | 003          |
		| 003220  | produto        | 003220       |
		| 003220  | qtde           | 2            |
		| 003220  | operacao       | VDA          |
		| 003220  | estoque        | SPE          |
		| 003220  | loja           |              |
		| 003220  | anulado_status | 0            |
		| 003220  | kit            | 0            |
		| 003220  | kit_id_estoque |              |
		| 003221  | id_movimento   | 000002801470 |
		#| 003221  | usuario        | USRMAG       |
		| 003221  | id_estoque     |              |
		| 003221  | fabricante     | 003          |
		| 003221  | produto        | 003221       |
		| 003221  | qtde           | 2            |
		| 003221  | operacao       | VDA          |
		| 003221  | estoque        | SPE          |
		| 003221  | loja           |              |
		| 003221  | anulado_status | 0            |
		| 003221  | kit            | 0            |
		| 003221  | kit_id_estoque |              |

#CAMPOS DE DATA E HORA PEDIDO
#| 003221 | data             | 2021-01-20 00:00:00 |
#| 003221 | hora             | 174738              |
#| 003220 | data             | 2021-01-20 00:00:00 |
#| 003220 | hora             | 174737              |
#| 003221 | pedido           | 222266N             |
#| 003220 | pedido           | 222266N             |
#####
# CAMPOS NÃO MAPEADOS
#| 003220 | anulado_data     | null | não mapeado
#| 003220 | anulado_hora     | null | não mapeado
#| 003220 | anulado_usuario  | null | não mapeado
#| 003220 | timestamp        | [x-  | não mapeado
#| 003220 | timestamp        | [x.  | não mapeado
#| 003221 | anulado_data     | null | não mapeado
#| 003221 | anulado_hora     | null | não mapeado
#| 003221 | anulado_usuario  | null | não mapeado
#| 003221 | id_ordem_servico | null | não mapeado
#| 003220 | id_ordem_servico | null | não mapeado
@ignore
Scenario Outline: verificar log da movimentação
	Then afazer essa validação

@ignore
Scenario Outline: verificar alteração de último movimento estoque
	Given Pedido base
	Then Sem nenhum erro
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