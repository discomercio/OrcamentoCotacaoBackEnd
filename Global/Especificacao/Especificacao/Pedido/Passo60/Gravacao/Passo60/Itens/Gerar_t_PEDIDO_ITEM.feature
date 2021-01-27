@ignore
Feature: Gerar_t_PEDIDO_ITEM

Scenario Outline: Gerar_t_PEDIDO_ITEM
	#loja/PedidoNovoConfirma.asp
	#de linha 2088
	#s="SELECT * FROM t_PEDIDO_ITEM WHERE pedido='X'"
	#até linha
	#rs("descontinuado") = .descontinuado
	#rs.Update
	Given Pedido base
	Then Sem nehum erro
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "pedido gerado" e campo "sequencia" = <sequencia>, verificar campo "<campo>" = "<valor>"

	Examples:
		| Sequencia | campo                           | valor                                       |
		| 1         | pedido                          | pedido gerado                               |
		| 1         | fabricante                      | 003                                         |
		| 1         | produto                         | 003220                                      |
		| 1         | qtde                            | 2                                           |
		| 1         | desc_dado                       | 0                                           |
		| 1         | preco_venda                     | 659.600                                     |
		| 1         | preco_fabricante                | 659.300                                     |
		| 1         | preco_lista                     | 659.600                                     |
		| 1         | margem                          | 0.0                                         |
		| 1         | desc_max                        | 8.0                                         |
		| 1         | comissao                        | 0.0                                         |
		| 1         | descricao                       | Evap. AR09MVSPBGMNAZ Inverter - FRIO        |
		| 1         | ean                             | 7892509093149                               |
		| 1         | grupo                           | INV                                         |
		| 1         | peso                            | 10.2                                        |
		| 1         | qtde_volumes                    | 1                                           |
		| 1         | abaixo_min_status               | 0                                           |
		| 1         | abaixo_min_autorizacao          |                                             |
		| 1         | abaixo_min_autorizador          |                                             |
		| 1         | sequencia                       | 1                                           |
		| 1         | markup_fabricante               | 0                                           |
		| 1         | preco_NF                        | 694.0500                                    |
		| 1         | abaixo_min_superv_autorizador   |                                             |
		| 1         | vl_custo2                       | 0.0000                                      |
		| 1         | descricao_html                  | Evap. <b>AR09MVS</b>PBGMNAZ Inverter - FRIO |
		| 1         | custoFinancFornecCoeficiente    | 1.0527                                      |
		| 1         | custoFinancFornecPrecoListaBase | 626.5800                                    |
		| 1         | cubagem                         | 0.09                                        |
		| 1         | ncm                             | 84151011                                    |
		| 1         | cst                             | 000                                         |
		| 1         | separacao_rel_nsu               | 0                                           |
		| 1         | separacao_data                  | null                                        |
		| 1         | separacao_data_hora             | null                                        |
		| 1         | separacao_deposito_zona_id      | 0                                           |
		| 1         | descontinuado                   |                                             |
		| 1         | subgrupo                        | null                                        |
		| 2         | pedido                          | pedido gerado                               |
		| 2         | fabricante                      | 003                                         |
		| 2         | produto                         | 003221                                      |
		| 2         | qtde                            | 2                                           |
		| 2         | desc_dado                       | 0                                           |
		| 2         | preco_venda                     | 989.4000                                    |
		| 2         | preco_fabricante                | 988.9500                                    |
		| 2         | preco_lista                     | 939.8700                                    |
		| 2         | margem                          | 0.0                                         |
		| 2         | desc_max                        | 8.0                                         |
		| 2         | comissao                        | 0.0                                         |
		| 2         | descricao                       | Cond. AR09MVSPBGMXAZ Inverter - FRIO        |
		| 2         | ean                             | 7892509093156                               |
		| 2         | grupo                           | INV                                         |
		| 2         | peso                            | 29.5                                        |
		| 2         | qtde_volumes                    | 1                                           |
		| 2         | abaixo_min_status               | 0                                           |
		| 2         | abaixo_min_autorizacao          |                                             |
		| 2         | abaixo_min_autorizador          |                                             |
		| 2         | sequencia                       | 2                                           |
		| 2         | markup_fabricante               | 0                                           |
		| 2         | preco_NF                        | 1041.0700                                   |
		| 2         | abaixo_min_superv_autorizador   |                                             |
		| 2         | vl_custo2                       | 0.0000                                      |
		| 2         | descricao_html                  | Cond. <b>AR09MVS</b>PBGMXAZ Inverter - FRIO |
		| 2         | custoFinancFornecCoeficiente    | 1.0527                                      |
		| 2         | custoFinancFornecPrecoListaBase | 939.8700                                    |
		| 2         | cubagem                         | 0.19                                        |
		| 2         | ncm                             | 84151011                                    |
		| 2         | cst                             | 000                                         |
		| 2         | separacao_rel_nsu               | 0                                           |
		| 2         | separacao_data                  | null                                        |
		| 2         | separacao_data_hora             | null                                        |
		| 2         | separacao_deposito_zona_id      | 0                                           |
		| 2         | descontinuado                   |                                             |
		| 2         | subgrupo                        | null                                        |