@ignore
Feature: Preenche_os_campos_do_pedido

#a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
#loja/PedidoNovoConfirma.asp
#de linha 1788
#s = "SELECT * FROM t_PEDIDO WHERE pedido='X'"
#até linha 2057
#rs("id_nfe_emitente") = vEmpresaAutoSplit(iv)
#rs.Update
Scenario Outline: Preenche_os_campos_do_pedido - pedido pai
	#Campos que existem em todos os pedidos: pedido, loja, data, hora
	#Campos que existem somente no pedido base, não nos filhotes:
	#	st_auto_split se tiver filhotes
	#	Campos transferidos: de linha 1800 rs("dt_st_pagto") = Date até 1887 rs("perc_limite_RA_sem_desagio") = perc_limite_RA_sem_desagio
	#Campos que existem somente nos pedidos filhotes, não no base:
	#	linha 1892 rs("st_auto_split") = 1 até 1903 rs("forma_pagto")=""
	#Transfere mais campos: linha 1907 até 2055
	#
	#Gabriel fez o pedido 222267N com o mesmo endereço de entrega do json e com os mesmos produtos
	#e fez split manualmente, verifiquei o o cd é o mesmo
	# nos pedidos que fizeram spli auto os cd são diferentes
	# no meu teste não tinha a opção de selecionar um cd e nem selcionar se era auto
	Given Pedido base
	When Informo "ListaProdutos[0].Qtde" = "100"
	When Informo "ListaProdutos[1].Qtde" = "100"
	When Informo "ValorTotalDestePedidoComRA" = "173512.00"
	When Informo "VlTotalDestePedido" = ""
	When Informo "FormaPagtoCriacao.C_pc_valor" = "173512.00"
	Then Sem nehum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "pedido gerado", verificar campo "<campo>" = "<valor>"

	Examples:
		| pedido | campo                                          | valor                           |
		| pedido | pedido                                         | pedido pai                      |
		| pedido | loja                                           | 202                             |
		| pedido | data                                           | 2021-01-20 00:00:00             |
		| pedido | hora                                           | 183128                          |
		| pedido | id_cliente                                     | 000000684226                    |
		| pedido | midia                                          |                                 |
		| pedido | servicos                                       |                                 |
		| pedido | vl_servicos                                    | 0.0000                          |
		| pedido | vendedor                                       | HAMILTON                        |
		| pedido | st_entrega                                     | SPL                             |
		| pedido | entregue_data                                  |                                 |
		| pedido | entregue_usuario                               |                                 |
		| pedido | cancelado_data                                 |                                 |
		| pedido | cancelado_usuario                              |                                 |
		| pedido | st_pagto                                       | N                               |
		| pedido | st_recebido                                    |                                 |
		| pedido | obs_1                                          |                                 |
		| pedido | obs_2                                          |                                 |
		| pedido | qtde_parcelas                                  | 1                               |
		| pedido | forma_pagto                                    |                                 |
		| pedido | vl_total_familia                               | 173512.0000                     |
		| pedido | vl_pago_familia                                | 0.0000                          |
		| pedido | split_status                                   | 0                               |
		| pedido | split_data                                     |                                 |
		| pedido | split_hora                                     |                                 |
		| pedido | split_usuario                                  |                                 |
		| pedido | a_entregar_status                              | 0                               |
		| pedido | a_entregar_data_marcada                        |                                 |
		| pedido | a_entregar_data                                |                                 |
		| pedido | a_entregar_hora                                |                                 |
		| pedido | a_entregar_usuario                             |                                 |
		| pedido | timestamp                                      | [xL                             |
		| pedido | loja_indicou                                   |                                 |
		| pedido | comissao_loja_indicou                          | 0.0                             |
		| pedido | venda_externa                                  | 0                               |
		| pedido | vl_frete                                       | 0.0000                          |
		| pedido | transportadora_id                              |                                 |
		| pedido | transportadora_data                            |                                 |
		| pedido | transportadora_usuario                         |                                 |
		| pedido | analise_credito                                | 0                               |
		| pedido | analise_credito_data                           |                                 |
		| pedido | analise_credito_usuario                        |                                 |
		| pedido | tipo_parcelamento                              | 2                               |
		| pedido | av_forma_pagto                                 | 0                               |
		| pedido | pc_qtde_parcelas                               | 1                               |
		| pedido | pc_valor_parcela                               | 173512.0000                     |
		| pedido | pce_forma_pagto_entrada                        | 0                               |
		| pedido | pce_forma_pagto_prestacao                      | 0                               |
		| pedido | pce_entrada_valor                              | 0.0000                          |
		| pedido | pce_prestacao_qtde                             | 0                               |
		| pedido | pce_prestacao_valor                            | 0.0000                          |
		| pedido | pce_prestacao_periodo                          | 0                               |
		| pedido | pse_forma_pagto_prim_prest                     | 0                               |
		| pedido | pse_forma_pagto_demais_prest                   | 0                               |
		| pedido | pse_prim_prest_valor                           | 0.0000                          |
		| pedido | pse_prim_prest_apos                            | 0                               |
		| pedido | pse_demais_prest_qtde                          | 0                               |
		| pedido | pse_demais_prest_valor                         | 0.0000                          |
		| pedido | pse_demais_prest_periodo                       | 0                               |
		| pedido | pu_forma_pagto                                 | 0                               |
		| pedido | pu_valor                                       | 0.0000                          |
		| pedido | pu_vencto_apos                                 | 0                               |
		| pedido | indicador                                      |                                 |
		| pedido | vl_total_NF                                    | 173512.0000                     |
		| pedido | vl_total_RA                                    | 0.0000                          |
		| pedido | perc_RT                                        | 0.0                             |
		| pedido | st_orc_virou_pedido                            | 0                               |
		| pedido | orcamento                                      |                                 |
		| pedido | orcamentista                                   |                                 |
		| pedido | comissao_paga                                  | 0                               |
		| pedido | comissao_paga_ult_op                           |                                 |
		| pedido | comissao_paga_data                             |                                 |
		| pedido | comissao_paga_usuario                          |                                 |
		| pedido | perc_desagio_RA                                | 0.0                             |
		| pedido | perc_limite_RA_sem_desagio                     | 0.0                             |
		| pedido | vl_total_RA_liquido                            | 0.0000                          |
		| pedido | st_tem_desagio_RA                              | 0                               |
		| pedido | qtde_parcelas_desagio_RA                       | 0                               |
		| pedido | transportadora_num_coleta                      |                                 |
		| pedido | transportadora_contato                         |                                 |
		| pedido | st_end_entrega                                 | 1                               |
		| pedido | EndEtg_endereco                                | Rua Professor Fábio Fanucchi    |
		| pedido | EndEtg_bairro                                  | Jardim São Paulo(Zona Norte)    |
		| pedido | EndEtg_cidade                                  | São Paulo                       |
		| pedido | EndEtg_uf                                      | SP                              |
		| pedido | EndEtg_cep                                     | 02045080                        |
		| pedido | st_etg_imediata                                | 2                               |
		| pedido | etg_imediata_data                              | 2021-01-20 18:31:31             |
		| pedido | etg_imediata_usuario                           | HAMILTON                        |
		| pedido | frete_status                                   | 0                               |
		| pedido | frete_valor                                    | 0.0000                          |
		| pedido | frete_data                                     |                                 |
		| pedido | frete_usuario                                  |                                 |
		| pedido | StBemUsoConsumo                                | 1                               |
		| pedido | PedidoRecebidoStatus                           | 0                               |
		| pedido | PedidoRecebidoData                             |                                 |
		| pedido | PedidoRecebidoUsuarioUltAtualiz                |                                 |
		| pedido | PedidoRecebidoDtHrUltAtualiz                   |                                 |
		| pedido | InstaladorInstalaStatus                        | 2                               |
		| pedido | InstaladorInstalaUsuarioUltAtualiz             | HAMILTON                        |
		| pedido | InstaladorInstalaDtHrUltAtualiz                | 2021-01-20 18:31:31             |
		| pedido | custoFinancFornecTipoParcelamento              | SE                              |
		| pedido | custoFinancFornecQtdeParcelas                  | 1                               |
		| pedido | BoletoConfeccionadoStatus                      | 0                               |
		| pedido | BoletoConfeccionadoData                        |                                 |
		| pedido | GarantiaIndicadorStatus                        | 0                               |
		| pedido | GarantiaIndicadorUsuarioUltAtualiz             | HAMILTON                        |
		| pedido | GarantiaIndicadorDtHrUltAtualiz                | 2021-01-20 18:31:31             |
		| pedido | EndEtg_endereco_numero                         | 97                              |
		| pedido | EndEtg_endereco_complemento                    |                                 |
		| pedido | romaneio_status                                | 0                               |
		| pedido | romaneio_data                                  |                                 |
		| pedido | romaneio_data_hora                             |                                 |
		| pedido | romaneio_usuario                               |                                 |
		| pedido | danfe_impressa_status                          | 0                               |
		| pedido | danfe_impressa_data                            |                                 |
		| pedido | danfe_impressa_data_hora                       |                                 |
		| pedido | danfe_impressa_usuario                         |                                 |
		| pedido | transportadora_conferente                      |                                 |
		| pedido | transportadora_motorista                       |                                 |
		| pedido | transportadora_placa_veiculo                   |                                 |
		| pedido | perc_desagio_RA_liquida                        | 30.0                            |
		| pedido | indicador_editado_manual_status                | 0                               |
		| pedido | indicador_editado_manual_data_hora             |                                 |
		| pedido | indicador_editado_manual_usuario               |                                 |
		| pedido | indicador_editado_manual_indicador_original    |                                 |
		| pedido | permite_RA_status                              | 0                               |
		| pedido | st_violado_permite_RA_status                   | 0                               |
		| pedido | dt_hr_violado_permite_RA_status                |                                 |
		| pedido | usuario_violado_permite_RA_status              |                                 |
		| pedido | opcao_possui_RA                                | -                               |
		| pedido | tamanho_num_pedido                             | 7                               |
		| pedido | pedido_base                                    | pedido gerado                   |
		| pedido | numero_loja                                    | 202                             |
		| pedido | data_hora                                      | 2021-01-20 18:31:28             |
		| pedido | st_forma_pagto_somente_cartao                  | 1                               |
		| pedido | endereco_memorizado_status                     | 1                               |
		| pedido | endereco_logradouro                            | Rua Francisco Pecoraro          |
		| pedido | endereco_bairro                                | Água Fria                       |
		| pedido | endereco_cidade                                | São Paulo                       |
		| pedido | endereco_uf                                    | SP                              |
		| pedido | endereco_cep                                   | 02408150                        |
		| pedido | endereco_numero                                | 97                              |
		| pedido | endereco_complemento                           | casa 01                         |
		| pedido | analise_endereco_tratar_status                 | 1                               |
		| pedido | analise_endereco_tratado_status                | 0                               |
		| pedido | analise_endereco_tratado_data                  |                                 |
		| pedido | analise_endereco_tratado_data_hora             |                                 |
		| pedido | analise_endereco_tratado_usuario               |                                 |
		| pedido | analise_credito_data_sem_hora                  |                                 |
		| pedido | cancelado_auto_status                          | 0                               |
		| pedido | cancelado_auto_data                            |                                 |
		| pedido | cancelado_auto_data_hora                       |                                 |
		| pedido | cancelado_auto_motivo                          |                                 |
		| pedido | obs_3                                          |                                 |
		| pedido | danfe_a_imprimir_status                        | 0                               |
		| pedido | danfe_a_imprimir_data_hora                     |                                 |
		| pedido | danfe_a_imprimir_usuario                       |                                 |
		| pedido | transportadora_selecao_auto_status             | 0                               |
		| pedido | transportadora_selecao_auto_cep                |                                 |
		| pedido | transportadora_selecao_auto_tipo_endereco      | 0                               |
		| pedido | transportadora_selecao_auto_transportadora     |                                 |
		| pedido | transportadora_selecao_auto_data_hora          |                                 |
		| pedido | pedido_bs_x_at                                 |                                 |
		| pedido | cancelado_data_hora                            |                                 |
		| pedido | cancelado_codigo_motivo                        |                                 |
		| pedido | cancelado_codigo_sub_motivo                    |                                 |
		| pedido | cancelado_motivo                               |                                 |
		| pedido | EndEtg_obs                                     |                                 |
		| pedido | pedido_bs_x_ac                                 |                                 |
		| pedido | pedido_bs_x_ac_reverso                         |                                 |
		| pedido | EndEtg_cod_justificativa                       | 003                             |
		| pedido | pedido_bs_x_marketplace                        |                                 |
		| pedido | marketplace_codigo_origem                      |                                 |
		| pedido | id_nfe_emitente                                | 4903                            |
		| pedido | st_pedido_novo_analise_credito_msg_alerta      | 0                               |
		| pedido | dt_hr_pedido_novo_analise_credito_msg_alerta   |                                 |
		| pedido | st_forma_pagto_possui_parcela_cartao           | 1                               |
		| pedido | vl_previsto_cartao                             | 173512.0000                     |
		| pedido | NFe_texto_constar                              |                                 |
		| pedido | NFe_xPed                                       |                                 |
		| pedido | MarketplacePedidoRecebidoRegistrarStatus       | 0                               |
		| pedido | MarketplacePedidoRecebidoRegistrarDataRecebido |                                 |
		| pedido | MarketplacePedidoRecebidoRegistrarDataHora     |                                 |
		| pedido | MarketplacePedidoRecebidoRegistrarUsuario      |                                 |
		| pedido | MarketplacePedidoRecebidoRegistradoStatus      | 0                               |
		| pedido | MarketplacePedidoRecebidoRegistradoDataHora    |                                 |
		| pedido | MarketplacePedidoRecebidoRegistradoUsuario     |                                 |
		| pedido | st_auto_split                                  | 0                               |
		| pedido | analise_credito_pendente_vendas_motivo         |                                 |
		| pedido | usuario_cadastro                               | HAMILTON                        |
		| pedido | plataforma_origem_pedido                       | 0                               |
		| pedido | magento_installer_commission_value             | 0.0000                          |
		| pedido | magento_installer_commission_discount          | 0.0000                          |
		| pedido | magento_shipping_amount                        | 0.0000                          |
		| pedido | dt_st_pagto                                    | 2021-01-20 00:00:00             |
		| pedido | dt_hr_st_pagto                                 | 2021-01-20 18:31:30             |
		| pedido | usuario_st_pagto                               | HAMILTON                        |
		| pedido | pc_maquineta_qtde_parcelas                     | 0                               |
		| pedido | pc_maquineta_valor_parcela                     | 0.0000                          |
		| pedido | sistema_responsavel_cadastro                   | 1                               |
		| pedido | sistema_responsavel_atualizacao                | 1                               |
		| pedido | num_obs_2                                      | 0                               |
		| pedido | num_obs_3                                      | 0                               |
		| pedido | PrevisaoEntregaData                            |                                 |
		| pedido | PrevisaoEntregaUsuarioUltAtualiz               |                                 |
		| pedido | PrevisaoEntregaDtHrUltAtualiz                  |                                 |
		| pedido | st_forma_pagto_possui_parcela_cartao_maquineta | 0                               |
		| pedido | st_memorizacao_completa_enderecos              | 1                               |
		| pedido | endereco_email                                 | gabriel.prada.teodoro@gmail.com |
		| pedido | endereco_email_xml                             | teste@xml.com                   |
		| pedido | endereco_nome                                  | Gabriel Prada Teodoro           |
		| pedido | endereco_ddd_res                               | 11                              |
		| pedido | endereco_tel_res                               | 25321634                        |
		| pedido | endereco_ddd_com                               | 11                              |
		| pedido | endereco_tel_com                               | 55788755                        |
		| pedido | endereco_ramal_com                             | 12                              |
		| pedido | endereco_ddd_cel                               | 11                              |
		| pedido | endereco_tel_cel                               | 981603313                       |
		| pedido | endereco_ddd_com_2                             |                                 |
		| pedido | endereco_tel_com_2                             |                                 |
		| pedido | endereco_ramal_com_2                           |                                 |
		| pedido | endereco_tipo_pessoa                           | PF                              |
		| pedido | endereco_cnpj_cpf                              | 35270445824                     |
		| pedido | endereco_contribuinte_icms_status              | 2                               |
		| pedido | endereco_produtor_rural_status                 | 2                               |
		| pedido | endereco_ie                                    | 361.289.183.714                 |
		| pedido | endereco_rg                                    | 304480484                       |
		| pedido | endereco_contato                               |                                 |
		| pedido | EndEtg_email                                   | gabriel.prada.teodoro@gmail.com |
		| pedido | EndEtg_email_xml                               | teste@xml.com                   |
		| pedido | EndEtg_nome                                    | Gabriel Prada Teodoro           |
		| pedido | EndEtg_ddd_res                                 |                                 |
		| pedido | EndEtg_tel_res                                 |                                 |
		| pedido | EndEtg_ddd_com                                 |                                 |
		| pedido | EndEtg_tel_com                                 |                                 |
		| pedido | EndEtg_ramal_com                               |                                 |
		| pedido | EndEtg_ddd_cel                                 |                                 |
		| pedido | EndEtg_tel_cel                                 |                                 |
		| pedido | EndEtg_ddd_com_2                               |                                 |
		| pedido | EndEtg_tel_com_2                               |                                 |
		| pedido | EndEtg_ramal_com_2                             |                                 |
		| pedido | EndEtg_tipo_pessoa                             | PF                              |
		| pedido | EndEtg_cnpj_cpf                                | 35270445824                     |
		| pedido | EndEtg_contribuinte_icms_status                | 2                               |
		| pedido | EndEtg_produtor_rural_status                   | 2                               |
		| pedido | EndEtg_ie                                      | 361.289.183.714                 |
		| pedido | EndEtg_rg                                      | 304480484                       |
		| pedido | endereco_nome_iniciais_em_maiusculas           | Gabriel Prada Teodoro           |
		| pedido | EndEtg_nome_iniciais_em_maiusculas             | Gabriel Prada Teodoro           |


Scenario: perc_desagio_RA_liquida
#gravado no pai e nos filhotes, depende da loja (NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca é gravado)
#
#loja/PedidoNovoConfirma.asp
#			'01/02/2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, portanto, não devem ter deságio do RA
#			if (Cstr(loja) <> Cstr(NUMERO_LOJA_ECOMMERCE_AR_CLUBE)) And (Not blnMagentoPedidoComIndicador) then rs("perc_desagio_RA_liquida") = getParametroPercDesagioRALiquida
#set rP = get_registro_t_parametro(ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA)
#if Trim("" & rP.campo_real) <> "" then getParametroPercDesagioRALiquida = rP.campo_real
#s = "SELECT " & _
#		"*" & _
#	" FROM t_PARAMETRO" & _
#	" WHERE" & _
#		" (id = '" & id_registro & "')"
#


Scenario Outline: Preenche_os_campos_do_pedido - pedido filhote
	Given Pedido base
	When Informo "ListaProdutos[0].Qtde" = "100"
	When Informo "ListaProdutos[1].Qtde" = "100"
	When Informo "ValorTotalDestePedidoComRA" = "173512.00"
	When Informo "VlTotalDestePedido" = ""
	When Informo "FormaPagtoCriacao.C_pc_valor" = "173512.00"
	Then Sem nehum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "pedido gerado + -A", verificar campo "<campo>" = "<valor>"

	Examples:
		| pedido | campo                                          | valor                           |
		| pedido | pedido                                         | 222267N-A                       |
		| pedido | loja                                           | 202                             |
		| pedido | data                                           | 2021-01-20 00:00:00             |
		| pedido | hora                                           | 183128                          |
		| pedido | id_cliente                                     | 000000684226                    |
		| pedido | midia                                          |                                 |
		| pedido | servicos                                       |                                 |
		| pedido | vl_servicos                                    | 0.0000                          |
		| pedido | vendedor                                       | HAMILTON                        |
		| pedido | st_entrega                                     | SEP                             |
		| pedido | entregue_data                                  | null                            |
		| pedido | entregue_usuario                               | null                            |
		| pedido | cancelado_data                                 | null                            |
		| pedido | cancelado_usuario                              | null                            |
		| pedido | st_pagto                                       |                                 |
		| pedido | st_recebido                                    |                                 |
		| pedido | obs_1                                          |                                 |
		| pedido | obs_2                                          |                                 |
		| pedido | qtde_parcelas                                  | 0                               |
		| pedido | forma_pagto                                    |                                 |
		| pedido | vl_total_familia                               | 0.0000                          |
		| pedido | vl_pago_familia                                | 0.0000                          |
		| pedido | split_status                                   | 1                               |
		| pedido | split_data                                     | 2021-01-20 00:00:00             |
		| pedido | split_hora                                     | 183840                          |
		| pedido | split_usuario                                  | HAMILTON                        |
		| pedido | a_entregar_status                              | 0                               |
		| pedido | a_entregar_data_marcada                        | null                            |
		| pedido | a_entregar_data                                | null                            |
		| pedido | a_entregar_hora                                | null                            |
		| pedido | a_entregar_usuario                             | null                            |
		| pedido | timestamp                                      | [xM                             |
		| pedido | loja_indicou                                   |                                 |
		| pedido | comissao_loja_indicou                          | 0.0                             |
		| pedido | venda_externa                                  | 0                               |
		| pedido | vl_frete                                       | 0.0000                          |
		| pedido | transportadora_id                              | null                            |
		| pedido | transportadora_data                            | null                            |
		| pedido | transportadora_usuario                         | null                            |
		| pedido | analise_credito                                | 0                               |
		| pedido | analise_credito_data                           | null                            |
		| pedido | analise_credito_usuario                        | null                            |
		| pedido | tipo_parcelamento                              | 0                               |
		| pedido | av_forma_pagto                                 | 0                               |
		| pedido | pc_qtde_parcelas                               | 0                               |
		| pedido | pc_valor_parcela                               | 0.0000                          |
		| pedido | pce_forma_pagto_entrada                        | 0                               |
		| pedido | pce_forma_pagto_prestacao                      | 0                               |
		| pedido | pce_entrada_valor                              | 0.0000                          |
		| pedido | pce_prestacao_qtde                             | 0                               |
		| pedido | pce_prestacao_valor                            | 0.0000                          |
		| pedido | pce_prestacao_periodo                          | 0                               |
		| pedido | pse_forma_pagto_prim_prest                     | 0                               |
		| pedido | pse_forma_pagto_demais_prest                   | 0                               |
		| pedido | pse_prim_prest_valor                           | 0.0000                          |
		| pedido | pse_prim_prest_apos                            | 0                               |
		| pedido | pse_demais_prest_qtde                          | 0                               |
		| pedido | pse_demais_prest_valor                         | 0.0000                          |
		| pedido | pse_demais_prest_periodo                       | 0                               |
		| pedido | pu_forma_pagto                                 | 0                               |
		| pedido | pu_valor                                       | 0.0000                          |
		| pedido | pu_vencto_apos                                 | 0                               |
		| pedido | indicador                                      |                                 |
		| pedido | vl_total_NF                                    | 0.0000                          |
		| pedido | vl_total_RA                                    | 0.0000                          |
		| pedido | perc_RT                                        | 0.0                             |
		| pedido | st_orc_virou_pedido                            | 0                               |
		| pedido | orcamento                                      |                                 |
		| pedido | orcamentista                                   |                                 |
		| pedido | comissao_paga                                  | 0                               |
		| pedido | comissao_paga_ult_op                           | null                            |
		| pedido | comissao_paga_data                             | null                            |
		| pedido | comissao_paga_usuario                          | null                            |
		| pedido | perc_desagio_RA                                | 0.0                             |
		| pedido | perc_limite_RA_sem_desagio                     | 0.0                             |
		| pedido | vl_total_RA_liquido                            | 0.0000                          |
		| pedido | st_tem_desagio_RA                              | 0                               |
		| pedido | qtde_parcelas_desagio_RA                       | 0                               |
		| pedido | transportadora_num_coleta                      | null                            |
		| pedido | transportadora_contato                         | null                            |
		| pedido | st_end_entrega                                 | 1                               |
		| pedido | EndEtg_endereco                                | Rua Professor Fábio Fanucchi    |
		| pedido | EndEtg_bairro                                  | Jardim São Paulo(Zona Norte)    |
		| pedido | EndEtg_cidade                                  | São Paulo                       |
		| pedido | EndEtg_uf                                      | SP                              |
		| pedido | EndEtg_cep                                     | 02045080                        |
		| pedido | st_etg_imediata                                | 2                               |
		| pedido | etg_imediata_data                              | 2021-01-20 18:31:31             |
		| pedido | etg_imediata_usuario                           | HAMILTON                        |
		| pedido | frete_status                                   | 0                               |
		| pedido | frete_valor                                    | 0.0000                          |
		| pedido | frete_data                                     | null                            |
		| pedido | frete_usuario                                  | null                            |
		| pedido | StBemUsoConsumo                                | 1                               |
		| pedido | PedidoRecebidoStatus                           | 0                               |
		| pedido | PedidoRecebidoData                             | null                            |
		| pedido | PedidoRecebidoUsuarioUltAtualiz                | null                            |
		| pedido | PedidoRecebidoDtHrUltAtualiz                   | null                            |
		| pedido | InstaladorInstalaStatus                        | 2                               |
		| pedido | InstaladorInstalaUsuarioUltAtualiz             | HAMILTON                        |
		| pedido | InstaladorInstalaDtHrUltAtualiz                | 2021-01-20 18:31:31             |
		| pedido | custoFinancFornecTipoParcelamento              | null                            |
		| pedido | custoFinancFornecQtdeParcelas                  | 0                               |
		| pedido | BoletoConfeccionadoStatus                      | 0                               |
		| pedido | BoletoConfeccionadoData                        |                                 |
		| pedido | GarantiaIndicadorStatus                        | 0                               |
		| pedido | GarantiaIndicadorUsuarioUltAtualiz             | HAMILTON                        |
		| pedido | GarantiaIndicadorDtHrUltAtualiz                | 2021-01-20 18:31:31             |
		| pedido | EndEtg_endereco_numero                         | 97                              |
		| pedido | EndEtg_endereco_complemento                    |                                 |
		| pedido | romaneio_status                                | 0                               |
		| pedido | romaneio_data                                  | null                            |
		| pedido | romaneio_data_hora                             | null                            |
		| pedido | romaneio_usuario                               | null                            |
		| pedido | danfe_impressa_status                          | 0                               |
		| pedido | danfe_impressa_data                            | null                            |
		| pedido | danfe_impressa_data_hora                       | null                            |
		| pedido | danfe_impressa_usuario                         | null                            |
		| pedido | transportadora_conferente                      | null                            |
		| pedido | transportadora_motorista                       | null                            |
		| pedido | transportadora_placa_veiculo                   | null                            |
		| pedido | perc_desagio_RA_liquida                        | 30.0                            |
		| pedido | indicador_editado_manual_status                | 0                               |
		| pedido | indicador_editado_manual_data_hora             | null                            |
		| pedido | indicador_editado_manual_usuario               | null                            |
		| pedido | indicador_editado_manual_indicador_original    | null                            |
		| pedido | permite_RA_status                              | 0                               |
		| pedido | st_violado_permite_RA_status                   | 0                               |
		| pedido | dt_hr_violado_permite_RA_status                | null                            |
		| pedido | usuario_violado_permite_RA_status              | null                            |
		| pedido | opcao_possui_RA                                | -                               |
		| pedido | tamanho_num_pedido                             | 9                               |
		| pedido | pedido_base                                    | 222267N                         |
		| pedido | numero_loja                                    | 202                             |
		| pedido | data_hora                                      | 2021-01-20 18:31:28             |
		| pedido | st_forma_pagto_somente_cartao                  | 0                               |
		| pedido | endereco_memorizado_status                     | 1                               |
		| pedido | endereco_logradouro                            | Rua Francisco Pecoraro          |
		| pedido | endereco_bairro                                | Água Fria                       |
		| pedido | endereco_cidade                                | São Paulo                       |
		| pedido | endereco_uf                                    | SP                              |
		| pedido | endereco_cep                                   | 02408150                        |
		| pedido | endereco_numero                                | 97                              |
		| pedido | endereco_complemento                           | casa 01                         |
		| pedido | analise_endereco_tratar_status                 | 0                               |
		| pedido | analise_endereco_tratado_status                | 0                               |
		| pedido | analise_endereco_tratado_data                  | null                            |
		| pedido | analise_endereco_tratado_data_hora             | null                            |
		| pedido | analise_endereco_tratado_usuario               | null                            |
		| pedido | analise_credito_data_sem_hora                  | null                            |
		| pedido | cancelado_auto_status                          | 0                               |
		| pedido | cancelado_auto_data                            | null                            |
		| pedido | cancelado_auto_data_hora                       | null                            |
		| pedido | cancelado_auto_motivo                          | null                            |
		| pedido | obs_3                                          | null                            |
		| pedido | danfe_a_imprimir_status                        | 0                               |
		| pedido | danfe_a_imprimir_data_hora                     | null                            |
		| pedido | danfe_a_imprimir_usuario                       | null                            |
		| pedido | transportadora_selecao_auto_status             | 0                               |
		| pedido | transportadora_selecao_auto_cep                | null                            |
		| pedido | transportadora_selecao_auto_tipo_endereco      | 0                               |
		| pedido | transportadora_selecao_auto_transportadora     | null                            |
		| pedido | transportadora_selecao_auto_data_hora          | null                            |
		| pedido | pedido_bs_x_at                                 | null                            |
		| pedido | cancelado_data_hora                            | null                            |
		| pedido | cancelado_codigo_motivo                        | null                            |
		| pedido | cancelado_codigo_sub_motivo                    | null                            |
		| pedido | cancelado_motivo                               | null                            |
		| pedido | EndEtg_obs                                     | null                            |
		| pedido | pedido_bs_x_ac                                 |                                 |
		| pedido | pedido_bs_x_ac_reverso                         |                                 |
		| pedido | EndEtg_cod_justificativa                       | 003                             |
		| pedido | pedido_bs_x_marketplace                        |                                 |
		| pedido | marketplace_codigo_origem                      |                                 |
		| pedido | id_nfe_emitente                                | 4903                            |
		| pedido | st_pedido_novo_analise_credito_msg_alerta      | 0                               |
		| pedido | dt_hr_pedido_novo_analise_credito_msg_alerta   | null                            |
		| pedido | st_forma_pagto_possui_parcela_cartao           | 0                               |
		| pedido | vl_previsto_cartao                             | 0.0000                          |
		| pedido | NFe_texto_constar                              | null                            |
		| pedido | NFe_xPed                                       | null                            |
		| pedido | MarketplacePedidoRecebidoRegistrarStatus       | 0                               |
		| pedido | MarketplacePedidoRecebidoRegistrarDataRecebido | null                            |
		| pedido | MarketplacePedidoRecebidoRegistrarDataHora     | null                            |
		| pedido | MarketplacePedidoRecebidoRegistrarUsuario      | null                            |
		| pedido | MarketplacePedidoRecebidoRegistradoStatus      | 0                               |
		| pedido | MarketplacePedidoRecebidoRegistradoDataHora    | null                            |
		| pedido | MarketplacePedidoRecebidoRegistradoUsuario     | null                            |
		| pedido | st_auto_split                                  | 0                               |
		| pedido | analise_credito_pendente_vendas_motivo         | null                            |
		| pedido | usuario_cadastro                               | HAMILTON                        |
		| pedido | plataforma_origem_pedido                       | 0                               |
		| pedido | magento_installer_commission_value             | 0.0000                          |
		| pedido | magento_installer_commission_discount          | 0.0000                          |
		| pedido | magento_shipping_amount                        | 0.0000                          |
		| pedido | dt_st_pagto                                    | null                            |
		| pedido | dt_hr_st_pagto                                 | null                            |
		| pedido | usuario_st_pagto                               |                                 |
		| pedido | pc_maquineta_qtde_parcelas                     | 0                               |
		| pedido | pc_maquineta_valor_parcela                     | 0.0000                          |
		| pedido | sistema_responsavel_cadastro                   | 1                               |
		| pedido | sistema_responsavel_atualizacao                | 1                               |
		| pedido | num_obs_2                                      | 0                               |
		| pedido | num_obs_3                                      | 0                               |
		| pedido | PrevisaoEntregaData                            | null                            |
		| pedido | PrevisaoEntregaUsuarioUltAtualiz               | null                            |
		| pedido | PrevisaoEntregaDtHrUltAtualiz                  | null                            |
		| pedido | st_forma_pagto_possui_parcela_cartao_maquineta | 0                               |
		| pedido | st_memorizacao_completa_enderecos              | 1                               |
		| pedido | endereco_email                                 | gabriel.prada.teodoro@gmail.com |
		| pedido | endereco_email_xml                             | teste@xml.com                   |
		| pedido | endereco_nome                                  | Gabriel Prada Teodoro           |
		| pedido | endereco_ddd_res                               | 11                              |
		| pedido | endereco_tel_res                               | 25321634                        |
		| pedido | endereco_ddd_com                               | 11                              |
		| pedido | endereco_tel_com                               | 55788755                        |
		| pedido | endereco_ramal_com                             | 12                              |
		| pedido | endereco_ddd_cel                               | 11                              |
		| pedido | endereco_tel_cel                               | 981603313                       |
		| pedido | endereco_ddd_com_2                             |                                 |
		| pedido | endereco_tel_com_2                             |                                 |
		| pedido | endereco_ramal_com_2                           |                                 |
		| pedido | endereco_tipo_pessoa                           | PF                              |
		| pedido | endereco_cnpj_cpf                              | 35270445824                     |
		| pedido | endereco_contribuinte_icms_status              | 2                               |
		| pedido | endereco_produtor_rural_status                 | 2                               |
		| pedido | endereco_ie                                    | 361.289.183.714                 |
		| pedido | endereco_rg                                    | 304480484                       |
		| pedido | endereco_contato                               |                                 |
		| pedido | EndEtg_email                                   | gabriel.prada.teodoro@gmail.com |
		| pedido | EndEtg_email_xml                               | teste@xml.com                   |
		| pedido | EndEtg_nome                                    | Gabriel Prada Teodoro           |
		| pedido | EndEtg_ddd_res                                 |                                 |
		| pedido | EndEtg_tel_res                                 |                                 |
		| pedido | EndEtg_ddd_com                                 |                                 |
		| pedido | EndEtg_tel_com                                 |                                 |
		| pedido | EndEtg_ramal_com                               |                                 |
		| pedido | EndEtg_ddd_cel                                 |                                 |
		| pedido | EndEtg_tel_cel                                 |                                 |
		| pedido | EndEtg_ddd_com_2                               |                                 |
		| pedido | EndEtg_tel_com_2                               |                                 |
		| pedido | EndEtg_ramal_com_2                             |                                 |
		| pedido | EndEtg_tipo_pessoa                             | PF                              |
		| pedido | EndEtg_cnpj_cpf                                | 35270445824                     |
		| pedido | EndEtg_contribuinte_icms_status                | 2                               |
		| pedido | EndEtg_produtor_rural_status                   | 2                               |
		| pedido | EndEtg_ie                                      | 361.289.183.714                 |
		| pedido | EndEtg_rg                                      | 304480484                       |
		| pedido | endereco_nome_iniciais_em_maiusculas           | Gabriel Prada Teodoro           |
		| pedido | EndEtg_nome_iniciais_em_maiusculas             | Gabriel Prada Teodoro           |

Scenario: perc_desagio_RA_liquida
	#gravado no pai e nos filhotes, depende da loja (NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca é gravado)
	#
	#loja/PedidoNovoConfirma.asp
	#			'01/02/2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, portanto, não devem ter deságio do RA
	#			if (Cstr(loja) <> Cstr(NUMERO_LOJA_ECOMMERCE_AR_CLUBE)) And (Not blnMagentoPedidoComIndicador) then rs("perc_desagio_RA_liquida") = getParametroPercDesagioRALiquida
	#set rP = get_registro_t_parametro(ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA)
	#if Trim("" & rP.campo_real) <> "" then getParametroPercDesagioRALiquida = rP.campo_real
	#s = "SELECT " & _
	#		"*" & _
	#	" FROM t_PARAMETRO" & _
	#	" WHERE" & _
	#		" (id = '" & id_registro & "')"
	#
	When Fazer esta validação