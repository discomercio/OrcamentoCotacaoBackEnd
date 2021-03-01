@Especificacao.Pedido.PedidoFaltandoImplementarSteps
@GerenciamentoBanco
#@ignore
#@Especificacao.Pedido.Passo30
Feature: Validações do perc_RT

Background: Reiniciar banco
	Given Reiniciar banco ao terminar cenário
	#ignoramos no prepedido inteiro, não tem perc_rt
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#magento não tem o campo perc_rt
	And Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"



Scenario: Verificar se pode ser editado - precisa da permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO
	#Given Usuário sem permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	Given Tabela "t_OPERACAO" apagar registro com campo "id" = "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	#And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	When Pedido base
	And Informo "perc_RT" = "1"
	Then Erro "Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)"

Scenario: Verificar se pode ser editado 2
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	Given Pedido base
	When Informo "perc_RT" = "1"
	Then Sem erro "Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)"

Scenario: Verificar se pode ser editado 3 - NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca pode
	Esse teste não tem como fazer, pois o perc_RT é calculado automaticamente
	#Given Implementado em Especificacao.Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos.PedidoMagentoDtoFeature.validação de Perc_RT
	#Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	#And Loja do usuário = "NUMERO_LOJA_ECOMMERCE_AR_CLUBE"
	#When Pedido Base
	#And Informo "perc_RT" = "1"
	#Then Erro "Usuário não pode editar perc_RT (permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO)"

Scenario: Limites do perc_RT 1
	#loja/PedidoNovoConfirma.asp
	#		if (perc_RT < 0) Or (perc_RT > 100) then
	#			alerta = "Percentual de comissão inválido."
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	Given Pedido base
	When Informo "perc_RT" = "-1"
	Then Erro "perc_RT inválido"

Scenario: Limites do perc_RT 2
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	Given Pedido base
	When Informo "perc_RT" = "101"
	Then Erro "perc_RT inválido"

Scenario: Limites do perc_RT 3
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	Given Pedido base
	When Informo "perc_RT" = "10"
	Then Sem nenhum erro

Scenario: Limite por loja 1
	#loja/PedidoNovoConsiste.asp
	#			elseif converte_numero(c_perc_RT) > rCD.perc_max_comissao then
	#				alerta = "O percentual de comissão excede o máximo permitido."
	#				end if
	#rCD: SELECT perc_max_comissao FROM t_LOJA
	#set rCD = obtem_perc_max_comissao_e_desconto_por_loja(loja)
	#loja/PedidoNovoConfirma.asp
	#if perc_RT > rCD.perc_max_comissao then
	#	alerta = "Percentual de comissão excede o máximo permitido."
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao" = "1"
	Given Pedido base
	When Informo "perc_RT" = "10"
	Then Erro "O percentual de comissão excede o máximo permitido."

Scenario: Limite por loja 2
	Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
	And Loja do usuário = "NUMERO_LOJA_BONSHOP"
	And Tabela "t_LOJA" com loja = "202" alterar campo "perc_max_comissao" = "11"
	Given Pedido base
	When Informo "perc_RT" = "10"
	Then Sem erro "O percentual de comissão excede o máximo permitido."

