@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE_MOVIMENTO

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

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
Scenario Outline: Verificar estoque movimento
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "<produto>", verificar campo "<campo>" = "<valor>"

	#esses campos ficam mudando
	#| 003220  | id_movimento   | 000002801471 |
	#| 003221  | id_movimento   | 000002801470 |
	Examples:
		| produto | campo          | valor  |
		| 003220  | id_estoque     |        |
		| 003220  | fabricante     | 003    |
		| 003220  | produto        | 003220 |
		| 003220  | qtde           | 2      |
		| 003220  | operacao       | VDA    |
		| 003220  | estoque        | SPE    |
		| 003220  | loja           |        |
		| 003220  | anulado_status | 0      |
		| 003220  | kit            | 0      |
		| 003220  | kit_id_estoque |        |
		| 003221  | id_estoque     |        |
		| 003221  | fabricante     | 003    |
		| 003221  | produto        | 003221 |
		| 003221  | qtde           | 2      |
		| 003221  | operacao       | VDA    |
		| 003221  | estoque        | SPE    |
		| 003221  | loja           |        |
		| 003221  | anulado_status | 0      |
		| 003221  | kit            | 0      |
		| 003221  | kit_id_estoque |        |

Scenario Outline: Verificar estoque movimento usuario - magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "<produto>", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo   | valor  |
		| 003220  | usuario | USRMAG |
		| 003221  | usuario | USRMAG |

Scenario Outline: Verificar estoque movimento usuario - loja
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_MOVIMENTO" registro pai e produto = "<produto>", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo   | valor   |
		| 003220  | usuario | USRLOJA |
		| 003221  | usuario | USRLOJA |

#não conseguiu movimentar a quantidade suficiente
#           if (qtde_movimentada < (qtde_a_sair - qtde_autorizada_sem_presenca))
#           {
#               lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
#                   ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) +
#                   " unidades no estoque para poder atender ao pedido.");
#               return retorno = false;
#           }
#When precisa alterar para que a qtde_movimentada seja menor que (qtde_a_sair - qtde_autorizada_sem_presenca)
#Then Erro "Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " + ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) + " unidades no estoque para poder atender ao pedido."
#And afazer - ajustar a mensagem de erro
@ignore
Scenario: Verificar qtde movimentada - erro
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base	
	When Lista de itens "0" informo "Qtde" = "100"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Given Definir saldo de estoque = "49" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "100", qtde_autorizada_sem_presenca = "49"
	Then Erro "Ajustar teste"
