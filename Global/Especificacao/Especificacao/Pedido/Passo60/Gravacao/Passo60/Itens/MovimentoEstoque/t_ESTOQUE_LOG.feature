@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE_LOG

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

Scenario Outline: Verificar log da movimentação
	Given Pedido base
	Given Definir saldo de estoque = "1000" para produto "um"
	Given Definir saldo de estoque = "1000" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_LOG" pedido gerado e produto = "<produto>" e operacao = "OP_ESTOQUE_LOG_VENDA", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo                 | valor      |
		| 003220  | data                  | data atual |
		| 003220  | fabricante            | 003        |
		| 003220  | produto               | 003220     |
		| 003220  | loja_estoque_origem   |            |
		| 003220  | loja_estoque_destino  |            |
		| 003220  | pedido_estoque_origem |            |
		| 003220  | documento             |            |
		| 003220  | complemento           |            |
		| 003220  | id_ordem_servico      |            |
		| 003220  | id_nfe_emitente       | 4903       |
		| 003221  | data                  | data atual |
		| 003221  | fabricante            | 003        |
		| 003221  | produto               | 003221     |
		| 003221  | loja_estoque_origem   |            |
		| 003221  | loja_estoque_destino  |            |
		| 003221  | pedido_estoque_origem |            |
		| 003221  | documento             |            |
		| 003221  | complemento           |            |
		| 003221  | id_ordem_servico      |            |
		| 003221  | id_nfe_emitente       | 4903       |

Scenario Outline: verificar - VDO
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "10"
	When Lista de itens "1" informo "Qtde" = "10"
	Given Definir saldo de estoque = "20" para produto "um"
	Given Definir saldo de estoque = "20" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_LOG" pedido gerado e produto = "<produto>" e operacao = "OP_ESTOQUE_LOG_VENDA", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo               | valor |
		| 003220  | qtde_solicitada     | 10    |
		| 003220  | qtde_atendida       | 10    |
		| 003220  | operacao            | VDA   |
		| 003220  | cod_estoque_origem  | VDA   |
		| 003220  | cod_estoque_destino | VDO   |
		| 003221  | qtde_solicitada     | 10    |
		| 003221  | qtde_atendida       | 10    |
		| 003221  | operacao            | VDA   |
		| 003221  | cod_estoque_origem  | VDA   |
		| 003221  | cod_estoque_destino | VDO   |

Scenario Outline: verificar - SPE
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	#Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "2"
	When Lista de itens "1" informo "Qtde" = "2"
	Given Definir saldo de estoque = "1" para produto "um"
	Given Definir saldo de estoque = "1" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_LOG" pedido gerado e produto = "<produto>" e operacao = "OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo               | valor |
		| 003220  | qtde_solicitada     | 1     |
		| 003220  | qtde_atendida       | 1     |
		| 003220  | operacao            | VSP   |
		| 003220  | cod_estoque_origem  |       |
		| 003220  | cod_estoque_destino | SPE   |
		| 003221  | qtde_solicitada     | 1     |
		| 003221  | qtde_atendida       | 1     |
		| 003221  | operacao            | VSP   |
		| 003221  | cod_estoque_origem  |       |
		| 003221  | cod_estoque_destino | SPE   |

Scenario Outline: Verificar usuario e número de pedido - magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Definir saldo de estoque = "1000" para produto "um"
	Given Definir saldo de estoque = "1000" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_LOG" pedido gerado e produto = "<produto>" e operacao = "OP_ESTOQUE_LOG_VENDA", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo                  | valor   |
		| 003220  | pedido_estoque_destino | 176368N |
		| 003220  | usuario                | USRMAG  |
		| 003221  | pedido_estoque_destino | 176368N |
		| 003221  | usuario                | USRMAG  |

Scenario Outline: Verificar usuario e número de pedido - Loja
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Definir saldo de estoque = "1000" para produto "um"
	Given Definir saldo de estoque = "1000" para produto "dois"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_LOG" pedido gerado e produto = "<produto>" e operacao = "OP_ESTOQUE_LOG_VENDA", verificar campo "<campo>" = "<valor>"

	Examples:
		| produto | campo                  | valor   |
		| 003220  | usuario                | USRLOJA |
		| 003220  | pedido_estoque_destino | 176368N |
		| 003221  | usuario                | USRLOJA |
		| 003221  | pedido_estoque_destino | 176368N |