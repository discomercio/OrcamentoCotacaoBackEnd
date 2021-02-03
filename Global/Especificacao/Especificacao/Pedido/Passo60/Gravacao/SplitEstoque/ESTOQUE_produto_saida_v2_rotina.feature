@ignore
Feature: ESTOQUE_produto_saida_v2_rotina
#vamos testar a rotina ESTOQUE_produto_saida_v2

Background: Configuracao
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	Given Usar produto "dois" como fabricante = "003", produto = "003221"
	And usar id_nfe_emitente "CD" = "4003"
	And usar id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" = "ID_ESTOQUE_SEM_PRESENCA"
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
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	Then Retorno qtde_estoque_vendido = "10", qtde_estoque_sem_presenca = "0"
	And msg_erro vazia
	And Saldo de estoque = "30" em id_nfe_emitente "CD"
	And Saldo de estoque = "0" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA"

Scenario: ESTOQUE_produto_saida_v2_rotina para estoque sem presença
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "100", qtde_autorizada_sem_presenca = "60"
	And msg_erro vazia
	And Saldo de estoque = "0" em id_nfe_emitente "CD"
	And Saldo de estoque = "60" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA"


Scenario: Produtos spe
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "10"
	Then Retorno qtde_estoque_vendido = "40", qtde_estoque_sem_presenca = "10"
	And msg_erro vazia
	And Saldo de estoque = "0" em id_nfe_emitente "CD"
	And Saldo de estoque = "10" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA"

Scenario: muitos produtos sem presenca
	#neste cenário, aceitamos o pedido - temos MAIS estoque do que o cliente aceitou
	Given Definir saldo de estoque = "40"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "20"
	Then Retorno qtde_estoque_vendido = "40", qtde_estoque_sem_presenca = "10"
	And msg_erro vazia
	And Saldo de estoque = "0" em id_nfe_emitente "CD"
	And Saldo de estoque = "10" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA"



Scenario: Sem produtos
	#o usuário não concordou em comprar sem presença no estoque
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "0"
	And msg_erro "quantidade inconsistente"


Scenario: Sem produtos - 2
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "5"
	And msg_erro "quantidade inconsistente"



#chamadas consecutivas para acumular
Scenario: chamadas acumuladas
	Given Definir saldo de estoque = "40" para id_nfe_emitente "CD" e produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "20", qtde_autorizada_sem_presenca = "0"
	Then Retorno qtde_estoque_vendido = "20", qtde_estoque_sem_presenca = "0"
	And msg_erro vazia
	And Saldo de estoque = "20" em id_nfe_emitente "CD" para produto "um" 

	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Retorno qtde_estoque_vendido = "15", qtde_estoque_sem_presenca = "0"
	And msg_erro vazia
	And Saldo de estoque = "5" em id_nfe_emitente "CD" para produto "um" 

	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	And msg_erro "quantidade inconsistente"

	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "11", qtde_autorizada_sem_presenca = "8"
	Then Retorno qtde_estoque_vendido = "5", qtde_estoque_sem_presenca = "6"
	And msg_erro vazia
	And Saldo de estoque = "0" em id_nfe_emitente "CD"
	And Saldo de estoque = "6" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" para produto "um" 

	#aqui já está sem estoque e esperando 6
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "12", qtde_autorizada_sem_presenca = "12"
	Then Retorno qtde_estoque_vendido = "0", qtde_estoque_sem_presenca = "12"
	And msg_erro vazia
	And Saldo de estoque = "0" em id_nfe_emitente "CD"
	And Saldo de estoque = "18" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" para produto "um"


Scenario: Respeita ordem FIFO
	Given Zerar todo o estoque
	Given Inserir movimento de estoque = "12" para id_nfe_emitente "CD" e produto "um" com valor "100"
	Given Inserir movimento de estoque = "14" para id_nfe_emitente "CD" e produto "um" com valor "222"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "100" saldo de estoque = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "222" saldo de estoque = "11"
	And Saldo de estoque = "0" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" para produto "um" 


#testar com mais de um produto
Scenario: testar com mais de um produto
	Given Zerar todo o estoque
	Given Inserir movimento de estoque = "12" para id_nfe_emitente "CD" e produto "um" com valor "100"
	Given Inserir movimento de estoque = "14" para id_nfe_emitente "CD" e produto "um" com valor "222"
	Given Inserir movimento de estoque = "21" para id_nfe_emitente "CD" e produto "dois" com valor "333"
	Given Inserir movimento de estoque = "24" para id_nfe_emitente "CD" e produto "dois" com valor "444"

	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "dois", qtde_a_sair = "44", qtde_autorizada_sem_presenca = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "333" saldo de estoque = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "444" saldo de estoque = "1"
	And Saldo de estoque = "0" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" para produto "dois" 

	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "100" saldo de estoque = "0"
	Then Verificar estoque para id_nfe_emitente "CD" e produto "um" com valor "222" saldo de estoque = "11"
	And Saldo de estoque = "0" em id_nfe_emitente "ID_ESTOQUE_SEM_PRESENCA" para produto "um" 


