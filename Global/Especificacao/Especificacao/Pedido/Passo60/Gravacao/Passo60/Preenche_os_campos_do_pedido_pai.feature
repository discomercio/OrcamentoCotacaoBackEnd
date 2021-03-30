@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Preenche_os_campos_do_pedido_pai

#Campos que existem em todos os pedidos: pedido, loja, data, hora
#Campos que existem somente no pedido base, não nos filhotes:
#	st_auto_split se tiver filhotes
#	Campos transferidos: de linha 1800 rs("dt_st_pagto") = Date até 1887 rs("perc_limite_RA_sem_desagio") = perc_limite_RA_sem_desagio
#Campos que existem somente nos pedidos filhotes, não no base:
#	linha 1892 rs("st_auto_split") = 1 até 1903 rs("forma_pagto")=""
#Transfere mais campos: linha 1907 até 2055
#
Background: Setup
	#ignoramos no prepedio inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário

#a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
#loja/PedidoNovoConfirma.asp
#de linha 1788
#s = "SELECT * FROM t_PEDIDO WHERE pedido='X'"
#até linha 2057
#rs("id_nfe_emitente") = vEmpresaAutoSplit(iv)
#rs.Update
Scenario: Preenche_os_campos_do_pedido_pai - endereco
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_nome_iniciais_em_maiusculas" = "Vivian"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_memorizacao_completa_enderecos" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_memorizado_status" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_logradouro" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_cidade" = "São Paulo"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_uf" = "SP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_cep" = "02045080"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_numero" = "97"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_complemento" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_email" = "testeCad@Gabriel.com"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_email_xml" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_nome" = "Vivian"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ddd_res" = "11"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_tel_res" = "11111111"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ddd_com" = "11"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_tel_com" = "12345678"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ramal_com" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ddd_cel" = "11"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_tel_cel" = "981603313"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ddd_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_tel_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ramal_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_tipo_pessoa" = "PF"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_cnpj_cpf" = "14039603052"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_contribuinte_icms_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_produtor_rural_status" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_ie" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_rg" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_contato" = ""

Scenario: Preenche_os_campos_do_pedido_pai - pagamento
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_forma_pagto_possui_parcela_cartao_maquineta" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_forma_pagto_somente_cartao" = "1"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_forma_pagto_possui_parcela_cartao" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "BoletoConfeccionadoStatus" = "0"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_pagto" = "N"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "forma_pagto" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "av_forma_pagto" = "6"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pc_valor_parcela" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pc_maquineta_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pc_maquineta_valor_parcela" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_forma_pagto_entrada" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_forma_pagto_prestacao" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_entrada_valor" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_prestacao_qtde" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_prestacao_valor" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pce_prestacao_periodo" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_forma_pagto_prim_prest" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_forma_pagto_demais_prest" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_prim_prest_valor" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_prim_prest_apos" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_demais_prest_qtde" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_demais_prest_valor" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pse_demais_prest_periodo" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pu_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pu_valor" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pu_vencto_apos" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "usuario_st_pagto" = "USRMAG"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "custoFinancFornecTipoParcelamento" = "AV"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "custoFinancFornecQtdeParcelas" = "0"

Scenario: Preenche_os_campos_do_pedido - endereco entrega
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_obs" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_nome_iniciais_em_maiusculas" = "Vivian"
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base com endereço de entrega
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_end_entrega" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_cod_justificativa" = "003"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_cidade" = "São Paulo"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_uf" = "SP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_cep" = "02045080"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_endereco_numero" = "97"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_endereco_complemento" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_email" = "testeCad@Gabriel.com"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_email_xml" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_nome" = "Vivian"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ddd_res" = "11"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_res" = "11111111"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ddd_com" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_com" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ramal_com" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ddd_cel" = "11"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_cel" = "981603313"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ddd_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ramal_com_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tipo_pessoa" = "PF"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_cnpj_cpf" = "29756194804"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_contribuinte_icms_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_produtor_rural_status" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_ie" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_rg" = ""

Scenario: Preenche_os_campos_do_pedido - campos soltos
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_servicos" = "0.0000"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "timestamp" = "[xL"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_orc_virou_pedido" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "tamanho_num_pedido" = "7"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido_base" = "pedido gerado"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "numero_loja" = "202"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "201"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "USRMAG"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "midia" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "servicos" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "orcamento" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "orcamentista" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "id_nfe_emitente" = "4903"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "NFe_texto_constar" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "NFe_xPed" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "usuario_cadastro" = "USRMAG"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "plataforma_origem_pedido" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "sistema_responsavel_cadastro" = "5"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "sistema_responsavel_atualizacao" = "5"

Scenario: Preenche_os_campos_do_pedido - campos soltos com valores
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_pago_familia" = "0.0000"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_previsto_cartao" = "156645.00"
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_familia" = "156645.00"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_NF" = "156645.00"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_RA" = "0.00"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_RA_liquido" = "0.00"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_frete" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "comissao_loja_indicou" = "0.0"

Scenario: Preenche_os_campos_do_pedido - transportadora
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_id" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_usuario" = "null"
	#Campo que não existem na t_PEDIDO
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_num_coleta" = "null"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_contato" = "null"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_conferente" = "null"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_motorista" = "null"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_placa_veiculo" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_selecao_auto_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_selecao_auto_cep" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_selecao_auto_tipo_endereco" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_selecao_auto_transportadora" = "null"

Scenario: Preenche_os_campos_do_pedido - refente a entrega
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "entregue_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "a_entregar_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "a_entregar_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "PedidoRecebidoUsuarioUltAtualiz" = ""
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_recebido" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_entrega" = "SEP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "a_entregar_data_marcada" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PedidoRecebidoStatus" = "0"

Scenario: Preenche_os_campos_do_pedido - analise de crédito
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratado_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratado_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_pedido_novo_analise_credito_msg_alerta" = "0"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "8"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_pendente_vendas_motivo" = "006"

Scenario: Preenche_os_campos_do_pedido - split
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	Given Usar produto "dois" como fabricante = "003", produto = "003221"
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Given Zerar todo o estoque
	Given Definir saldo de estoque = "40" para produto "um"
	Given Definir saldo estoque = "40" para produto = "um" e id_nfe_emitente = "4003"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD" alterar registro id_wms_regra_cd_x_uf_x_pessoa = "666" e id_nfe_emitente = "4003", campo "st_inativo" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido" = "176368N"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "split_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "split_usuario" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_auto_split" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido" = "176368N-A"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_status" = "1"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "split_usuario" = "SISTEMA"
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "st_auto_split" = "1"

Scenario: Preenche_os_campos_do_pedido - refente a cancelamento
	Given Pedido base
	Then Sem nenhum erro
#Obs: esses campo ainda não existem mas, serão utilizados para visualização
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_usuario" = ""
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_auto_status" = "0"
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_auto_motivo" = ""
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_codigo_motivo" = ""
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_codigo_sub_motivo" = ""
#And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_motivo" = ""

Scenario: Preenche_os_campos_do_pedido - instalador, bem de uso, entrega imediata e garantia
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_etg_imediata" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "etg_imediata_usuario" = "USRMAG"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PrevisaoEntregaUsuarioUltAtualiz" = "null"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "StBemUsoConsumo" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "InstaladorInstalaStatus" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "InstaladorInstalaUsuarioUltAtualiz" = "USRMAG"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "GarantiaIndicadorStatus" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "GarantiaIndicadorUsuarioUltAtualiz" = "USRMAG"

Scenario: Preenche_os_campos_do_pedido - campos de OBS
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "num_obs_2" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "num_obs_3" = "0"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "obs_1" = "teste magento"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "obs_2" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "obs_3" = "null"

Scenario: Preenche_os_campos_do_pedido - referente a indicação
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "comissao_paga" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "comissao_paga_ult_op" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "comissao_paga_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador_editado_manual_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador_editado_manual_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador_editado_manual_indicador_original" = ""
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja_indicou" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "venda_externa" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_RT" = "0.0"

@ignore
Scenario: Preenche_os_campos_do_pedido - campos de data e hora
	Given Pedido base
	Then Sem nenhum erro
	#podemos usar os seguintes valores: "especial: data atual, com hora", "especial: data atual, sem hora" e "especial: hora atual, formato HoraParaBanco"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "entregue_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "a_entregar_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "a_entregar_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PedidoRecebidoData" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PedidoRecebidoDtHrUltAtualiz" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratado_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratado_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_data_sem_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "split_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "split_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PrevisaoEntregaDtHrUltAtualiz" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "etg_imediata_data" = "2021-01-20 18:31:31"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "transportadora_selecao_auto_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "comissao_paga_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "frete_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "InstaladorInstalaDtHrUltAtualiz" = "2021-01-20 18:31:31"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "GarantiaIndicadorDtHrUltAtualiz" = "2021-01-20 18:31:31"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "romaneio_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "romaneio_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_impressa_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_impressa_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador_editado_manual_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "BoletoConfeccionadoData" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "data_hora" = "2021-01-20 18:31:28"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_auto_data" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_auto_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "dt_hr_violado_permite_RA_status" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_a_imprimir_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cancelado_data_hora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "dt_hr_pedido_novo_analise_credito_msg_alerta" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistrarDataRecebido" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistrarDataHora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistradoDataHora" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "dt_st_pagto" = "2021-01-20 00:00:00"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "dt_hr_st_pagto" = "2021-01-20 18:31:30"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "PrevisaoEntregaData" = ""

Scenario: Preenche_os_campos_do_pedido - frete, romaneio e danfe
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "frete_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "frete_valor" = "0.0000"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "frete_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "romaneio_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "romaneio_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_impressa_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_impressa_usuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_a_imprimir_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "danfe_a_imprimir_usuario" = ""
	#Obs: No momento esses campos não existem na tabela t_PEDIDO e alguns são utilizados apenas na Central
	Given Pedido base
	Then Sem nenhum erro

Scenario: Preenche_os_campos_do_pedido - referente a RA - magento
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_violado_permite_RA_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "usuario_violado_permite_RA_status" = ""
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_limite_RA_sem_desagio" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_tem_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "qtde_parcelas_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA_liquida" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "permite_RA_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_RA" = "-"

Scenario: Preenche_os_campos_do_pedido - referente a RA - loja
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_violado_permite_RA_status" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "usuario_violado_permite_RA_status" = ""
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_limite_RA_sem_desagio" = "0.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "st_tem_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "qtde_parcelas_desagio_RA" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "perc_desagio_RA_liquida" = "25.0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "permite_RA_status" = "0"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_RA" = "-"

Scenario: Preenche_os_campos_do_pedido - Marketplace e magento
	#nao temos como validar este campo porque ele é gerado sempre com um númeor único
	#mas deveriamos verificar que gravou algo!
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido_bs_x_ac" = "123456789"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido_bs_x_ac_reverso" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistrarStatus" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistrarUsuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistradoStatus" = "0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "MarketplacePedidoRecebidoRegistradoUsuario" = ""
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "magento_installer_commission_value" = "0.0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "magento_installer_commission_discount" = "0.0"
	#And Tabela "t_PEDIDO" registro pai criado, verificar campo "magento_shipping_amount" = "0.0"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido_bs_x_at" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido_bs_x_marketplace" = ""
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "marketplace_codigo_origem" = "001"

@ignore
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
@ignore
Scenario: perc_desagio_RA_liquida 2
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