@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Preenche_os_campos_do_pedido_filhote

Background: Setup
	#ignoramos no prepedio inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	Given Usar produto "dois" como fabricante = "003", produto = "003221"
	When Lista de itens "0" informo "Qtde" = "100"
	And Lista de itens "0" informo "Subtotal" = "62658.00"
	And Lista de itens "0" informo "RowTotal" = "62658.00"
	When Lista de itens "1" informo "Qtde" = "100"
	And Lista de itens "1" informo "Subtotal" = "93687.00"
	And Lista de itens "1" informo "RowTotal" = "93687.00"
	And Informo "InfCriacaoPedido.Pedido_marketplace" = "null"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Given Zerar todo o estoque
	Given Definir saldo de estoque = "40" para produto "um"
	Given Definir saldo estoque = "40" para produto = "um" e id_nfe_emitente = "4003"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD" alterar registro id_wms_regra_cd_x_uf_x_pessoa = "666" e id_nfe_emitente = "4003", campo "st_inativo" = "0"
	Given Reiniciar banco ao terminar cenário

Scenario: Preenche_os_campos_do_pedido_filhote - endereco
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_nome_iniciais_em_maiusculas" = "Vivian"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_memorizacao_completa_enderecos" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_memorizado_status" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_logradouro" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_cidade" = "São Paulo"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_uf" = "SP"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_cep" = "02045080"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_numero" = "97"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_complemento" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_email" = "testeCad@Gabriel.com"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_email_xml" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_nome" = "Vivian"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ddd_res" = "11"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_tel_res" = "11111111"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ddd_com" = "11"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_tel_com" = "12345678"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ramal_com" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ddd_cel" = "11"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_tel_cel" = "981603313"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ddd_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_tel_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ramal_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_tipo_pessoa" = "PF"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_cnpj_cpf" = "14039603052"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_contribuinte_icms_status" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_produtor_rural_status" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_ie" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_rg" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "endereco_contato" = ""

Scenario: Preenche_os_campos_do_pedido_filhote - pagamento
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_forma_pagto_possui_parcela_cartao_maquineta" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_forma_pagto_somente_cartao" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_forma_pagto_possui_parcela_cartao" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "BoletoConfeccionadoStatus" = "0"
	Given Ignorar cenário no ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_pagto" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "forma_pagto" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "tipo_parcelamento" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "av_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pc_valor_parcela" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_forma_pagto_entrada" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_forma_pagto_prestacao" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_entrada_valor" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_prestacao_qtde" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_prestacao_valor" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pce_prestacao_periodo" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_forma_pagto_prim_prest" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_forma_pagto_demais_prest" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_prim_prest_valor" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_prim_prest_apos" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_demais_prest_qtde" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_demais_prest_valor" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pse_demais_prest_periodo" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pu_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pu_valor" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pu_vencto_apos" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "usuario_st_pagto" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pc_maquineta_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pc_maquineta_valor_parcela" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "custoFinancFornecTipoParcelamento" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "custoFinancFornecQtdeParcelas" = "0"

Scenario: Preenche_os_campos_do_pedido_filhote - endereco entrega
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_end_entrega" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_cod_justificativa" = "003"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_cidade" = "São Paulo"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_uf" = "SP"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_cep" = "02045080"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_endereco_numero" = "97"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_endereco_complemento" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_obs" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_email" = "gabriel.prada.teodoro@gmail.com"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_email_xml" = "teste@xml.com"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_nome" = "Gabriel Prada Teodoro"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ddd_res" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_tel_res" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ddd_com" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_tel_com" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ramal_com" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ddd_cel" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_tel_cel" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ddd_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_tel_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ramal_com_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_tipo_pessoa" = "PF"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_cnpj_cpf" = "35270445824"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_contribuinte_icms_status" = "2"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_produtor_rural_status" = "2"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_ie" = "361.289.183.714"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_rg" = "304480484"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "EndEtg_nome_iniciais_em_maiusculas" = "Gabriel Prada Teodoro"

Scenario: Preenche_os_campos_do_pedido_filhote - campos soltos
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "timestamp" = "[xM"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido_base" = "222267N"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_servicos" = "0.0000"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_orc_virou_pedido" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "tamanho_num_pedido" = "9"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "numero_loja" = "202"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "loja" = "201"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vendedor" = "USRMAG"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "midia" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "servicos" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "orcamento" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "orcamentista" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "id_nfe_emitente" = "4003"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "NFe_texto_constar" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "NFe_xPed" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "usuario_cadastro" = "USRMAG"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "plataforma_origem_pedido" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "sistema_responsavel_cadastro" = "5"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "sistema_responsavel_atualizacao" = "5"

Scenario: Preenche_os_campos_do_pedido_filhote - campos soltos com valores
	##And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_pago_familia" = "0.0000"
	##And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_previsto_cartao" = "0.0000"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_total_familia" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_total_NF" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_total_RA" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_total_RA_liquido" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "vl_frete" = "0.0000"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "comissao_loja_indicou" = "0.0"

Scenario: Preenche_os_campos_do_pedido_filhote - transportadora
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_num_coleta" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_contato" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_conferente" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_motorista" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_placa_veiculo" = "null"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_id" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_usuario" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_selecao_auto_status" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_selecao_auto_cep" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_selecao_auto_tipo_endereco" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_selecao_auto_transportadora" = "null"

Scenario: Preenche_os_campos_do_pedido_filhote - refente a entrega
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "entregue_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "a_entregar_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "a_entregar_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PedidoRecebidoUsuarioUltAtualiz" = "null"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_recebido" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_entrega" = "SEP"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "a_entregar_data_marcada" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PedidoRecebidoStatus" = "0"

Scenario: Preenche_os_campos_do_pedido_filhote - analise de crédito
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_endereco_tratar_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_endereco_tratado_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_endereco_tratado_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_pedido_novo_analise_credito_msg_alerta" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_credito" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_credito_usuario" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_credito_pendente_vendas_motivo" = "null"

Scenario: Preenche_os_campos_do_pedido_filhote - split
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_status" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_usuario" = "SISTEMA"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_auto_split" = "1"

Scenario: Preenche_os_campos_do_pedido_filhote - refente a cancelamento
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_auto_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_auto_motivo" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_codigo_motivo" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_codigo_sub_motivo" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_motivo" = "null"
	Then Sem nenhum erro

Scenario: Preenche_os_campos_do_pedido_filhote - instalador, bem de uso, entrega imediata e garantia
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_etg_imediata" = "2"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "etg_imediata_usuario" = "USRMAG"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PrevisaoEntregaUsuarioUltAtualiz" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "StBemUsoConsumo" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "InstaladorInstalaStatus" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "InstaladorInstalaUsuarioUltAtualiz" = "USRMAG"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "GarantiaIndicadorStatus" = "0"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "GarantiaIndicadorUsuarioUltAtualiz" = "USRMAG"

Scenario: Preenche_os_campos_do_pedido_filhote - campos de OBS
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "num_obs_2" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "num_obs_3" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "obs_1" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "obs_2" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "obs_3" = "null"

Scenario: Preenche_os_campos_do_pedido_filhote - referente a indicação
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "comissao_paga" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "comissao_paga_ult_op" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "comissao_paga_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "indicador_editado_manual_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "indicador_editado_manual_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "indicador_editado_manual_indicador_original" = "null"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "loja_indicou" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "venda_externa" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "indicador" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "perc_RT" = "0.0"

Scenario: Preenche_os_campos_do_pedido_filhote - campos de data e hora
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "entregue_data" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_data" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_data" = "especial: data atual, sem hora"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_hora" = "especial: hora atual, formato HoraParaBanco"
	#campos comentados: ainda não temos na nossa t_PEDIDO
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "a_entregar_data" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "a_entregar_hora" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_data" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_credito_data" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "comissao_paga_data" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "etg_imediata_data" = "especial: data atual, com hora"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "frete_data" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PedidoRecebidoData" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PedidoRecebidoDtHrUltAtualiz" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "InstaladorInstalaDtHrUltAtualiz" = "especial: data atual, com hora"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "BoletoConfeccionadoData" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "GarantiaIndicadorDtHrUltAtualiz" = "especial: data atual, com hora"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "romaneio_data" = "null"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "romaneio_data_hora" = "null"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_impressa_data" = "null"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_impressa_data_hora" = "null"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "indicador_editado_manual_data_hora" = "null"
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "dt_hr_violado_permite_RA_status" = "null"
#campo data_hora é [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "data_hora" = "2021-01-20 18:31:28"

Scenario: Preenche_os_campos_do_pedido_filhote - campos de data e hora 2
	Then Sem nenhum erro
	#campos comentados: ainda não temos na nossa t_PEDIDO
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_endereco_tratado_data" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_endereco_tratado_data_hora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "analise_credito_data_sem_hora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_auto_data" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_auto_data_hora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_a_imprimir_data_hora" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "transportadora_selecao_auto_data_hora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "cancelado_data_hora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "dt_hr_pedido_novo_analise_credito_msg_alerta" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistrarDataRecebido" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistrarDataHora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistradoDataHora" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistradoUsuario" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "dt_st_pagto" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "dt_hr_st_pagto" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PrevisaoEntregaData" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "PrevisaoEntregaDtHrUltAtualiz" = "null"

Scenario: Preenche_os_campos_do_pedido_filhote - frete, romaneio e danfe
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "frete_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "frete_valor" = "0.0000"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "frete_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "romaneio_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "romaneio_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_impressa_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_impressa_usuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_a_imprimir_status" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "danfe_a_imprimir_usuario" = "null"
	#Obs: No momento esses campos não existem na tabela t_PEDIDO e alguns são utilizados apenas na Central
	Then Sem nenhum erro

Scenario: Preenche_os_campos_do_pedido_filhote - referente a RA
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_violado_permite_RA_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "usuario_violado_permite_RA_status" = ""
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_limite_RA_sem_desagio" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_tem_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "qtde_parcelas_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA_liquida" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "permite_RA_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_RA" = "-"

Scenario: Preenche_os_campos_do_pedido_filhote - Marketplace e magento
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido_bs_x_ac_reverso" = ""
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistrarStatus" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistrarUsuario" = "null"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "MarketplacePedidoRecebidoRegistradoStatus" = "0"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "magento_installer_commission_value" = "0.0000"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "magento_installer_commission_discount" = "0.0000"
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "magento_shipping_amount" = "0.0000"
	Then Sem nenhum erro
	#nao temos como validar este campo porque ele é gerado sempre com um númeor único
	#mas deveriamos verificar que gravou algo!
	#And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido_bs_x_ac" = "123457006"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido_bs_x_at" = ""
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido_bs_x_marketplace" = "null"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "marketplace_codigo_origem" = "001"