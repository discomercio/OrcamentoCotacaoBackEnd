@ignore
Feature: Validações do perc_RT

Background: Reiniciar banco
	Given Reiniciar permissões do usuário quando terminar o teste
	Given Reiniciar t_loja quando terminar o teste


Scenario: Verificar se pode ser editado - precisa da permissão OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO
Given Usuário sem permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
When Pedido Base
And Informo "perc_RT" = 1
Then Erro "Usuário não pode editar perc_RT"

Scenario: Verificar se pode ser editado 2
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
When Pedido Base
And Informo "perc_RT" = 1
Then Sem erro "Usuário não pode editar perc_RT"

Scenario: Verificar se pode ser editado 3 - NUMERO_LOJA_ECOMMERCE_AR_CLUBE nunca pode
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "NUMERO_LOJA_ECOMMERCE_AR_CLUBE"
When Pedido Base
And Informo "perc_RT" = 1
Then Erro "Usuário não pode editar perc_RT"

Scenario: Limites do perc_RT 1
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
When Pedido Base
And Informo "perc_RT" = -1
Then Erro "perc_RT inválido"

Scenario: Limites do perc_RT 2
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
When Pedido Base
And Informo "perc_RT" = 101
Then Erro "perc_RT inválido"

Scenario: Limites do perc_RT 3
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
When Pedido Base
And Informo "perc_RT" = 10
Then Sem erro "perc_RT inválido"

Scenario: Limite por loja 1
#loja/PedidoNovoConsiste.asp
#			elseif converte_numero(c_perc_RT) > rCD.perc_max_comissao then
#				alerta = "O percentual de comissão excede o máximo permitido."
#				end if
#rCD: SELECT perc_max_comissao FROM t_LOJA
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
And Tabela t_loja registro com loja = 202
And Tabela t_loja registro atual perc_max_comissao = 1
When Pedido Base
And Informo "perc_RT" = 10
Then Erro "O percentual de comissão excede o máximo permitido."

Scenario: Limite por loja 2
Given Usuário com permissão "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO"
And Loja do usuário = "202"
And Tabela t_loja registro com loja = 202
And Tabela t_loja registro atual perc_max_comissao = 11
When Pedido Base
And Informo "perc_RT" = 10
Then Sem erro "O percentual de comissão excede o máximo permitido."

