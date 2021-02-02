@ignore
Feature: ESTOQUE_produto_saida_v2_rotina
#vamos testar a rotina ESTOQUE_produto_saida_v2

Background: Configuracao
	Given Usar produto fabricante = "003", produto = "003000"
	And usar id_nfe_emitente  = "4003"
	And Zerar todo o estoque
#
#' --------------------------------------------------------------------
#'   ESTOQUE_PRODUTO_SAIDA_V2
#'   Retorno da função:
#'      False - Ocorreu falha ao tentar movimentar o estoque.
#'      True - Conseguiu fazer a movimentação do estoque.
#'   IMPORTANTE: sempre chame esta rotina dentro de uma transação para
#'      garantir a consistência dos registros entre as várias tabelas.
#'   Esta função processa a saída dos produtos do "estoque de venda"
#'   para o "estoque vendido".  No caso de não haver produtos sufi-
#'   cientes no "estoque de venda" e desde que esteja autorizado
#'   através do parâmetro "qtde_autorizada_sem_presenca", os produtos
#'   que faltam são colocados automaticamente na lista de produtos
#'   vendidos sem presença no estoque.
#function estoque_produto_saida_v2(byval id_usuario, byval id_pedido, _
#								byval id_nfe_emitente, byval id_fabricante, byval id_produto, _
#								byval qtde_a_sair, byval qtde_autorizada_sem_presenca, _
#								byref qtde_estoque_vendido, byref qtde_estoque_sem_presenca, _
#								byref msg_erro)

Scenario: ESTOQUE_produto_saida_v2_rotina normal
	Given Definir saldo de estoque = "40"
	When Chamar saida com qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	Then Retorno qtde_estoque_vendido = "10", qtde_estoque_sem_presenca = "0"
	And msg_erro vazia
	And Saldo de estoque = "30"
	And Saldo de estoque ID_ESTOQUE_SEM_PRESENCA = "0"

Scenario: Sem produtos
	Given Definir saldo de estoque = "40"
	When Chamar saida com qtde_a_sair = "50", qtde_autorizada_sem_presenca = "0"
	And msg_erro "quantidade inconsistente"

Scenario: Produtos spe
	Given Definir saldo de estoque = "40"
	When Chamar saida com qtde_a_sair = "50", qtde_autorizada_sem_presenca = "10"
	Then Retorno qtde_estoque_vendido = "50", qtde_estoque_sem_presenca = "10"
	And msg_erro vazia
	And Saldo de estoque = "0"
	And Saldo de estoque ID_ESTOQUE_SEM_PRESENCA = "10"

Scenario: muitos produtos
	Given Definir saldo de estoque = "40"
	When Chamar saida com qtde_a_sair = "50", qtde_autorizada_sem_presenca = "20"
	Then Retorno qtde_estoque_vendido = "50", qtde_estoque_sem_presenca = "10"
	And msg_erro vazia
	And Saldo de estoque = "0"
	And Saldo de estoque ID_ESTOQUE_SEM_PRESENCA = "10"


#chamadas consecutivas para acumular
Scenario: chamadas acumuladas
	Given Definir saldo de estoque = "40"
	When Chamar saida com qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	Then Retorno qtde_estoque_vendido = "10", qtde_estoque_sem_presenca = "0"
	And msg_erro vazia
	And Saldo de estoque = "30"
	And Saldo de estoque ID_ESTOQUE_SEM_PRESENCA = "0"
And Terminar este testes

